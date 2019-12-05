using System.Runtime.InteropServices;
using System.Text;
using System;
using System.IO;
using Rhino;
using Rhino.Commands;
using System.Collections.Generic;

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
        internal static extern IntPtr initStructure();
       
        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int deleteStructure(IntPtr topoData);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int deletePolyLineRhino(IntPtr lines);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern bool deletePolyMeshRhino(IntPtr mesh);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int partNumber(IntPtr topoData);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr initPartMeshPtr(int partID, IntPtr topoData);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr initCrossMeshPtr(IntPtr topoData);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr initTextureMeshPtr(IntPtr topoData);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr initPatternMeshPtr(IntPtr topoData);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr initBaseMesh2DPtr(IntPtr topoData);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int isNull(IntPtr mesh);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int getNVertices(IntPtr mesh);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int getNFaces(IntPtr mesh);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void copyVertexI(IntPtr mesh, int vID, IntPtr point);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void copyFaceI(IntPtr mesh, int fID, IntPtr face);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int getNVertexGroup(IntPtr mesh);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int getNFaceGroup(IntPtr mesh);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int getNVertexGroupI(IntPtr mesh, int vgID);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int getNFaceGroupI(IntPtr mesh, int vfID);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void copyVertexGroupI(IntPtr mesh, int vgID, IntPtr vg);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void copyFaceGroupI(IntPtr mesh, int fgID, IntPtr fg);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int isBoundary(int partID, IntPtr topoData);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern float ComputeGroundHeight(IntPtr topoData);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void setParaDouble(string name, double value, IntPtr topoData);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void setPatternAngle(double angle, IntPtr topoData);
        
        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void setPatternXY(double x, double y, IntPtr topoData);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void setPatternScale(double s, IntPtr topoData);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void refresh(IntPtr topoData);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void preview(IntPtr topoData);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int getNPolyLines(IntPtr polylines);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern int getNPolyLineI(IntPtr polylines, int lID);

        [DllImport(Import.lib, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void copyPolyLineI(IntPtr polylines, int lID, IntPtr points);





        public static bool getPartMesh(int partID, Rhino.Geometry.Mesh rhino_mesh, IntPtr topoData)
        {
            IntPtr polymesh = initPartMeshPtr(partID, topoData);
            int nullmesh = isNull(polymesh);
            if(nullmesh == 0)
            {
                int n_vertices = getNVertices(polymesh);
                int n_faces = getNFaces(polymesh);

                //cmesh.points = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(CPoint)) * cmesh.n_vertices);
                //cmesh.faces = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(CFace)) * cmesh.n_faces);

                float ground_height = ComputeGroundHeight(topoData);
                for (int id = 0; id < n_vertices; id++)
                {
                    IntPtr pPoint = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * 3);
                    copyVertexI(polymesh, id, pPoint);
                    Single[] point = new Single[3];
                    Marshal.Copy(pPoint, point, 0, 3);
                    rhino_mesh.Vertices.Add(point[0], -point[2], point[1] - ground_height);
                    Marshal.FreeHGlobal(pPoint);
                }

                for (int id = 0; id < n_faces; id++)
                {
                    IntPtr pFace = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * 3);
                    copyFaceI(polymesh, id, pFace);
                    Int32[] face = new Int32[3];
                    Marshal.Copy(pFace, face, 0, 3);
                    rhino_mesh.Faces.AddFace(face[0], face[1], face[2]);
                    Marshal.FreeHGlobal(pFace);
                }


                //create ngon mesh
                int nngon = getNVertexGroup(polymesh);
                for(int id = 0; id < nngon; id++)
                {
                    int[] meshFaceIndexList;
                    int[] meshVertexIndexList;

                    //face group
                    {
                        int nFaceGroupI = getNFaceGroupI(polymesh, id);
                        IntPtr pFaceGroupI = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * nFaceGroupI);
                        copyFaceGroupI(polymesh, id, pFaceGroupI);
                        meshFaceIndexList = new int[nFaceGroupI];
                        Marshal.Copy(pFaceGroupI, meshFaceIndexList, 0, nFaceGroupI);
                        Marshal.FreeHGlobal(pFaceGroupI);
                    }

                    //vertex group
                    {
                        int nVertexGroupI = getNVertexGroupI(polymesh, id);
                        IntPtr pVertexGroupI = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(int)) * nVertexGroupI);
                        copyVertexGroupI(polymesh, id, pVertexGroupI);
                        meshVertexIndexList = new int[nVertexGroupI];
                        Marshal.Copy(pVertexGroupI, meshVertexIndexList, 0, nVertexGroupI);
                        Marshal.FreeHGlobal(pVertexGroupI);
                    }

                    Rhino.Geometry.MeshNgon ngon = Rhino.Geometry.MeshNgon.Create(meshVertexIndexList, meshFaceIndexList);
                    rhino_mesh.Ngons.AddNgon(ngon); 
                }
                deletePolyMeshRhino(polymesh);
                return true;
            }
            return false;
        }

        public static bool getPolyLines(List<Rhino.Geometry.Polyline> rhino_polylines, IntPtr pPolylines, bool flipYZ = true)
        {
            if(isNull(pPolylines) == 0)
            {
                int nPolyLines = getNPolyLines(pPolylines);
                for(int id = 0; id < nPolyLines; id++)
                {
                    int nPolyLineI = getNPolyLineI(pPolylines, id);
                    IntPtr pPoints = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(float)) * nPolyLineI * 3);
                    copyPolyLineI(pPolylines, id, pPoints);
                    float[] points = new float[3 * nPolyLineI];
                    Marshal.Copy(pPoints, points, 0, 3 * nPolyLineI);
                    Marshal.FreeHGlobal(pPoints);

                    //use points to construct polylines
                    List<Rhino.Geometry.Point3d> rhino_points = new List<Rhino.Geometry.Point3d>(nPolyLineI);
                    for (int jd = 0; jd < nPolyLineI; jd++)
                    {
                        Rhino.Geometry.Point3d pt = new Rhino.Geometry.Point3d();
                        if (flipYZ)
                        {
                            pt = new Rhino.Geometry.Point3d(points[jd * 3], -points[jd * 3 + 2], points[jd * 3 + 1]);
                        }
                        else
                        {
                            pt = new Rhino.Geometry.Point3d(points[jd * 3], points[jd * 3 + 1], points[jd * 3 + 2]);
                        }
                        
                        rhino_points.Add(pt);
                    }

                    Rhino.Geometry.Polyline polyline = new Rhino.Geometry.Polyline(rhino_points);
                    rhino_polylines.Add(polyline);
                }
                return true;
            }
            return false;
        }
    }
}
