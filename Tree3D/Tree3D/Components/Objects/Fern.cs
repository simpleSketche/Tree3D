using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tree3D
{
    public class Fern : Blend, IBlend
    {

        public Fern(List<GeometryBase> geometries) : base(geometries)
        {
            Geometries = geometries;
        }

    }
}
