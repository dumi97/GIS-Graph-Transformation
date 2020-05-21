using System.Collections.Generic;
using NUnit.Framework;
using GIS_Graph_Transformation;

namespace GIS_Graph_Transformation_Tests
{
    class VertexToEdgeTests
    {
        private Dictionary<string, Vertex> _g1, _g2;

        [SetUp]
        public void Setup()
        {
            // test graph 1
            _g1 = new Dictionary<string, Vertex>
            {
                ["1"] = new Vertex(),
                ["2"] = new Vertex(),
                ["3"] = new Vertex()
            };
            _g1["1"].AddOutEdge(" ", "3");
            _g1["3"].AddInEdge(" ", "1");
            _g1["2"].AddOutEdge(" ", "3");
            _g1["3"].AddInEdge(" ", "2");

            // test graph 2
            _g2 = new Dictionary<string, Vertex>
            {
                ["1"] = new Vertex(),
                ["2"] = new Vertex(),
                ["3"] = new Vertex(),
                ["4"] = new Vertex(),
                ["5"] = new Vertex(),
                ["6"] = new Vertex(),
                ["7"] = new Vertex()
            };
            _g2["1"].AddOutEdge(" ", "4");
            _g2["4"].AddInEdge(" ", "1");
            _g2["1"].AddOutEdge(" ", "7");
            _g2["7"].AddInEdge(" ", "1");
            _g2["1"].AddOutEdge(" ", "5");
            _g2["5"].AddInEdge(" ", "1");
            _g2["2"].AddOutEdge(" ", "5");
            _g2["5"].AddInEdge(" ", "2");
            _g2["3"].AddOutEdge(" ", "5");
            _g2["5"].AddInEdge(" ", "3");
            _g2["3"].AddOutEdge(" ", "6");
            _g2["6"].AddInEdge(" ", "3");
            _g2["5"].AddOutEdge(" ", "7");
            _g2["7"].AddInEdge(" ", "5");
        }

