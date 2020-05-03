using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GIS_Graph_Transformation
{
    class Program
    {
        static void Main()
        {
            GraphVisualizer gv = new GraphVisualizer();
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

            gv.Visualize(graph);
        }
    }
}
