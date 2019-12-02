using System.Runtime.InteropServices;
using System.Text;
using System;
using System.IO;
using Rhino;
using Rhino.Commands;

namespace TopoRhino
{
  internal static class Import
  {
       #if _WINDOWS
       public const string lib = "dllTopo.dll";
       #else
       public const string lib = "libdllTopo.dylib";
       #endif
  
}
    /// <summary>
    /// http://msdn.microsoft.com/en-us/library/aa288468(VS.71).aspx
    /// http://www.mono-project.com/docs/advanced/pinvoke/
    /// </summary>mesh.points = new CPoint[mesh.n_vertices];
   internal static class TopoCreator
  {
        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr readXML(string xmlpath);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int deleteStructure(IntPtr topoData);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int partNumber(IntPtr topoData);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool initMesh(int partID, ref CMesh mesh, IntPtr topoData);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool assignMesh(int partID, ref CMesh mesh, IntPtr topoData);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool isBoundary(int partID, IntPtr topoData);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float ComputeGroundHeight(IntPtr topoData);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void setParaDouble(string name, double value, IntPtr topoData);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void refresh(IntPtr topoData);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void preview(IntPtr topoData);

        public static bool getMesh(int partID, Rhino.Geometry.Mesh rhino_mesh, IntPtr topoData)
        {
            CMesh cmesh = new CMesh();
            if(initMesh(partID, ref cmesh, topoData))
            {
                cmesh.points = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(CPoint)) * cmesh.n_vertices);
                cmesh.faces = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(CFace)) * cmesh.n_faces);
                if (assignMesh(partID, ref cmesh, topoData))
                {
                    float ground_height = ComputeGroundHeight(topoData);
                    Console.WriteLine("Height {0}", ground_height);
                    for (int id = 0; id < cmesh.n_vertices; id++)
                    {
                        IntPtr pPoint = new IntPtr(cmesh.points.ToInt64() + id * Marshal.SizeOf(typeof(CPoint)));
                        Single[] point = new Single[3];
                        Marshal.Copy(pPoint, point, 0, 3);
                        rhino_mesh.Vertices.Add(point[0], -point[2], point[1] - ground_height);
                    }

                    for (int id = 0; id < cmesh.n_faces; id++)
                    {
                        IntPtr pFace = new IntPtr(cmesh.faces.ToInt64() + id * Marshal.SizeOf(typeof(CFace)));
                        Int32[] face = new Int32[3];
                        Marshal.Copy(pFace, face, 0, 3);
                        rhino_mesh.Faces.AddFace(face[0], face[1], face[2]);
                    }

                    Marshal.FreeHGlobal(cmesh.faces);
                    Marshal.FreeHGlobal(cmesh.points);
                    return true;
                }
            }
            return false;
        }
    }

  
}
