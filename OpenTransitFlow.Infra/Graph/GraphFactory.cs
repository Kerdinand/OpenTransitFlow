using OpenTransitFlow.Connection;
using QuikGraph;
using QuikGraph.Algorithms;
using QuikGraph.Graphviz;

namespace OpenTransitFlow.Infra.Graph
{
    internal class GraphFactory
    {
        public static BidirectionalGraph<NetworkGraphVertex, NetworkGraphEdge> Create(IEnumerable<BaseTrackJson> tracks)
        {
            var graph = new BidirectionalGraph<NetworkGraphVertex, NetworkGraphEdge>(true);

            var nodes = new HashSet<string>();
            var edges = new HashSet<(string, string)>();


            /// TODO: Adding of Edges. Before that DTO and frontend class needs to be updated :/
            foreach (BaseTrackJson track in tracks)
            {
                nodes.Add(track.inboundDiverging);
                nodes.Add(track.outboundDiverging);
                nodes.Add(track.inboundTrack);
                nodes.Add(track.outboundTrack);
            }

            foreach (BaseTrackJson track in tracks)
            {
                edges.Add((track.uuid, track.outboundTrack));
                edges.Add((track.uuid, track.inboundTrack));
                edges.Add((track.uuid, track.inboundDiverging));
                edges.Add((track.uuid, track.outboundDiverging));
            }
            nodes.Remove("");
            edges.RemoveWhere((entry) => String.IsNullOrWhiteSpace(entry.Item1) || String.IsNullOrWhiteSpace(entry.Item2));

            foreach (var entry in nodes)
            {
                graph.AddVertex(new NetworkGraphVertex(entry, [0, 0]));
            }

            foreach (var entry in edges)
            {
                graph.AddEdge(new NetworkGraphEdge(graph.Vertices.Where(node => node.uuid == entry.Item1).First(), graph.Vertices.Where(node => node.uuid == entry.Item1).First()));
            }
            return graph;
        }
    }
}
