using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

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

            /*
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
            */

            //Dictionary<string, Vertex> graph = dio.LoadGraph();
            Dictionary<string, Vertex> graph = dio.GenerateGraph(4, renumerate: true, generatedFile:"testGenerated.txt");
            inputVisualizer.Visualize(graph, "Test Generated graph");
            Dictionary<string, Vertex> outG = vte.Transform(graph);
            dio.SaveGraph(outG, "testOutput.txt");
            outputVisualizer.Visualize(outG, "Vertex to edge graph");
            

            /* Nie działa, bo coś te podprogramy się nie zamykają jak powinny
            Task[] tasks = new Task[2];
            tasks[0] = Task.Run(() => { inputVisualizer.Visualize(graph, "Test Generated graph"); });
            tasks[1] = Task.Run(() => { outputVisualizer.Visualize(vte.Transform(graph), "Vertex to edge graph"); });
            try
            {
                Task.WaitAll(tasks);
            }
            catch (AggregateException ae)
            {
                Console.WriteLine("One or more exceptions occurred: ");
                foreach (var ex in ae.Flatten().InnerExceptions)
                    Console.WriteLine("   {0}", ex.Message);
            }
            */
        }
    }
}
