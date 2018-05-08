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


void
DT_code2index(int code, int *dim, int *index)
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

bool DistanceTransform2D_NormL1_GetClosest(int r, int c, uchar *path, int rows, int cols, int *outr, int *outc)
{
	int code, dim, index;
	int r0 = r, c0 = c;
	bool moved = false;

	while ((code = path[r0*cols + c0]) != 0)
	{
		moved = true;
		DT_code2index(code, &dim, &index);
		if (dim == 2)
			r0 += index;
		if (dim == 1)
			c0 += index;
	}

	*outr = r0;
	*outc = c0;
	return moved;
}

///////////////////////////////////////////////////////////////////////////

static void dist_transform_1D_L2NormRet(double *line, int len, double *res, int step, int *aux_int, double *aux_double, int *closer, int cstep, int base)
{
	double *DT = res;
	double *L = line;
	int    *v = aux_int;
	double *z = aux_double;
	double s;

	int q, ind;

	int k = 0;
	v[0] = 0;
	z[0] = -DBL_MAX;
	z[1] = DBL_MAX;

	for (q = 1; q < len; q++)
	{
		k++;
		do
		{
			k--;
			s = ((L[q] + q * q) - (L[v[k]] + v[k] * v[k])) / (2 * q - 2 * v[k]);
		} while (s <= z[k]);

		k++;
		v[k] = q;
		z[k] = s;
		z[k + 1] = DBL_MAX;
	}

	k = 0;
	for (q = 0, ind = 0; q < len; q++, ind += step)
	{
		while (z[k + 1] < q)
			k++;

		DT[ind] = (q - v[k]) * (q - v[k]) + L[v[k]];
		closer[base + q * cstep] = v[k];
	}

	return;
}

void DistanceTransform2D_NormL2Ret(double *map, int rows, int cols, double *dt, int *path)
{
	double *tmp = new double[rows*cols];
	int len = MAX(rows, cols) + 1;
	int *v = new int[len];
	double *z = new double[len];

	for (int r = 0; r < rows; r++)
		dist_transform_1D_L2NormRet(&(map[r*cols]), cols, &(tmp[r]), rows, v, z, &(path[r*cols]), 1, 0);

	for (int c = 0; c < cols; c++)
		dist_transform_1D_L2NormRet(&(tmp[c*rows]), rows, &(dt[c]), cols, v, z, &(path[c]), cols, rows*cols);

	delete[] tmp;
	delete[] v;
	delete[] z;

	return;
}

bool DistanceTransform2D_NormL2_GetClosest(int r, int c, int *path, int rows, int cols, int *outr, int *outc)
{
	int r0 = path[(r + rows)*cols + c];
	int c0 = path[r0*cols + c];
	*outr = r0;
	*outc = c0;
	if (r0 != r || c0 != c)
		return true;
	else
		return false;
}

/*
int main(int argc, char *argv[])
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
	GenMatType *path2 = GMCreate(dimg->rows * 2, dimg->cols, GM_ITYPE);

	//    GenMatType *dtr = DistanceTransform2D_NormBox(dimg, w, w);
	DistanceTransform2D_NormL1Ret(dimg->ddata[0], dimg->rows, dimg->cols, dt->ddata[0], path->udata[0]);
	GMShow(dt, 0);
	GMShow(path, 0);

	DistanceTransform2D_NormL2Ret(dimg->ddata[0], dimg->rows, dimg->cols, dt->ddata[0], path2->idata[0]);
	GMShow(dt, 0);
	GMShow(path2, 0);

	//memcpy((char *)dt->ddata[0], (char *)DT, dimg->rows * dimg->cols * sizeof(double));
	//memcpy((char *)path->udata[0], (char *)PATH, dimg->rows * dimg->cols * sizeof(uchar));

	GMShow(uimg, 0);
	for (int r = 0; r < path->rows; r++)
		for (int c = 0; c < path->cols; c++)
		{
			int r0, c0;
			//bool moved = DistanceTransform2D_NormL1_GetClosest(r, c, path->udata[0], path->rows, path->cols, &r0, &c0);
			bool moved = DistanceTransform2D_NormL2_GetClosest(r, c, path2->idata[0], path->rows, path->cols, &r0, &c0);

			//fprintf(stderr, "%d %d ---> %d %d\n", r, c, r0, c0);

			if (moved)
				uimg->udata[r][c] = uimg->udata[r0][c0];
		}
	GMShow(uimg, 0);


	exit(0);
}
*/
