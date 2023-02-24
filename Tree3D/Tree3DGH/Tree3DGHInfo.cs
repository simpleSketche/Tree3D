using System;
using System.Drawing;
using Grasshopper.Kernel;

namespace Tree3DGH
{
    public class Tree3DGHInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "Tree3DGH";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return null;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("12598f81-11d7-4a67-94a2-dcd8db6ccd91");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "";
            }
        }
    }
}
