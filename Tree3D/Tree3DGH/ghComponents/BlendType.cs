using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Grasshopper.Kernel;
using Rhino.Geometry;
using Tree3D;

namespace Tree3DGH.ghComponents
{
    public class BlendType : GH_Component
    {
        public string SelBlendOption { get; set; }

        /// <summary>
        /// Initializes a new instance of the BlendType class.
        /// </summary>
        public BlendType()
          : base("BlendType", "BlendType",
              "Right click the component to pick the BlendType",
              Constants.tabName, Constants.subCategory)
        {
            this.SelBlendOption = " ";
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBooleanParameter("Confirm Button", "ConfirmBtn", "Click Button to confirm the selected option", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddTextParameter("Selected Blend Type", "SelBlendType", "The selected blend type", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            bool btn = false;

            if(!DA.GetData(0, ref btn)) { return; }

            DA.SetData(0, this.SelBlendOption);
        }

        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            base.AppendAdditionalComponentMenuItems(menu);
            Menu_AppendItem(menu, Constants.Gypsophila, SelGpysophila);
            Menu_AppendItem(menu, Constants.Fern, SelFern);
        }

        private void SelGpysophila(object sender, EventArgs e)
        {
            this.SelBlendOption = Constants.Gypsophila;
        }

        private void SelFern(object sender, EventArgs e)
        {
            this.SelBlendOption = Constants.Fern;
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
            get { return new Guid("68f7b0c8-2660-4e0c-851f-cb1d93981e84"); }
        }
    }
}