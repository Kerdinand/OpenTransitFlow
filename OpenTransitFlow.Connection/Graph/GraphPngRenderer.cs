using QuikGraph;
using QuikGraph.Graphviz;

namespace OpenTransitFlow.Connection.Graph
{
    public static class GraphPngRenderer
    {
        /// <summary>
        /// Helper Method to generate a dot string that can be copied and used to view in browser...
        /// </summary>
        public static void Renderer(BidirectionalGraph<NetworkGraphVertex, NetworkGraphEdge> graph)
        {
            var graphviz = new GraphvizAlgorithm<NetworkGraphVertex, NetworkGraphEdge>(graph);
            string dotGraph = graphviz.Generate();
            Console.WriteLine(dotGraph);
        }
    }
}