        [Test]
        public void TransformTests()
        {
            VertexToEdge vte = new VertexToEdge();
            Dictionary<string, Vertex> _out1, _out2;

            _out1 = vte.Transform(_g1, reletter: true);
            _out2 = vte.Transform(_g2, reletter: true);

            // testing g1 output
            Assert.IsTrue(_out1.ContainsKey("A"), "Test graph 1 output invalid");
            Assert.AreEqual(0, _out1["A"].InEdge.Count, "Test graph 1 output invalid");
            Assert.AreEqual(2, _out1["A"].OutEdge.Count, "Test graph 1 output invalid");
            Assert.IsTrue(_out1.ContainsKey("B"), "Test graph 1 output invalid");
            Assert.AreEqual(2, _out1["B"].InEdge.Count, "Test graph 1 output invalid");
            Assert.AreEqual(1, _out1["B"].OutEdge.Count, "Test graph 1 output invalid");
            Assert.IsTrue(_out1.ContainsKey("C"), "Test graph 1 output invalid");
            Assert.AreEqual(1, _out1["C"].InEdge.Count, "Test graph 1 output invalid");
            Assert.AreEqual(0, _out1["C"].OutEdge.Count, "Test graph 1 output invalid");

            Assert.IsTrue(_out1["A"].OutEdge.Contains(new Pair("1", "B")), "Test graph 1 output invalid");
            Assert.IsTrue(_out1["A"].OutEdge.Contains(new Pair("2", "B")), "Test graph 1 output invalid");
            Assert.IsTrue(_out1["B"].InEdge.Contains(new Pair("1", "A")), "Test graph 1 output invalid");
            Assert.IsTrue(_out1["B"].InEdge.Contains(new Pair("2", "A")), "Test graph 1 output invalid");
            Assert.IsTrue(_out1["B"].OutEdge.Contains(new Pair("3", "C")), "Test graph 1 output invalid");
            Assert.IsTrue(_out1["C"].InEdge.Contains(new Pair("3", "B")), "Test graph 1 output invalid");

            // testing g2 output
            Assert.IsTrue(_out2.ContainsKey("A"), "Test graph 2 output invalid");
            Assert.AreEqual(0, _out2["A"].InEdge.Count, "Test graph 2 output invalid");
            Assert.AreEqual(3, _out2["A"].OutEdge.Count, "Test graph 2 output invalid");
            Assert.IsTrue(_out2.ContainsKey("B"), "Test graph 2 output invalid");
            Assert.AreEqual(1, _out2["B"].InEdge.Count, "Test graph 2 output invalid");
            Assert.AreEqual(3, _out2["B"].OutEdge.Count, "Test graph 2 output invalid");
            Assert.IsTrue(_out2.ContainsKey("C"), "Test graph 2 output invalid");
            Assert.AreEqual(1, _out2["C"].InEdge.Count, "Test graph 2 output invalid");
            Assert.AreEqual(2, _out2["C"].OutEdge.Count, "Test graph 2 output invalid");
            Assert.IsTrue(_out2.ContainsKey("D"), "Test graph 2 output invalid");
            Assert.AreEqual(1, _out2["D"].InEdge.Count, "Test graph 2 output invalid");
            Assert.AreEqual(0, _out2["D"].OutEdge.Count, "Test graph 2 output invalid");
            Assert.IsTrue(_out2.ContainsKey("E"), "Test graph 2 output invalid");
            Assert.AreEqual(2, _out2["E"].InEdge.Count, "Test graph 2 output invalid");
            Assert.AreEqual(1, _out2["E"].OutEdge.Count, "Test graph 2 output invalid");
            Assert.IsTrue(_out2.ContainsKey("F"), "Test graph 2 output invalid");
            Assert.AreEqual(1, _out2["F"].InEdge.Count, "Test graph 2 output invalid");
            Assert.AreEqual(0, _out2["F"].OutEdge.Count, "Test graph 2 output invalid");
            Assert.IsTrue(_out2.ContainsKey("G"), "Test graph 2 output invalid");
            Assert.AreEqual(3, _out2["G"].InEdge.Count, "Test graph 2 output invalid");
            Assert.AreEqual(1, _out2["G"].OutEdge.Count, "Test graph 2 output invalid");
            Assert.IsTrue(_out2.ContainsKey("H"), "Test graph 2 output invalid");
            Assert.AreEqual(1, _out2["H"].InEdge.Count, "Test graph 2 output invalid");
            Assert.AreEqual(0, _out2["H"].OutEdge.Count, "Test graph 2 output invalid");

            Assert.IsTrue(_out2["A"].OutEdge.Contains(new Pair("1", "B")), "Test graph 2 output invalid");
            Assert.IsTrue(_out2["A"].OutEdge.Contains(new Pair("2", "G")), "Test graph 2 output invalid");
            Assert.IsTrue(_out2["A"].OutEdge.Contains(new Pair("3", "C")), "Test graph 2 output invalid");
            Assert.IsTrue(_out2["B"].InEdge.Contains(new Pair("1", "A")), "Test graph 2 output invalid");
            Assert.IsTrue(_out2["B"].OutEdge.Contains(new Pair("4", "D")), "Test graph 2 output invalid");
            Assert.IsTrue(_out2["B"].OutEdge.Contains(new Pair("null", "E")), "Test graph 2 output invalid");
            Assert.IsTrue(_out2["B"].OutEdge.Contains(new Pair("null", "G")), "Test graph 2 output invalid");
            Assert.IsTrue(_out2["C"].InEdge.Contains(new Pair("3", "A")), "Test graph 2 output invalid");
            Assert.IsTrue(_out2["C"].OutEdge.Contains(new Pair("null", "G")), "Test graph 2 output invalid");
            Assert.IsTrue(_out2["C"].OutEdge.Contains(new Pair("6", "H")), "Test graph 2 output invalid");
            Assert.IsTrue(_out2["D"].InEdge.Contains(new Pair("4", "B")), "Test graph 2 output invalid");
            Assert.IsTrue(_out2["E"].InEdge.Contains(new Pair("null", "B")), "Test graph 2 output invalid");
            Assert.IsTrue(_out2["E"].InEdge.Contains(new Pair("5", "G")), "Test graph 2 output invalid");
            Assert.IsTrue(_out2["F"].InEdge.Contains(new Pair("7", "E")), "Test graph 2 output invalid");
            Assert.IsTrue(_out2["G"].InEdge.Contains(new Pair("null", "B")), "Test graph 2 output invalid");
            Assert.IsTrue(_out2["G"].InEdge.Contains(new Pair("2", "A")), "Test graph 2 output invalid");
            Assert.IsTrue(_out2["G"].InEdge.Contains(new Pair("null", "C")), "Test graph 2 output invalid");
            Assert.IsTrue(_out2["G"].OutEdge.Contains(new Pair("5", "E")), "Test graph 2 output invalid");
            Assert.IsTrue(_out2["H"].InEdge.Contains(new Pair("6", "C")), "Test graph 2 output invalid");
        }
    }
}
