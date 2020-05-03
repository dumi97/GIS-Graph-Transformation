using System;
using System.Collections.Generic;
using System.IO;

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

        public Dictionary<string, Vertex> GenerateGraph()
        {
            Dictionary<string, Vertex> graph = new Dictionary<string, Vertex>();

            //TODO
            Console.WriteLine("TODO");

            return graph;
        }
    }
}
