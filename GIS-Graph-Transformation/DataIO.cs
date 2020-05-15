using Microsoft.Msagl.Drawing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GIS_Graph_Transformation
{
    class DataIO
    {
        public Dictionary<string, Vertex> LoadGraph(string fileName = "input.txt")
        {
            // check if file exists, if not - generate it
            if (!File.Exists(fileName))
            {
                Console.WriteLine($"[WARNING] File \"{fileName}\" does not exist. Generating random data.");
                return GenerateGraph();
            }

            Dictionary<string, Vertex> graph = new Dictionary<string, Vertex>();
            // load data from file
            char[] delims = { '\t', '\n', '\r', ' ' };
            string[] edges = File.ReadAllText(fileName)
                .Split(delims, StringSplitOptions.RemoveEmptyEntries);

            // check if data is valid
            if(edges.Length % 2 != 0)
            {
                Console.WriteLine("[WARNING] Input graph invalid - one edge has no target vertex");
                return graph;
            }

            // create the graph
            for (int i = 0; i < edges.Length - 1; i += 2)
            {
                string from = edges[i], to = edges[i + 1];

                // check if the graph contains current vertices
                if (!graph.ContainsKey(from))
                    graph[from] = new Vertex();
                if (!graph.ContainsKey(to))
                    graph[to] = new Vertex();

                graph[from].AddOutEdge(" ", to);
                graph[to].AddInEdge(" ", from);
            }

            return graph;
        }

        public void SaveGraph(Dictionary<string, Vertex> graph, string fileName="output.txt")
        {
            using (StreamWriter file = new StreamWriter(fileName))
            {
                foreach (KeyValuePair<string, Vertex> v in graph)
                    foreach (Pair e in v.Value.OutEdge)
                        file.WriteLine($"{v.Key} {e.Id} {e.Vertex}");
            }
        }

        public Dictionary<string, Vertex> GenerateGraph(int vertexCount = 30, int maxVertexDistance = 2)
        {
            Dictionary<string, Vertex> graph = new Dictionary<string, Vertex>();
            Dictionary<string, int> layers = new Dictionary<string, int>();
            int vertexSequence = 0;
            Random rand = new Random();

            // insert inital vertex
            graph[(++vertexSequence).ToString()] = new Vertex();
            layers[vertexSequence.ToString()] = 0;
            
            // build graph
            for(int i = 0; i < vertexCount-1; ++i)
            {
                Vertex newVert = new Vertex();
                string newVertId = (++vertexSequence).ToString();
                int newVertLayer;

                // select random vertex id
                List<string> values = Enumerable.ToList(graph.Keys);
                string randVertexId = values[rand.Next(values.Count)];

                // get random distance (must be in <1,maxVertexDistance> range)
                int dist = rand.Next(maxVertexDistance) + 1;

                // insert forward or backward
                if(rand.Next(2) == 0) // forward
                {
                    newVert.AddInEdge(" ", randVertexId);
                    graph[randVertexId].AddOutEdge(" ", newVertId);
                    newVertLayer = layers[randVertexId] + dist;
                }
                else // backward
                {
                    newVert.AddOutEdge(" ", randVertexId);
                    graph[randVertexId].AddInEdge(" ", newVertId);
                    newVertLayer = layers[randVertexId] - dist;
                }

                layers[newVertId] = newVertLayer;
                graph[newVertId] = newVert;
            }
            
            return graph;
        }
    }
}
