#include "stdafx.h"
#include <opencv2/opencv.hpp>
#include "distanceTransform.h"

/*
 *   System include
 */
 
//#include <stdinclude.h>
 
/*
 *   Global include
 */
 
//#include <macros.h>
 
/*
 *   Public header of the used modules
 */
 
//#include <error_lib.h>
//#include <genmat2.h>

/* 
   dim:  1 | 2 | 3
   ind:   -1 | 1
*/
static uchar DT_index2code(int dim, int ind)
{
	return((dim - 1) * 2 + ((ind > 0) ? 1 : 2));
}


void DT_code2index(int code, int *dim, int *index)
{
	*dim = (code + 1) / 2;
	*index = (code % 2 == 0) ? -1 : 1;
}

static void dist_transf_1D_L1NormRet(double *line, uchar *path, int dim, int len)
{
	int q;

	for (q = 1; q < len; q++)
		if (line[q - 1] + 1 < line[q])
		{
			line[q] = line[q - 1] + 1;
			path[q] = DT_index2code(dim, -1);
		}

	for (q = len - 2; q >= 0; q--)
		if (line[q + 1] + 1 < line[q])
		{
			line[q] = line[q + 1] + 1;
			path[q] = DT_index2code(dim, 1);
		}

	return;
}

static void dist_transform_1D_L1NormRet(double *line, uchar *path, int dim, int len, int step)
{
	int q, ind;

	if (step == 1)
	{
		dist_transf_1D_L1NormRet(line, path, dim, len);
		return;
	}

	for (q = 1, ind = step; q < len; q++, ind += step)
		if (line[ind - step] + 1 < line[ind])
		{
			line[ind] = line[ind - step] + 1;
			path[ind] = DT_index2code(dim, -1);
		}

	for (q = len - 2, ind = (len - 2)*step; q >= 0; q--, ind -= step)
		if (line[ind + step] + 1 < line[ind])
		{
			line[ind] = line[ind + step] + 1;
			path[ind] = DT_index2code(dim, 1);
		}

	return;
}

void DistanceTransform2D_NormL1Ret(double *map, int rows, int cols, double *dt, uchar *path)
{
	//double *dt = new double[rows*cols];
	memcpy((char *)dt, (char *)map, rows * cols * sizeof(double));

	//uchar *path = new uchar[rows*cols];

	memset((char *)path, 0, rows * cols * sizeof(uchar));
	int r, c;
	//GMSet(0.0, path);

	for (r = 0; r < rows; r++)
		dist_transform_1D_L1NormRet(&(dt[r*cols]), &(path[r*cols]), 1, cols, 1);

	for (c = 0; c < cols; c++)
		dist_transform_1D_L1NormRet(&(dt[c]), &(path[c]), 2, rows, cols);

	//DT = dt;
	//PATH = path;

	return;
}

/*
int
main(int argc, char *argv[])
{
	GenMatType *img = GMRead(argv[1], NULL);
	GMShow(img, 0);
	//GMPrint(img);

	GenMatType *uimg = GMCastUchar(img, NULL);
	GenMatType *dimg = GMCastDouble(uimg, NULL);
	GMShow(dimg, 0);

	double thr = 50;

	for (int r = 0; r < dimg->rows; r++)
		for (int c = 0; c < dimg->cols; c++)
		{
			if (dimg->ddata[r][c] > thr)
				dimg->ddata[r][c] = 1.0e20;
			else
				dimg->ddata[r][c] = 0.0;
		}

	GMShow(dimg, 0);

	//GenMatType *DTres[2] = {NULL, NULL};

	GenMatType *dt = GMCreate(dimg->rows, dimg->cols, GM_DTYPE);
	GenMatType *path = GMCreate(dimg->rows, dimg->cols, GM_UTYPE);

	//    GenMatType *dtr = DistanceTransform2D_NormBox(dimg, w, w);
	DistanceTransform2D_NormL1Ret(dimg->ddata[0], dimg->rows, dimg->cols, dt->ddata[0], path->udata[0]);


	//memcpy((char *)dt->ddata[0], (char *)DT, dimg->rows * dimg->cols * sizeof(double));
	//memcpy((char *)path->udata[0], (char *)PATH, dimg->rows * dimg->cols * sizeof(uchar));

	GMShow(dt, 0);
	GMShow(path, 0);

	GMShow(uimg, 0);
	for (int r = 0; r < path->rows; r++)
		for (int c = 0; c < path->cols; c++)
		{
			int code; // = DTres[1]->ddata[r][c];
			int dim, index;
			int r0 = r, c0 = c;
			bool moved = false;

			while ((code = path->udata[r0][c0]) != 0)
			{
				moved = true;
				DT_code2index(code, &dim, &index);
				if (dim == 2)
					r0 += index;
				if (dim == 1)
					c0 += index;
			}

			//fprintf(stderr, "%d %d ---> %d %d\n", r, c, r0, c0);

			if (moved)
				uimg->udata[r][c] = uimg->udata[r0][c0];
		}
	GMShow(uimg, 0);


	exit(0);
}
*/