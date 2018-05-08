#include "stdafx.h"
void DistanceTransform2D_NormL1Ret(double *map, int rows, int cols, double *dt, uchar *path);
void DistanceTransform2D_NormL2Ret(double *map, int rows, int cols, double *dt, int *path);
void DT_code2index(int code, int *dim, int *index);
bool DistanceTransform2D_NormL1_GetClosest(int r, int c, uchar *path, int rows, int cols, int *outr, int *outc);
bool DistanceTransform2D_NormL2_GetClosest(int r, int c, int *path, int rows, int cols, int *outr, int *outc);