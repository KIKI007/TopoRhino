using System;
using System.Runtime.InteropServices;

namespace TopoRhino
{
    static class Constants
    {
        public const int MAXIMUM_MESHSIZE = 4096;
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
}
