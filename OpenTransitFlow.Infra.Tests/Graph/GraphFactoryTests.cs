using NUnit.Framework;
using OpenTransitFlow.Infra.Graph;
using System.Numerics;

namespace OpenTransitFlow.Infra.Tests.Graph
{
    internal class GraphFactoryTests
    {
        [Test]
        public void GraphInitTest()
        {
            GraphFactory factory = new GraphFactory();
            factory.AddTestTrack();
            var result = GraphPngRenderer.Renderer(factory.GetGraph());
            Assert.That("digraph G {\r\n0;\r\n1;\r\n2;\r\n3;\r\n4;\r\n5;\r\n0 -> 4;\r\n1 -> 4;\r\n1 -> 5;\r\n2 -> 4;\r\n3 -> 5;\r\n4 -> 0;\r\n4 -> 1;\r\n5 -> 1;\r\n5 -> 2;\r\n5 -> 3;\r\n}", Is.EqualTo(result.ToString()));
        }

        [Test]
        public void TestVmax()
        {
            NetworkGraphVertex start = new NetworkGraphVertex("start", new Vector2(0, 0));
            NetworkGraphVertex end = new NetworkGraphVertex("end", new Vector2(0, 10));
            NetworkGraphEdge testEdge160 = new NetworkGraphEdge(start, end, "TestTrack");
            NetworkGraphEdge testEdge100 = new NetworkGraphEdge(start, end, "TestTrack100");

            testEdge100.vmaxFunction = edge => 100;
            
            Assert.That(testEdge100.vmax,Is.EqualTo(100));
            Assert.That(testEdge160.vmax, Is.EqualTo(160));
        }

        [Test]
        public void TestIfVehicleCanMove()
        {
            var node1 = new NetworkGraphVertex("start", [0, 0]);
            var signal = new NetworkGraphVertexSignal("signal", [10, 0]);
            var node2 = new NetworkGraphVertex("end", [20, 0]);

            var track1 = new NetworkGraphEdge(node1, signal, "T1");
            var track2 = new NetworkGraphEdge(signal, node2, "T2");

            var path = new NetworkGraphEdge[] { track1, track2 };

            var testTrain = new Train("testTrain", "", 100, path, track1);

            Assert.That(testTrain.currentEdge.UUID, Is.EqualTo("T1"));
            var result = testTrain.MoveVehicle();
            Assert.That(testTrain.currentEdge.UUID, Is.EqualTo("T2"));
            Assert.That(result, Is.EqualTo(VehicleMoveStatus.MOVING));
            result = testTrain.MoveVehicle();
            Assert.That(testTrain.currentEdge.UUID, Is.EqualTo("T2"));
            Assert.That(result, Is.EqualTo(VehicleMoveStatus.REACHED_DESTINATION));
        }
    }
}
