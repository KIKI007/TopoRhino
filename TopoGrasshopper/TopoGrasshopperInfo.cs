using System;
using System.Drawing;
using Grasshopper;
using Grasshopper.Kernel;

namespace TopoGrasshopper
{
    public class TopoGrasshopperInfo : GH_AssemblyInfo
    {
        public override string Name => "TopoGrasshopper Info";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "TopoCreator";

        public override Guid Id => new Guid("80594C2D-5CBE-4398-93D4-E1D5B463AD90");

        //Return a string identifying you or your company.
        public override string AuthorName => "Ziqi Wang";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "";
    }
}