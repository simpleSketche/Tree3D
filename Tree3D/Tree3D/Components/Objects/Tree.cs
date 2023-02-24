using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tree3D
{
    public class Tree : ITree
    {
        public ICentralLeader CentralLeaders { get; set; }
        

        public ITree Clone()
        {
            ICentralLeader leader = this.CentralLeaders.Clone();

            ITree tree = new Tree();
            tree.CentralLeaders = leader;

            return tree;
        }
    }
}
