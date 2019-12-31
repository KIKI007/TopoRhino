using System;
using System.Runtime.InteropServices;

namespace TopoRhino
{
    [StructLayout(LayoutKind.Sequential)]
    struct CMesh
    {
        public IntPtr points;
        public IntPtr faces;
        public int n_vertices;
        public int n_faces;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct CPolyLines
    {
        public IntPtr points;
        public IntPtr sta_ends;
        public IntPtr atBoundary;

        public int n_polyline;
        public int n_points;
    }

}
