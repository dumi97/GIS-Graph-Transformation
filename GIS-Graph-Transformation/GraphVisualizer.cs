using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Microsoft.Msagl.GraphViewerGdi;
using Microsoft.Msagl.Drawing;

namespace GIS_Graph_Transformation
{
    public class GraphVisualizer
    {
        private Form _form;

        public GraphVisualizer()
        {
            _form = new Form();
        }

        public void Visualize(Dictionary<string, Vertex> inputGraph, string graphName = "Graph")
        {
            Close();
            GViewer _viewer = new GViewer();
            Graph _graph = new Graph(graphName)
            {
                LayoutAlgorithmSettings = new Microsoft.Msagl.Prototype.Ranking.RankingLayoutSettings()
            };

            foreach (KeyValuePair<string, Vertex> v in inputGraph)
                foreach (Pair e in v.Value.InEdge)
                {
                    if(string.IsNullOrEmpty(e.Id) || e.Id.Equals("null"))
                        _graph.AddEdge(e.Vertex, v.Key).Attr.AddStyle(Style.Dashed);
                    else
                        _graph.AddEdge(e.Vertex, v.Key).LabelText = e.Id;
                }

            //bind the graph to the viewer 
            _viewer.Graph = _graph;
            //associate the viewer with the form 
            _form.SuspendLayout();
            _viewer.Dock = DockStyle.Fill;
            _form.Controls.Add(_viewer);
            _form.ResumeLayout();
            //show the form 
            _form.ShowDialog();
            //Console.ReadKey();
        }

        public void Close()
        {
            if(_form != null)
                _form.Close();
            _form = new Form();
        }
    }
}
