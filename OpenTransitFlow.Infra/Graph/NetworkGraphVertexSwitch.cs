using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OpenTransitFlow.Infra.Graph
{
    internal class NetworkGraphVertexSwitch : BaseNetworkGraphVertex
    {
        public NetworkGraphVertexSwitch(string uuid, Vector2 pos)
    : base(uuid, pos)
        {
        }

        public NetworkGraphVertexSwitch(string uuid, double[] pos)
            : base(uuid, new Vector2((float)pos[0], (float)pos[1]))
        {
        }

        private NetworkGraphEdge singleInbound;
        private NetworkGraphEdge singleOutbound;

        private Dictionary<string,NetworkGraphEdge> multipleOutboundEdges = new Dictionary<string, NetworkGraphEdge>();
        public Dictionary<string, NetworkGraphEdge> outboundEdges => multipleOutboundEdges;

        public void AddOutboundTracks(NetworkGraphEdge edge)
        {
            base.outboundEdges.Add(edge.UUID, edge);
        }

        public void AddInboundTrack(NetworkGraphEdge edge)
        {
            singleInbound = edge;
            inboundEdges.Clear();
            inboundEdges.Add(edge.UUID, edge);
        }

        public IEnumerable<NetworkGraphEdge> GetValidEdges(NetworkGraphEdge edge)
        {
            if (edge.UUID == inboundEdge.UUID) return outboundEdges.Values;
            if (inboundEdge.Target == this && inboundEdge.oppositeDirectionEdge != null) return new[] { inboundEdge.oppositeDirectionEdge };
            if (inboundEdge.Source == this) return new[] { inboundEdge };

            return Enumerable.Empty<NetworkGraphEdge>();
            
        }

    }
}
