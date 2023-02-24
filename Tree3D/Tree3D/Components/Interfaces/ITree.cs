using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace Tree3D
{
    public interface ITree
    {
        /// <summary>
        /// The Central Leaders of the tree
        /// </summary>
        ICentralLeader CentralLeaders { get; set; }

        ITree Clone();
    }
}
