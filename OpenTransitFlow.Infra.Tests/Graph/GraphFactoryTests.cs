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

        [Test]
        public void CheckIfTrackIsBlockedBasicTest()
        {
            var node1 = new NetworkGraphVertex("N1", [0, 0]);
            var node2 = new NetworkGraphVertex("N2", [0, 0]);
            var node3 = new NetworkGraphVertex("N3", [0, 0]);
            var node4 = new NetworkGraphVertex("N4", [0, 0]);

            var track1 = new NetworkGraphEdge(node1, node2, "T1");
            var track2 = new NetworkGraphEdge(node2, node3, "T2");
            var track3 = new NetworkGraphEdge(node3, node4, "T3");

            var path = new NetworkGraphEdge[] { track1, track2, track3 };

            var testTrain1 = new Train("Train1", "", 100, path, track1);
            var testTrain2 = new Train("Train2", "", 100, path, track2 );

            Assert.That(track1.IsBlocked, Is.EqualTo(true));
            Assert.That(track2.IsBlocked, Is.EqualTo(true));
            Assert.That(track3.IsBlocked, Is.EqualTo(false));


            Assert.That(testTrain1.MoveVehicle(), Is.EqualTo(VehicleMoveStatus.UNKNOWN));
            testTrain2.Remove();
            Assert.That(testTrain1.MoveVehicle(), Is.EqualTo(VehicleMoveStatus.MOVING));
        }

        [Test]
        public void CheckIfTrackIsBlockedWithSignalTest()
        {
            var node1 = new NetworkGraphVertex("N1", [0, 0]);
            var node2 = new NetworkGraphVertexSignal("N2", [0, 0]);
            var node3 = new NetworkGraphVertexSignal("N3", [0, 0]);
            var node4 = new NetworkGraphVertex("N4", [0, 0]);

            var track1 = new NetworkGraphEdge(node1, node2, "T1");
            var track2 = new NetworkGraphEdge(node2, node3, "T2");
            var track3 = new NetworkGraphEdge(node3, node4, "T3");

            var path = new NetworkGraphEdge[] { track1, track2, track3 };

            var testTrain1 = new Train("Train1", "", 100, path, track1);
            var testTrain2 = new Train("Train2", "", 100, path, track2);

            Assert.That(testTrain1.MoveVehicle(), Is.EqualTo(VehicleMoveStatus.STOPPED_AT_SIGNAL));
            Assert.That(testTrain2.MoveVehicle(), Is.EqualTo(VehicleMoveStatus.MOVING));
            Assert.That(testTrain2.MoveVehicle(), Is.EqualTo(VehicleMoveStatus.REACHED_DESTINATION));
            Assert.That(testTrain1.MoveVehicle(), Is.EqualTo(VehicleMoveStatus.MOVING));
            Assert.That(testTrain1.MoveVehicle(), Is.EqualTo(VehicleMoveStatus.STOPPED_AT_SIGNAL));
            testTrain2.Remove();
            Assert.That(testTrain1.MoveVehicle(), Is.EqualTo(VehicleMoveStatus.MOVING));
            Assert.That(testTrain1.MoveVehicle(), Is.EqualTo(VehicleMoveStatus.REACHED_DESTINATION));
        }

        [Test]
        public void CheckTwoWayTrack()
        {

            var factory = new GraphFactory();

            var node1 = new NetworkGraphVertex("N1", [0, 0]);
            var node2 = new NetworkGraphVertexSignal("N2", [0, 0]);
            var node3 = new NetworkGraphVertexSignal("N3", [0, 0]);
            var node4 = new NetworkGraphVertex("N4", [0, 0]);
            factory.AddNodeRange(new BaseNetworkGraphVertex[] { node1, node2 });
            factory.AddEdge("T1", "N1", "N2", true);
            var track1I = factory.GetEdge("T1I");
            var track1II = factory.GetEdge("T1II");
            
            var track2 = new NetworkGraphEdge(node2, node3, "T2");
            var track3 = new NetworkGraphEdge(node4, node2, "T3");

            var path1 = new NetworkGraphEdge[] { track1I, track2 };
            var path2 = new NetworkGraphEdge[] {track3, track1II };

            var testTrain1 = new Train("Train1", "", 100, path1, track1I);
            var testTrain2 = new Train("Train2", "", 100, path2, track3);
            Assert.That(track1I.UUID, Is.EqualTo("T1I"));
            Assert.That(track1II.UUID, Is.EqualTo("T1II"));
            Assert.That(track1I.oppositeDirectionEdge.UUID, Is.EqualTo("T1II"));
            Assert.That(track1II.oppositeDirectionEdge.UUID, Is.EqualTo("T1I"));

            Assert.That(testTrain2.MoveVehicle(), Is.EqualTo(VehicleMoveStatus.STOPPED_AT_SIGNAL));
            Assert.That(testTrain1.MoveVehicle(), Is.EqualTo(VehicleMoveStatus.MOVING));

            Assert.That(testTrain1.MoveVehicle(), Is.EqualTo(VehicleMoveStatus.REACHED_DESTINATION));
        }

        [Test]
        public void CheckSwitchNode()
        {

            var factory = new GraphFactory();
            var node0 = new NetworkGraphVertex("N0", [0, 0]);
            var node1 = new NetworkGraphVertexSignal("N1", [0, 0]);
            var node2 = new NetworkGraphVertexSwitch("N2", [0, 0]);
            var node3 = new NetworkGraphVertex("N3", [0, 0]);
            var node4 = new NetworkGraphVertex("N4", [0, 0]);
            factory.AddNodeRange(new BaseNetworkGraphVertex[] { node0, node1, node2 });
            factory.AddEdge("T0", "N0", "N1", true);
            factory.AddEdge("T1", "N1", "N2", true);
            var track0I = factory.GetEdge("T0I");
            var track0II = factory.GetEdge("T0II");
            var track1I = factory.GetEdge("T1I");
            var track1II = factory.GetEdge("T1II");

            node2.AddInboundTrack(track1I);
            
           
            var track2 = new NetworkGraphEdge(node2, node3, "T2");
            var track3 = new NetworkGraphEdge(node4, node2, "T3");
            node2.AddOutboundTracks(track2);
            node2.AddOutboundTracks(track3);
            var path1 = new NetworkGraphEdge[] { track0I ,track1I, track2 };
            var path2 = new NetworkGraphEdge[] {track3, track1II, track0II };

            var testTrain1 = new Train("Train1", "", 100, path1, track0I);
            //var testTrain2 = new Train("Train2", "", 100, path2, track3);
            
            //Assert.That(testTrain2.MoveVehicle(), Is.EqualTo(VehicleMoveStatus.STOPPED_AT_SIGNAL));
            Assert.That(testTrain1.MoveVehicle(), Is.EqualTo(VehicleMoveStatus.MOVING));
            Assert.That(testTrain1.MoveVehicle(), Is.EqualTo(VehicleMoveStatus.MOVING));
            Assert.That(testTrain1.MoveVehicle(), Is.EqualTo(VehicleMoveStatus.REACHED_DESTINATION));
        }
    }
}
