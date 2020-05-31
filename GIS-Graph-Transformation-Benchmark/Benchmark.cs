using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using GIS_Graph_Transformation;

namespace GIS_Graph_Transformation_Benchmark
{
    class Benchmark
    {
        private static int _median = 0;
        private static long _tnMedian = 0;
        private static string _medianMsg = "";
        private static bool _printedMedianMsg = false;

        static void Main()
        {
            int averageOf = 10;

            int[] vertices = { 1000, 2000, 3000, 4000 };
            FindMedian(vertices);
            PrepareMedianMsg(averageOf, createFiles: false);
            RunBenchmark(vertices, averageOf, createFiles: false);
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
                file.WriteLine($"n\t(n)[ms]\tq(n)");
                foreach (int v in vertices)
                {
                    long result = 0;

                    // check if should print median
                    if(v == _median)
                    {
                        Console.WriteLine($"Writig median message (median: {_median}, medianTime: {_tnMedian})...");
                        file.WriteLine(_medianMsg);
                        _printedMedianMsg = true;
                        continue;
                    }
                    else if(_median < v && !_printedMedianMsg)
                    {
                        Console.WriteLine($"Writig median message (median: {_median}, medianTime: {_tnMedian})...");
                        file.WriteLine(_medianMsg);
                        _printedMedianMsg = true;
                    }

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
                    file.WriteLine($"{v}\t{result}\t{CalculateQn(v,result)}");
                }
            }
            Console.WriteLine("Benchmark finished successfully");
        }

        private static void FindMedian(int[] numbers)
        {
            if (numbers.Length == 0)
            {
                _median = 0;
                return;
            }
                
            else
            {
                if (numbers.Length % 2 == 0)
                    _median = (numbers[numbers.Length / 2 - 1] + numbers[numbers.Length / 2]) / 2;
                else
                    _median = numbers[numbers.Length / 2];
            }

            return;
        }

        private static void PrepareMedianMsg(int averageOf, int generatedMaxDistance = 3, bool createFiles = false)
        {
            DataIO dio = new DataIO();
            VertexToEdge vte = new VertexToEdge();
            
            _tnMedian = 0;
            _printedMedianMsg = false;

            Console.WriteLine($"Calculating median time ({_median} vertices)...");
            for (int i = 0; i < averageOf; ++i)
            {
                string inputFilename = createFiles ? $"testGenerated-{_median}-{i}-median.txt" : "";
                string outputFilename = createFiles ? $"testOutput-{_median}-{i}-median.txt" : "";

                Console.WriteLine($"Now transforming: {_median}-{i}...");
                Stopwatch watch = new Stopwatch();
                Dictionary<string, Vertex> input = dio.GenerateGraph(_median, generatedMaxDistance, generatedFile: inputFilename);
                watch.Start();
                Dictionary<string, Vertex> output = vte.Transform(input);
                watch.Stop();
                _tnMedian += watch.ElapsedMilliseconds;

                if (createFiles)
                    dio.SaveGraph(output, outputFilename);
            }

            _tnMedian /= averageOf;
            _medianMsg = $"{_median}\t{_tnMedian}\t1.0\t(median)";

            Console.WriteLine($"Median time calculated ({_tnMedian}ms)");
            return;
        }

        private static double CalculateQn(int n, long tn)
        {
            double result = (double)(tn * _median * _median) / (n * n * _tnMedian);
            return result;
        }
    }
}
