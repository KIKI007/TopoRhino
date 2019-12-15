using System;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.DocObjects;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.UI;

namespace MyRhino
{
    [System.Runtime.InteropServices.Guid("a68845b8-fced-4cb2-bcf4-8e377933905f")]
    public class MyRhinoCommand : Rhino.Commands.Command
    {
        public MyRhinoCommand()
        {
            // Rhino only creates one instance of each command class defined in a
            // plug-in, so it is safe to store a refence in a static property.
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        public static MyRhinoCommand Instance { get; private set; }

        public override string EnglishName => "ComputeContacts";

        protected override Result RunCommand(Rhino.RhinoDoc doc, RunMode mode)
        {

            const ObjectType geometryFilter = ObjectType.Mesh;

            GetObject go = new GetObject();
            go.SetCommandPrompt("Select meshes to compute contacts");
            go.GeometryFilter = geometryFilter;
            go.GroupSelect = true;
            go.SubObjectSelect = false;
            go.EnableClearObjectsOnEntry(false);
            go.EnableUnselectObjectsOnExit(false);
            go.DeselectAllBeforePostSelect = false;

            GetResult res = go.GetMultiple(1, 0);

            if (res != GetResult.Object)
                return Result.Cancel;

            Rhino.RhinoApp.WriteLine(res.GetType().ToString())

            return Result.Success;
        }
    }
}
