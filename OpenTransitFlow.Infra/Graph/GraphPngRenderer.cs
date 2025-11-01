using QuikGraph;
using QuikGraph.Graphviz;

namespace OpenTransitFlow.Infra.Graph
{
    public static class GraphPngRenderer
    {
        /// <summary>
        /// Helper Method to generate a dot string that can be copied and used to view in browser...
        /// </summary>
        public static string Renderer(BidirectionalGraph<NetworkGraphVertex, NetworkGraphEdge> graph)
        {
            var graphviz = new GraphvizAlgorithm<NetworkGraphVertex, NetworkGraphEdge>(graph);
            string dotGraph = graphviz.Generate();
            Console.WriteLine(dotGraph);
            return dotGraph;
        }
    }
}
