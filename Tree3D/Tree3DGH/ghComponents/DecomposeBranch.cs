using System;
using System.Collections.Generic;
using Grasshopper;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Rhino.Geometry;
using Tree3D;

namespace Tree3DGH.ghComponents
{
    public class DecomposeBranch : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DecomposeBranch class.
        /// </summary>
        public DecomposeBranch()
          : base("DecomposeBranch", "DecomposeBranch",
              "DecomposeBranch",
              Constants.tabName, Constants.subCategory)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Branch", "Branch", "Branch", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Blends", "Blends", "Blends", GH_ParamAccess.list);
            pManager.AddGenericParameter("Geos", "Geos", "Geos", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {

            IBranch branch = null;
            if(!DA.GetData(0, ref branch)) { return; }

            List<IBlend> blends = new List<IBlend>();
            DataTree<GeometryBase> dataGHTree = new Grasshopper.DataTree<GeometryBase>();

            int curPath = 0;
            foreach (IBlend blend in branch.PlantParts)
            {
                blends.Add(blend);
                dataGHTree.AddRange(blend.Geometries, new GH_Path(curPath));
            }

            DA.SetDataList(0, blends);
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
            get { return new Guid("91cf341f-a8d4-49d4-b44b-150f5e107340"); }
        }
    }
}