using System;
using System.Runtime.InteropServices;

namespace TopoGrasshopper
{
    [StructLayout(LayoutKind.Sequential)]
    struct CPoint
    {
        public CPoint(float _x, float _y, float _z)
        {
            x = _x;
            y = _y;
            z = _z;
        }
        public float x, y, z;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct CFace
    {
        public CFace(int _a, int _b, int _c)
        {
            a = _a;
            b = _b;
            c = _c;
        }
        public int a, b, c;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct CMesh
    {
        public IntPtr points;
        public IntPtr faces;
        public int n_vertices;
        public int n_faces;
    }
}
