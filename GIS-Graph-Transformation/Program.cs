using System;
using System.Collections.Generic;
using Mono.Options;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;

namespace GIS_Graph_Transformation
{
    class Program
    {
        static void Main(string[] args)
        {
            // cmd argument variables
            bool showHelp = false, textOnly = false;
            bool generateData = false;
            bool renumerate = false, reletter = false;
            string genFileName = "";
            int genCount = 30, genVertDist = 3;
            string inputFile = "input.txt";
            string outputFile = "output.txt";

            OptionSet opt = new OptionSet() {
                { "g|generate", "generate the input graph",
                    v => generateData = v != null },
                { "f|generatedName=", "(string) name of the file to save the generated input graph\n" +
                    "empty string (\"\") means no file will be generated\n" +
                    "no effect if not generating the input graph\n" +
                    "default: <empty>",
                    (string v) => genFileName = v },
                { "c|generatedCount=", "(int) how many vertices to generate for the input graph\n" +
                    "no effect if not generating the input graph\n" +
                    "default: 30",
                    (int v) => genCount = v },
                { "d|generatedMaxDist=", "(int) the maximum distance between two vertices in the generated graph\n" +
                    "no effect if not generating the input graph\n" +
                    "default: 3",
                    (int v) => genVertDist = v },
                { "n|generatedRenumerate", "renumerate the generated graph so that the vertices on the lowest level have the lowest numbers\n" +
                    "this may decrease performance\n" +
                    "no effect if not generating the input graph",
                    v => renumerate = v != null },
                { "l|reletter", "reletter the output graph so that the vertices are letterd continuously\n" +
                    "this may decrease performance",
                    v => reletter = v != null },
                { "i|input=", "(string) the input file\n" +
                    "default: input.txt",
                    (string v) => inputFile = v },
                { "o|output=", "(string) the output file\n" +
                    "type \"\" for no output file\n" +
                    "default: output.txt",
                    (string v) => outputFile = v },
                { "t", "text only, do not visualise the input and output graphs\n",
                    v => textOnly = v != null },
                { "h|help",  "show this message and exit",
                    v => showHelp = v != null },
            };

            // parse and apply cmd arguments
            List<string> extra;
            try
            {
                extra = opt.Parse(args);
            }
            catch (OptionException e)
            {
                Console.Write("Vertex To Edge Activity Graph: ");
                Console.WriteLine(e.Message);
                Console.WriteLine("Try 'gis_gt.exe -h' for more information.");
                return;
            }

            // handle unknown arguments
            if (extra.Count != 0)
            {
                foreach (string s in extra)
                    Console.WriteLine($"[ERROR] Unknown argument: {s}");
                Console.WriteLine($"Try 'gis_gt.exe -h' for a full list of arguments.");
                return;
            }

            if (showHelp)
            {
                ShowHelp(opt);
                return;
            }

            DataIO dio = new DataIO();
            VertexToEdge vte = new VertexToEdge();
            GraphVisualizer inputVisualizer = new GraphVisualizer();
            GraphVisualizer outputVisualizer = new GraphVisualizer();
            Dictionary<string, Vertex> graph;
            Stopwatch watch = new Stopwatch();

            if (generateData)
            {
                Console.WriteLine("Generating input graph...");
                graph = dio.GenerateGraph(genCount, genVertDist, renumerate, genFileName);
            }
            else
            {
                Console.WriteLine($"Loading input graph from {inputFile}...");
                graph = dio.LoadGraph(inputFile);
            }

            // error while loading graph
            if (graph.Count == 0)
                return;

            if(!textOnly)
            {
                Console.WriteLine("Visualizing input graph...");
                inputVisualizer.Visualize(graph, "Input graph");
            }

            Console.WriteLine("Transforming input graph...");
            watch.Start();
            Dictionary<string, Vertex> outG = vte.Transform(graph, reletter);
            watch.Stop();
            Console.WriteLine($"Done in {watch.ElapsedMilliseconds} milliseconds");

            if(!string.IsNullOrEmpty(outputFile))
            {
                Console.WriteLine($"Saving output graph to {outputFile}...");
                dio.SaveGraph(outG, outputFile);
            }

            if(!textOnly)
            {
                Console.WriteLine("Visualizing output graph...");
                outputVisualizer.Visualize(outG, "Vertex to edge graph");
            }
            Console.WriteLine("Finished");

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

        private static void ShowHelp(OptionSet p)
        {
            Console.WriteLine("Usage: gis_gt.exe [OPTIONS]");
            Console.WriteLine("Transforms input Vertex Activiti Graph to its edge representation.");
            Console.WriteLine();
            Console.WriteLine("Options:");
            p.WriteOptionDescriptions(Console.Out);
        }
    }
}
