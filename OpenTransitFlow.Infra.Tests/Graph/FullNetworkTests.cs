using NUnit.Framework;
using OpenTransitFlow.Infra.Graph;
using OpenTransitFlow.Infra.Simulation;

namespace OpenTransitFlow.Infra.Tests.Graph
{
    /// <summary>
    /// Test class to build time sensetive simulation setup
    /// </summary>
    internal class FullNetworkTests
    {
        [Test]
        public void CircleLineBasicTest()
        {
            var simulationRunner = new Runner();

            var factory = new GraphFactory();

            var node1 = new NetworkGraphVertex("N1", [0, 0]);
            var node2 = new NetworkGraphVertexSignal("N2", [400, 0]);
            var node3 = new NetworkGraphVertex("N3", [2000, 0]);
            var node4 = new NetworkGraphVertexSignal("N4", [2400, 0]);
            var node5 = new NetworkGraphVertex("N5", [3000, 0]);

            factory.AddNodeRange(new BaseNetworkGraphVertex[] { node1, node2, node3, node4, node5 });

            factory.AddEdge("T1", "N1", "N2", true);
            factory.AddEdge("T2", "N2", "N3", true);
            factory.AddEdge("T3", "N3", "N4", true);
            factory.AddEdge("T4", "N4", "N5", true);

            node2.SignalEdge = factory.GetEdge("T2I");
            node4.SignalEdge = factory.GetEdge("T3II");

        }
    }
}
