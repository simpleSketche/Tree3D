using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Tree3D;

namespace Tree3DGH
{
    public class CreateBlend : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the CreateBlend class.
        /// </summary>
        public CreateBlend()
          : base("Create Blend", "CreateBlend",
              "CreateBlend",
              Constants.tabName, Constants.subCategory)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGeometryParameter("Geometries", "Geometries", "Geometry representations of the Blend object", GH_ParamAccess.list);
            pManager.AddTextParameter("Blend Type", "BlendType", "The type of the blend, pick this input from the BlendType gh Component.", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Blend", "Blend", "The output blend object", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<GeometryBase> geos = new List<GeometryBase>();
            string blendType = "";

            if(!DA.GetDataList(0, geos)) { return; }
            if (!DA.GetData(1, ref blendType)) { return; }

            IBlend outputBlend = null;

            if(blendType == Constants.Gypsophila)
            {
                outputBlend = new Gypsophila(geos);
            }
            else if(blendType == Constants.Fern)
            {
                outputBlend = new Fern(geos);
            }

            DA.SetData(0, outputBlend);
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
            get { return new Guid("1bd0e070-83a8-493c-9ab8-5685b4428532"); }
        }
    }
}