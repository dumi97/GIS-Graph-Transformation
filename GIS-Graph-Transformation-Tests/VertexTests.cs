using System;
using System.Collections.Generic;
using NUnit.Framework;
using GIS_Graph_Transformation;

namespace GIS_Graph_Transformation_Tests
{
    class VertexTests
    {
        [Test]
        public void ConstructorTest()
        {
            Vertex test1 = new Vertex();
            Vertex test2 = new Vertex(
                null,
                new List<Pair> { new Pair("1", "A"), new Pair("2", "B") }
                );
            Vertex test3 = new Vertex(
                new List<Pair> { new Pair("3", "D"), new Pair("4", "E") },
                new List<Pair> { new Pair("5", "F"), new Pair("5", "F") }
                );

            try
            {
                Assert.IsTrue(test1.InEdge.Count == 0 && test1.OutEdge.Count == 0, "Vertex 1 constructed wrong");
                Assert.IsTrue(test2.InEdge.Count == 0 && test2.OutEdge.Count == 2, "Vertex 2 constructed wrong");
                Assert.IsTrue(test2.OutEdge.Contains(new Pair("1", "A")), "Vertex 2 constructed wrong");
                Assert.IsTrue(test2.OutEdge.Contains(new Pair("2", "B")), "Vertex 2 constructed wrong");
                Assert.IsTrue(test3.InEdge.Count == 2 && test3.OutEdge.Count == 2, "Vertex 3 constructed wrong");
                Assert.IsTrue(test3.InEdge.Contains(new Pair("3", "D")), "Vertex 3 constructed wrong");
                Assert.IsTrue(test3.InEdge.Contains(new Pair("4", "E")), "Vertex 3 constructed wrong");
                Assert.IsTrue(test3.OutEdge.Contains(new Pair("5", "F")), "Vertex 3 constructed wrong");
                Assert.IsFalse(test3.OutEdge.Contains(new Pair("F", "5")), "Vertex 3 constructed wrong");
                Assert.IsFalse(test3.OutEdge.Contains(new Pair("3", "D")), "Vertex 3 constructed wrong");
            }
            catch(NullReferenceException)
            {
                Assert.Fail("Null reference exception while constructiong objects");
            }
        }

        [Test]
        public void AddRemoveEdgeTest()
        {
            Vertex test1 = new Vertex();

            Assert.AreEqual(0, test1.InEdge.Count, "Vertex constructed with unexpected in-edges");
            Assert.AreEqual(0, test1.OutEdge.Count, "Vertex constructed with unexpected out-edges");

            test1.AddInEdge("1", "A");
            Assert.AreEqual(1, test1.InEdge.Count, "Vertex in-edge not added");
            Assert.AreEqual(0, test1.OutEdge.Count, "Vertex in-edge adding affected out-edge");
            Assert.IsTrue(test1.InEdge.Contains(new Pair("1", "A")), "Vertex does not contain correct in-edge");

            test1.RemoveInEdge(new Pair("1", "A"));
            Assert.AreEqual(0, test1.InEdge.Count, "Vertex in-edge not removed");
            Assert.AreEqual(0, test1.OutEdge.Count, "Vertex in-edge removing affected out-edges");

            test1.AddOutEdge("2", "B");
            Assert.AreEqual(0, test1.InEdge.Count, "Vertex out-edge adding affected in-edge");
            Assert.AreEqual(1, test1.OutEdge.Count, "Vertex out-edge not added");
            Assert.IsTrue(test1.OutEdge.Contains(new Pair("2", "B")), "Vertex does not contain correct out-edge");

            test1.RemoveOutEdge(new Pair("2", "B"));
            Assert.AreEqual(0, test1.InEdge.Count, "Vertex out-edge removing affected in-edges");
            Assert.AreEqual(0, test1.OutEdge.Count, "Vertex out-edge not removed");
        }
    }
}
