using Leap;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CustomLeap
{
    [Serializable]
    public class Hand
    {
        public List<List<float>> armBasis { get; set; }
        public float armWidth { get; set; }
        public float confidence { get; set; }
        public List<float> direction { get; set; }
        public List<float> elbow { get; set; }
        public float grabAngle { get; set; }
        public float grabStrength { get; set; }
        public int id { get; set; }
        public List<float> palmNormal { get; set; }
        public List<float> palmPosition { get; set; }
        public List<float> palmVelocity { get; set; }
        public float palmWidth { get; set; }
        public float pinchDistance { get; set; }
        public float pinchStrength { get; set; }
        public List<List<float>> r { get; set; }
        public double s { get; set; }
        public List<double> sphereCenter { get; set; }
        public double sphereRadius { get; set; }
        public List<float> stabilizedPalmPosition { get; set; }
        public List<double> t { get; set; }
        public float timeVisible { get; set; }
        public string type { get; set; }
        public List<float> wrist { get; set; }


        //Converte un oggetto CustomLeap.Hand in Leap.Hand
        //Non inserisce le fingers
        public Leap.Hand convertToRealHand(long frameid)
        {
            Vector directionVector= new Leap.Vector(direction[0], direction[1], direction[2]);
            Vector palmNormalVector= new Leap.Vector(palmNormal[0], palmNormal[1], palmNormal[2]);

            Leap.Hand rHand = new Leap.Hand();
            rHand.FrameId = frameid;
            rHand.Id = this.id;
            rHand.Confidence = this.confidence;
            rHand.GrabStrength = this.grabStrength;
            rHand.GrabAngle = this.grabAngle;
            rHand.PinchStrength = this.pinchStrength;
            rHand.PinchDistance = this.pinchDistance;
            rHand.PalmWidth = this.palmWidth;
            rHand.IsLeft = (this.type == "left") ? true : false;
            rHand.TimeVisible = this.timeVisible;
            rHand.PalmPosition = new Leap.Vector(palmPosition[0], palmPosition[1], palmPosition[2]);
            rHand.StabilizedPalmPosition = new Leap.Vector(stabilizedPalmPosition[0], stabilizedPalmPosition[1], stabilizedPalmPosition[2]);
            rHand.PalmVelocity = new Leap.Vector(palmVelocity[0], palmVelocity[1], palmVelocity[2]);
            rHand.PalmNormal = palmNormalVector;
            rHand.Direction = directionVector;
            rHand.WristPosition = new Leap.Vector(wrist[0], wrist[1], wrist[2]);

            //Calcoliamo rotation (LeapQuaternion)
            Vector x = directionVector.Cross(palmNormalVector) * -1;
            rHand.Rotation = QuaternionHelper.generateQuaternion(x, palmNormalVector * -1, directionVector * -1);

            //arm
            if (armBasis.Count > 0)
            {
                //ref: https://github.com/logotype/LeapMotionAS3/blob/adca9465d6209a6b98073c9fc50660a05c9a5579/src/com/leapmotion/leap/socket/LeapSocket.as

                Leap.Vector elbowV = new Leap.Vector(elbow[0], elbow[1], elbow[2]);
                Leap.Vector wristV = new Leap.Vector(wrist[0], wrist[1], wrist[2]);
                Leap.Vector diff = elbowV - wristV;

                Leap.LeapQuaternion rotQuat = rHand.IsLeft ? QuaternionHelper.generateQuaternion(new Vector(-armBasis[0][0], -armBasis[0][1], -armBasis[0][2]), new Vector(armBasis[1][0], armBasis[1][1], armBasis[1][2]), new Vector(armBasis[2][0], armBasis[2][1], armBasis[2][2])) :  QuaternionHelper.generateQuaternion(new Vector(armBasis[0][0], armBasis[0][1], armBasis[0][2]), new Vector(armBasis[1][0], armBasis[1][1], armBasis[1][2]), new Vector(armBasis[2][0], armBasis[2][1], armBasis[2][2]));


                //TODO edit rotation
                rHand.Arm = new Leap.Arm(
                                elbowV,
                                wristV,
                                elbowV - wristV,
                                new Leap.Vector(this.armBasis[2][0] * -1, this.armBasis[2][1] * -1, this.armBasis[2][2] * -1),
                                diff.Magnitude,
                                armWidth,
                                rotQuat);
                               
            }
            else
            {
                rHand.Arm = new Leap.Arm();
            }

            //fingers

            rHand.Fingers = new List<Leap.Finger>();

            return rHand;
        }


    }



}

