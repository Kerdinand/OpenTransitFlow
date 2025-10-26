using OpenTransitFlow.Connection;
using QuikGraph;

namespace OpenTransitFlow.Infra.Graph
{
    internal class GraphFactory
    {
        public static BidirectionalGraph<NetworkGraphVertex, NetworkGraphEdge> Create(IEnumerable<BaseTrackJson> tracks)
        {
            var graph = new BidirectionalGraph<NetworkGraphVertex, NetworkGraphEdge>(true);
            
            var nodes = new HashSet<string>();
            var edges = new HashSet<(string,string)>();


            /// TODO: Adding of Edges. Before that DTO and frontend class needs to be updated :/
            foreach (BaseTrackJson track in tracks) 
            {
                nodes.Add(track.inboundDiverging);
                nodes.Add(track.outboundDiverging);
                nodes.Add(track.inboundTrack);
                nodes.Add(track.outboundTrack);
            }

            return graph;
        }
    }
}
