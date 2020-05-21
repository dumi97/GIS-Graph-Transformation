using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using GIS_Graph_Transformation;

namespace GIS_Graph_Transformation_Benchmark
{
    class Benchmark
    { 
        static void Main()
        {
            int[] vertices = { 10, 50, 100, 500, 1000, 5000, 10000 };
            RunBenchmark(vertices, 5, createFiles: false);

        }

        static void RunBenchmark(int[] vertices, int averageOf, int generatedMaxDistance = 3, bool createFiles = false, string resultFilename = "benchmark_results.txt")
        {
            string startupMessage = $"Graph transformation benchmark with parameters: averageOf={averageOf}, " +
                $"generatedMaxDistance={generatedMaxDistance}";

            Console.WriteLine(startupMessage);
            DataIO dio = new DataIO();
            VertexToEdge vte = new VertexToEdge();

            using (StreamWriter file = new StreamWriter(resultFilename))
            {
                file.WriteLine(startupMessage);
                foreach (int v in vertices)
                {
                    long result = 0;
                    for (int i = 0; i < averageOf; ++i)
                    {
                        string inputFilename = createFiles ? $"testGenerated-{v}-{i}.txt" : "";
                        string outputFilename = createFiles ? $"testOutput-{v}-{i}.txt" : "";

                        Console.WriteLine($"Now transforming: {v}-{i}...");
                        Stopwatch watch = new Stopwatch();
                        Dictionary<string, Vertex> input = dio.GenerateGraph(v, generatedMaxDistance, generatedFile:inputFilename);
                        watch.Start();
                        Dictionary<string, Vertex> output = vte.Transform(input);
                        watch.Stop();

                        result += watch.ElapsedMilliseconds;
                        if (createFiles)
                            dio.SaveGraph(output, outputFilename);
                    }
                    result /= averageOf;
                    file.WriteLine($"{v} vertices = {result} milliseconds");
                }
            }
            Console.WriteLine("Benchmark finished successfully");
        }
    }
}
