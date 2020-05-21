using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace GIS_Graph_Transformation
{
    public class DataIO
    {
        public Dictionary<string, Vertex> LoadGraph(string fileName = "input.txt")
        {
            // check if file exists, if not - generate it
            if (string.IsNullOrEmpty(fileName) || !File.Exists(fileName))
            {
                Console.WriteLine($"[ERROR] File \"{fileName}\" does not exist");
                return new Dictionary<string, Vertex>();
            }

            // load data from file
            char[] delims = { '\t', '\n', '\r', ' ' };
            string[] edges = File.ReadAllText(fileName)
                .Split(delims, StringSplitOptions.RemoveEmptyEntries);
            return StringsToGraph(edges);
        }

        private Dictionary<string, Vertex> StringsToGraph(string[] graphStrings)
        {
            Dictionary<string, Vertex> graph = new Dictionary<string, Vertex>();

            // check if data is valid
            if (graphStrings.Length % 2 != 0)
            {
                Console.WriteLine("[ERROR] Input graph invalid - one edge has no target vertex");
                return graph;
            }

            // create the graph
            for (int i = 0; i < graphStrings.Length - 1; i += 2)
            {
                string from = graphStrings[i], to = graphStrings[i + 1];

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
            if (string.IsNullOrEmpty(fileName))
                return;

            using (StreamWriter file = new StreamWriter(fileName))
            {
                foreach (KeyValuePair<string, Vertex> v in graph)
                    foreach (Pair e in v.Value.OutEdge)
                        file.WriteLine($"{v.Key} {e.Id} {e.Vertex}");
            }
        }

        public Dictionary<string, Vertex> GenerateGraph(int vertexCount = 30, int maxVertexDistance = 3, bool renumerate = false, string generatedFile = "")
        {
            Dictionary<string, Vertex> graph = new Dictionary<string, Vertex>();
            string graphString = "";
            Dictionary<string, int> layers = new Dictionary<string, int>();
            int vertexSequence = 0;
            Random rand = new Random();

            // insert inital vertex
            graph[(++vertexSequence).ToString()] = new Vertex();
            layers[vertexSequence.ToString()] = 0;
            
            // build graph
            for(int i = 0; i < vertexCount-1; ++i)
            {
                // create new vertex
                Vertex newVert = new Vertex();
                string newVertId = (++vertexSequence).ToString();
                int newVertLayer;

                // select random vertex id from current graph
                List<string> values = Enumerable.ToList(graph.Keys);
                string randVertexId = values[rand.Next(values.Count)];

                // get a random distance (must be in <1,maxVertexDistance> range)
                int dist = rand.Next(maxVertexDistance) + 1;

                // insert new vertex in front or behind selected vetex
                if(rand.Next(2) == 0) // in front
                {
                    newVert.AddInEdge(" ", randVertexId);
                    graph[randVertexId].AddOutEdge(" ", newVertId);
                    newVertLayer = layers[randVertexId] + dist;
                    graphString += $"{randVertexId} {newVertId} ";
                }
                else // behind
                {
                    newVert.AddOutEdge(" ", randVertexId);
                    graph[randVertexId].AddInEdge(" ", newVertId);
                    newVertLayer = layers[randVertexId] - dist;
                    graphString += $"{newVertId} {randVertexId} ";
                }

                layers[newVertId] = newVertLayer;
                graph[newVertId] = newVert;
            }
            
            if(renumerate)
            {
                vertexSequence = 0;

                // all vertices with no input edges are on the lowest layer
                foreach (var entry in layers.ToList())
                    if (graph[entry.Key].InEdge.Count == 0)
                        layers[entry.Key] = int.MinValue;

                // sort vertices by their layer
                List<KeyValuePair<string, int>> sortedLayers = layers.ToList();
                sortedLayers.Sort((pair1, pair2) => pair1.Value.CompareTo(pair2.Value));
                Dictionary<int, int> idMapping = new Dictionary<int, int>();

                // create ID mappings
                foreach (KeyValuePair<string, int> entry in sortedLayers)
                    idMapping[int.Parse(entry.Key)] = ++vertexSequence;

                // renumerate all vertices using graph string representation
                string[] edges = graphString.Trim()
                    .Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < edges.Length; ++i)
                    edges[i] = idMapping[int.Parse(edges[i])].ToString();

                graphString = string.Join(" ", edges);

                // convert graph string representation to dictionary representation
                graph = StringsToGraph(edges);
            }

            if(!string.IsNullOrEmpty(generatedFile))
            {
                using (StreamWriter file = new StreamWriter(generatedFile))
                {
                    string[] edges = graphString.Trim()
                        .Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    for (int i = 0; i < edges.Length; i+=2)
                        file.WriteLine($"{edges[i]} {edges[i+1]}");
                }
            }

            return graph;
        }
    }
}
