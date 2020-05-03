using System;
using NUnit.Framework;
using GIS_Graph_Transformation;

namespace GIS_Graph_Transformation_Tests
{
    public class PairTests
    {
        private Pair _p1, _p2, _p3, _p4, _p5;

        [SetUp]
        public void Setup()
        {
            _p1 = new Pair("1", "A");
            _p2 = new Pair("1", "A");
            _p3 = new Pair("A", "1");
            _p4 = new Pair("1", "B");
            _p5 = new Pair("3", "C");
        }

        [Test]
        public void ConstructorTest()
        {
            Pair test1 = new Pair();
            Pair test2 = new Pair(vertex: "A");
            try
            {
                Assert.IsTrue(test1.Id is null && test1.Vertex is null, "Pair 1 constructed wrong");
                Assert.IsTrue(test2.Id is null && test2.Vertex.Equals("A"), "Pair 2 constructed wrong");
            }
            catch (NullReferenceException)
            {
                Assert.Fail("Null reference exception while constructiong objects");
            }
        }

        [Test]
        public void EqualsTest()
        {
            Assert.AreEqual(_p1, _p2, "Points 1 and 2 are not equal");
            Assert.AreNotEqual(_p1, _p3, "Points 1 and 3 are equal");
            Assert.AreNotEqual(_p1, _p4, "Points 1 and 4 are equal");
            Assert.AreNotEqual(_p1, _p5, "Points 1 and 5 are equal");
        }
    }
}