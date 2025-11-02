using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OpenTransitFlow.Infra.Graph
{
    public abstract class BaseNetworkGraphVertex : INetworkGraphVertex
    {
        protected BaseNetworkGraphVertex(string uuid, Vector2 pos)
        {
            this.uuid = uuid;
            this.position = pos;
        }

        protected BaseNetworkGraphVertex(string uuid, int[] pos)
        {
            this.uuid = uuid;
            this.position = new Vector2(pos[0], pos[1]);
        }

        public Vector2 position { get; set; }
        public string uuid { get; set; }

        public Dictionary<string, NetworkGraphEdge> inboundEdges = new Dictionary<string, NetworkGraphEdge>();
        public Dictionary<string, NetworkGraphEdge> outboundEdges = new Dictionary<string, NetworkGraphEdge>();

        /// <summary>
        /// Checks the side of which the IVehicle is approaching, and returns the edges in direction of travel.
        /// </summary>
        /// <param name="incomingEdge"></param>
        /// <returns></returns>
        public virtual IEnumerable<NetworkGraphEdge> GetValidEdges(NetworkGraphEdge incomingEdge)
        {
            List<NetworkGraphEdge> result = new List<NetworkGraphEdge>();
            if (inboundEdges.Values.Contains(incomingEdge)) result = new List<NetworkGraphEdge>(outboundEdges.Values);
            //if (outboundEdges.Values.Contains(incomingEdge)) result = new List<NetworkGraphEdge>(inboundEdges.Values);
            if (incomingEdge.oppositeDirectionEdge != null) result.Remove(incomingEdge.oppositeDirectionEdge);
            
            return result;
        }
    }
}
