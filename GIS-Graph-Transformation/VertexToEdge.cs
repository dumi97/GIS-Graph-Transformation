using Microsoft.Msagl.Drawing;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace GIS_Graph_Transformation
{
    class VertexToEdge
    {
        private string vertex;
        private Dictionary<string, KeyValuePair<string, string> > addedEdges;
        public VertexToEdge()
        {
            ResetAll();
        }

        private void ResetAll()
        {
            vertex = "B";
            addedEdges = new Dictionary<string, KeyValuePair<string, string>>();
        }

        public Dictionary<string, Vertex> Transform(Dictionary<string, Vertex> inputGraph, bool reletter = false)
        {
            Queue<string> V = new Queue<string>();
            Dictionary<string, Vertex> graph = new Dictionary<string, Vertex>();
            graph.Add("A", new Vertex());
            ResetAll();

            foreach (string g in inputGraph.Keys)
            {
                Vertex value;
                inputGraph.TryGetValue(g, out value);
                if (value.InEdge.Count==0)
                    V.Enqueue(g);
            }

            foreach (string v in V)
                AddTrueEdge(graph, "A", v, vertex);

            while (V.Count > 0)
            {
                //FIFO pierwszy wierzchołek
                string v = V.Dequeue();
                //Weź poprzedniki i następniki z oryginalnego grafu
                Vertex value;
                inputGraph.TryGetValue(v, out value);
                //Znajdź krawędź, która może już istnieć
                KeyValuePair<string, string> valuePair;
                addedEdges.TryGetValue(v, out valuePair);

                //Przejrzyj następniki wierzchołka z FIFO
                for (int i=0; i<value.OutEdge.Count; ++i)
                {
                    Vertex valueNext;
                    inputGraph.TryGetValue(value.OutEdge[i].Vertex, out valueNext);
                    //Sprawdź czy następnik ma już krawędź
                    bool ifExist = false;
                    KeyValuePair<string, string> valuePairNext;
                    addedEdges.TryGetValue(value.OutEdge[i].Vertex, out valuePairNext);

                    if (addedEdges.ContainsKey(value.OutEdge[i].Vertex))
                    {
                        ifExist = true;
                    }
                    else
                    {
                        if (valueNext.OutEdge.Count > 0)
                            V.Enqueue(value.OutEdge[i].Vertex);
                    }

                    if (valueNext.InEdge.Count == 1)
                        AddTrueEdge(graph, valuePair.Value, value.OutEdge[i].Vertex, vertex);
                    else if(valueNext.InEdge.Count > 1)
                    {
                        if(ifExist)
                            AddFictionalEdge(graph, valuePair.Value, "null", valuePairNext.Key);
                        else
                        {
                            string temp = vertex;
                            IncrementVertex();
                            AddTrueEdge(graph, temp, value.OutEdge[i].Vertex, vertex);
                            AddFictionalEdge(graph, valuePair.Value, "null", temp);
                        }
                    }
                }
            }
            //Krok naprawczy
            List<string> toDelete = new List<string>();
            foreach (string g in graph.Keys)
            {
                Vertex value;
                graph.TryGetValue(g, out value);
                if(value.InEdge.Count==1 && value.OutEdge.Count==1)
                {
                    if(value.InEdge[0].Id != "null" && value.OutEdge[0].Id == "null")
                    {
                        Vertex prev, next;
                        graph.TryGetValue(value.InEdge[0].Vertex, out prev);
                        graph.TryGetValue(value.OutEdge[0].Vertex, out next);
                        prev.OutEdge.Remove(new Pair(value.InEdge[0].Id, g));
                        next.InEdge.Remove(new Pair("null", g));
                        prev.AddOutEdge(value.InEdge[0].Id, value.OutEdge[0].Vertex);
                        next.AddInEdge(value.InEdge[0].Id, value.InEdge[0].Vertex);
                        toDelete.Add(g);
                    }
                }
            }
            for (int i = 0; i < toDelete.Count; ++i)
                graph.Remove(toDelete[i]);

            return reletter ? ReletterGraph(graph) : graph;
        }

        private void AddTrueEdge(Dictionary<string, Vertex> graph, string from, string id, string to)
        {
            if(!graph.ContainsKey(from))
                graph.Add(from, new Vertex());
            graph[from].AddOutEdge(id, to);
            if (!graph.ContainsKey(to))
                graph.Add(to, new Vertex());
            graph[to].AddInEdge(id, from);
            addedEdges.Add(id, new KeyValuePair<string, string>(from, to));
            IncrementVertex();
        }
        private void AddFictionalEdge(Dictionary<string, Vertex> graph, string from, string id, string to)
        {
            graph[from].AddOutEdge(id, to);
            if (!graph.ContainsKey(to))
                graph.Add(to, new Vertex());
            graph[to].AddInEdge(id, from);
            IncrementVertex();
        }

        // Alphabetical increment for edges
        private void IncrementVertex()
        {
            for(int i=vertex.Length-1; i>=0; i--)
            {
                if (vertex[i] == 'Z')
                {
                    vertex = vertex.Remove(i, 1);
                    vertex = vertex.Insert(i, "A");
                    if (i == 0)
                    {
                        vertex = vertex.Insert(i, "A");
                        return;
                    }
                }
                else
                {
                    char x = (char)vertex[i];
                    vertex = vertex.Remove(i, 1);
                    vertex = vertex.Insert(i, (++x).ToString());
                    return;
                }
            }
        }

        /**
         * <summary>
         * Reletters the graph so that the vertices are lettered continuously.
         * For aestethic purposes only. Should not be used in benchmarking.
         * </summary>
         * <param name="graph">The graph to be relettered; Should be sorted by key values.</param>
         */
        private Dictionary<string, Vertex> ReletterGraph(Dictionary<string, Vertex> graph)
        {
            Dictionary<string, Vertex> outGraph = new Dictionary<string, Vertex>();
            Dictionary<string, string> letterMapping = new Dictionary<string, string>();
            string graphString = "";
            vertex = "A";

            // create graph string representation
            foreach (KeyValuePair<string, Vertex> v in graph)
            {
                letterMapping[v.Key] = vertex;
                IncrementVertex();
                foreach (Pair e in v.Value.OutEdge)
                    graphString += $"{v.Key} {e.Id} {e.Vertex} ";
            }
                

            // renumerate all vertices using graph string representation
            string[] edges = graphString.Trim()
                .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < edges.Length; i+=3)
            {
                string from = letterMapping[edges[i]];
                string edgeId = edges[i + 1];
                string to = letterMapping[edges[i + 2]];

                // check if the graph contains current vertices
                if (!outGraph.ContainsKey(from))
                    outGraph[from] = new Vertex();
                if (!outGraph.ContainsKey(to))
                    outGraph[to] = new Vertex();

                outGraph[from].AddOutEdge(edgeId, to);
                outGraph[to].AddInEdge(edgeId, from);
            }
                

            return outGraph;
        }
    }
}
