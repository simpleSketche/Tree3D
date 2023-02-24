using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Rhino.Geometry;
using Tree3D;

namespace Tree3DGH.ghComponents
{
    public class VisualizeTree : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the VisualizeTree class.
        /// </summary>
        public VisualizeTree()
          : base("Visualize Tree", "VisualizeTree",
              "Visualize the geometries of the Tree",
              Constants.tabName, Constants.subCategory)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Tree", "Tree", "Tree", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Branches", "Branches", "Branches", GH_ParamAccess.list);
            pManager.AddGeometryParameter("Geometries", "Geometries", "Geometries", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            ITree tree = null;

            if(!DA.GetData(0, ref tree)) { return; }

            List<IBranch> branches = tree.CentralLeaders.Branches;
            List<List<GeometryBase>> geometries = branches.Select(b => b.PlantParts.SelectMany(p => p.Geometries).ToList()).ToList();

            DataTree<GeometryBase> dataGHTree = new Grasshopper.DataTree<GeometryBase>();

            int curPath = 0;

            foreach(List<GeometryBase> geometryList in geometries)
            {
                dataGHTree.AddRange(geometryList, new GH_Path(curPath));
                curPath += 1;
            }

            DA.SetDataList(0, branches);
            DA.SetDataTree(1, dataGHTree);


        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("69464309-6664-4e32-ad3d-042770776c33"); }
        }
    }
}