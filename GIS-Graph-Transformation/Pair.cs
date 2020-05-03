using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GIS_Graph_Transformation
{
    class Pair
    {
        public string Id;
        public string Vertex;

        public Pair(string id = null, string vertex = null)
        {
            Id = id;
            Vertex = vertex;
        }

        public static bool operator ==(Pair e1, Pair e2)
        {
            if (e1 is null)
                return e2 is null;

            return e1.Equals(e2);
        }

        public static bool operator !=(Pair e1, Pair e2)
        {
            return !(e1 == e2);
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Pair e2 = (Pair)obj;
            return (Id == e2.Id && Vertex == e2.Vertex);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode() ^ Vertex.GetHashCode();
        }
    }
}
