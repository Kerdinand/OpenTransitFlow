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

        public Dictionary<string, NetworkGraphEdge> DivergingEdges = new Dictionary<string, NetworkGraphEdge>();
        public Dictionary<string, NetworkGraphEdge> SingleSideEdges = new Dictionary<string, NetworkGraphEdge>();
        public Dictionary<string, NetworkGraphEdge> DoubleSideEdges = new Dictionary<string, NetworkGraphEdge> ();

        public void AddDivergingTrack(NetworkGraphEdge edge)
        {
            DivergingEdges.Add(edge.UUID,edge);
            if (edge.oppositeDirectionEdge != null && !DivergingEdges.Values.Contains(edge.oppositeDirectionEdge)) DivergingEdges.Add(edge.oppositeDirectionEdge.UUID,edge.oppositeDirectionEdge);
        }

        public void AddSingleSideTrack(NetworkGraphEdge edge)
        {
            SingleSideEdges.Add(edge.UUID,edge);
            if (edge.oppositeDirectionEdge != null && !SingleSideEdges.Values.Contains(edge.oppositeDirectionEdge)) SingleSideEdges.Add(edge.oppositeDirectionEdge.UUID, edge.oppositeDirectionEdge);
        }

        public void AddMainDoubleSideTrack(NetworkGraphEdge edge)
        {
            DoubleSideEdges.Add(edge.UUID,edge);
            if (edge.oppositeDirectionEdge != null && !DoubleSideEdges.Values.Contains(edge.oppositeDirectionEdge)) DoubleSideEdges.Add(edge.oppositeDirectionEdge.UUID, edge.oppositeDirectionEdge);

        }

        public override IEnumerable<NetworkGraphEdge> GetValidEdges(NetworkGraphEdge incomingEdge)
        {
            
            if (DivergingEdges.Values.Contains(incomingEdge))
            {
                return SingleSideEdges.Values.Where(edge => edge.Source == this);
            }
            if (SingleSideEdges.Values.Contains(incomingEdge))
            {
                return DoubleSideEdges.Values.Where(edge => edge.Source == this).Union(DivergingEdges.Values.Where(edge => edge.Source == this));
            }
            if (DoubleSideEdges.Values.Contains(incomingEdge))
            {
                return SingleSideEdges.Values.Where(edge => edge.Source == this);
            }
            return Enumerable.Empty<NetworkGraphEdge>();
        }
    }
}
