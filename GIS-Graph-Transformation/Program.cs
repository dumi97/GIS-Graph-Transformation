using System.Collections.Generic;

namespace GIS_Graph_Transformation
{
    class Program
    {
        static void Main()
        {
            DataIO dio = new DataIO();
            VertexToEdge vte = new VertexToEdge();
            GraphVisualizer inputVisualizer = new GraphVisualizer();
            GraphVisualizer outputVisualizer = new GraphVisualizer();

            // SAMPLE GRAPH
            Vertex temp;
            Dictionary<string, Vertex> graph = new Dictionary<string, Vertex>();
            temp = new Vertex(
                null,
                new List<Pair> { new Pair("1", "B"), new Pair("2", "B")}
                );
            graph.Add("A", temp);

            temp = new Vertex(
                new List<Pair> { new Pair("1", "A"), new Pair("2", "A") },
                new List<Pair> { new Pair("3", "C") }
                );
            graph.Add("B", temp);

            temp = new Vertex(
                new List<Pair> { new Pair("3", "B")},
                null
                );
            graph.Add("C", temp);
            // SAMPLE GRAPH END

            // PLACEHOLDER
            vte.Transform(graph);

            inputVisualizer.Visualize(dio.LoadGraph(), "Test Input graph");
            dio.SaveGraph(graph);
            outputVisualizer.Visualize(graph, "Test Output graph");
        }
    }
}
