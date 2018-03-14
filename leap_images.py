#! python

import time
from PIL import Image
import numpy as np
import cv2
import ctypes
import Leap
from matplotlib import pyplot as plt

import warnings

class LeapBridge():
	mx,my,img_leap,img_raw,img_rectified = {},{},{},{},{}
	sides = ["left","right"]
	sbm = None
	
	def __init__(self,oWidth=640,oHeight=240,mapFile=None):
		warnings.filterwarnings('ignore','.*PEP 3118*.',)
		self.controller = Leap.Controller()
		self.controller.set_policy_flags(Leap.Controller.POLICY_IMAGES)
		self.width = oWidth
		self.height = oHeight
		
		#generating blank image here for re-use later:
		self.img_blank = np.zeros([self.height,self.width,3]).astype("uint8")
		self.kernel = np.ones((3,3),np.uint8)
		self.d2d = np.array([[1.0,0.,0.,-5.0638e+02],[0.,1.,0.,-2.3762e+02],[0.,0.,0.,1.3476e+03],[0.,0.,6.9349981e-01,3.503271]])	#depth-to-dist mapping
		print ("*Initializing...")
		while not self.controller.is_connected:		#always a delay prior to active frames
			time.sleep(0.75)		
		print ("*Leap initialized")

		if mapFile is not None:
			print ("*Loading map file at: "+repr(mapFile))
			m = np.load(mapFile).tolist()
			if "mx" in m.keys() and "my" in m.keys():
				mx = m["mx"]
				my = m["my"]
				if all(s in mx.keys() and s in my.keys() for s in self.sides):
					if all(mx[s].shape[0]==self.height and mx[s].shape[1]==self.width and my[s].shape[0]==self.height and my[s].shape[1]==self.width for s in self.sides):
						print ("*Map loaded successfully!")
						self.mx = mx
						self.my = my
					else:
						print ("*ERROR: Map dimensionality mismatch")
				else:
					print ("*ERROR: Map key mismatch")	
			else:
				print ("*ERROR: Map file missing mappings")			
		if len(self.mx)==0 or len(self.my)==0:
			print ("*Missing or invalid map file...")
			self._genMap()
			self.view()

	def view(self):
		self._update()
		for i,s in enumerate(self.sides):
			cv2.imshow(s+" raw",self.img_raw[s])
			#cv2.imshow(s+" rectified",self.img_rectified[s])
			
		cv2.waitKey()
		cv2.destroyAllWindows()
	
	def _update(self):
		self._poll()
		self._genRectified()
	
	#retrieves data from Leap	
	def _poll(self):
		f = self.controller.frame()
		while not f.is_valid:
			print ("*ERROR: invalid Leap frame.")
			time.sleep(0.1)
			f = self.controller.frame()
		imgs = f.images
		for i,s in enumerate(self.sides):
			self.img_leap[s] = imgs[i]
			self.img_raw[s] = self.convertCV(self.img_leap[s])

	def _genRectified(self):
		for i,s in enumerate(self.sides):
			self.img_rectified[s] = self.undistort(self.img_raw[s],self.mx[s],self.my[s])

	def _genMap(self):
		for s in self.sides:
			print ("Generating map for "+s+"...")
			self.mx[s] = (np.ones([self.height,self.width])*-1).astype('float32')
			self.my[s] = (np.ones([self.height,self.width])*-1).astype('float32')

			if len(self.img_leap)==0 or self.img_leap[s] is None:
				self._poll()
			for iy in xrange(self.height):
				for ix in xrange(self.width):
					if self.height/4 <= iy <= self.height-self.height/4 and self.width/4 <= iy <= self.width-self.width/4:
						v_input = Leap.Vector(float(ix)/self.width, float(iy)/self.height,0)
						v_input.x = (v_input.x-self.img_leap[s].ray_offset_x) / self.img_leap[s].ray_scale_x
						v_input.y = (v_input.y-self.img_leap[s].ray_offset_y) / self.img_leap[s].ray_scale_y
						ip = self.img_leap[s].warp(v_input)
						if ip.x>=0 and ip.x<self.width and ip.y>=0 and ip.y<self.height:
							self.mx[s][iy,ix] = int(ip.x)
							self.my[s][iy,ix] = int(ip.y)
						else:
							self.mx[s][iy,ix] = -1
							self.my[s][iy,ix] = -1
						
	def _saveMap(self,fileName="LeapMaps.npy"):
		if len(self.mx)==0 or len(self.my)==0:
			self._genMap()
		np.save(fileName,{"mx":self.mx,"my":self.my})

	def undistort(self,cv_img,mx,my):
		return cv2.remap(cv_img,mx,my,cv2.INTER_LINEAR)
		
	def leap_to_array(self,leap_img):
		imdata = ctypes.cast(leap_img.data.cast().__long__(), ctypes.POINTER(leap_img.width*leap_img.height*ctypes.c_ubyte)).contents
		return np.reshape(np.array(imdata,'int'),(leap_img.height,leap_img.width))

	def convertPIL(self,leap_img):
		return Image.fromarray(self.leap_to_array(leap_img)).convert("RGB")
		
	def convertCV(self,leap_img):
		pil_img = self.convertPIL(leap_img)
		cv_img = np.array(pil_img)[:,:,::-1].copy()		#still has three channels
		crop_img = cv_img[self.height/4:self.height-self.height/4,self.width/4:self.width-self.width/4]
		crop_img = cv2.resize(crop_img,dsize=(self.width,self.height),fx=2,fy=2)		
		crop_img = cv2.cvtColor(crop_img,cv2.COLOR_BGR2GRAY)
		(thresh, img_bin) = cv2.threshold(crop_img, 0, 255, cv2.THRESH_BINARY | cv2.THRESH_OTSU)
		return img_bin

if __name__=="__main__":
	lb = LeapBridge(640,240)
