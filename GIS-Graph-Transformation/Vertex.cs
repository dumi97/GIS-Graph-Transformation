using System;
using System.Collections.Generic;

namespace GIS_Graph_Transformation
{
    class Vertex
    {
        public List<Pair> InEdge { get; private set; }
        public List<Pair> OutEdge { get; private set; }

        public Vertex()
        {
            InEdge = new List<Pair>();
            OutEdge = new List<Pair>();
        }

        public Vertex(List<Pair> inE = null, List<Pair> outE = null)
        {
            InEdge = inE is null ? new List<Pair>() : inE;
            OutEdge = outE is null ? new List<Pair>() : outE;
        }

        public void AddInEdge(string id, string from)
        {
            InEdge.Add(new Pair(id, from));
        }

        public void AddOutEdge(string id, string from)
        {
            OutEdge.Add(new Pair(id, from));
        }

        public void RemoveInEdge(Pair edge)
        {
            InEdge.Remove(edge);
        }

        public void RemoveOutEdge(Pair edge)
        {
            OutEdge.Remove(edge);
        }
    }
}
