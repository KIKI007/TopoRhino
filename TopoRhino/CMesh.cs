using System;
using System.Runtime.InteropServices;

namespace TopoRhino
{
    static class Constants
    {
        public const int MAXIMUM_MESHSIZE = 4096;
        public const int MAXIMUM_POLYLINE_POINTS = 10000;
        public const int MAXIMUM_POLYLINE_FACE = 1000;
    }
    

    [StructLayout(LayoutKind.Sequential)]
    struct CMesh
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.MAXIMUM_MESHSIZE)]
        public float[] points;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.MAXIMUM_MESHSIZE)]
        public int[] faces;
        public int n_vertices;
        public int n_faces;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct CPolyLines
    {
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.MAXIMUM_POLYLINE_POINTS)]
        public float[] points;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.MAXIMUM_POLYLINE_FACE)]
        public int[] sta_ends;
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = Constants.MAXIMUM_POLYLINE_FACE)]
        public int[] atBoundary;

        public int n_polyline;
        public int n_points;
    }

}
