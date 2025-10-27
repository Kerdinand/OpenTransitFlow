using System.Diagnostics;
using QuikGraph;

namespace OpenTransitFlow.Connection.Graph
{
    public class NetworkGraphEdge : IEdge<NetworkGraphVertex>
    {
        public NetworkGraphEdge(NetworkGraphVertex source, NetworkGraphVertex target) : base()
        {
            this.source = source;
            this.target = target;
        }

        private NetworkGraphVertex source;
        private NetworkGraphVertex target;

        NetworkGraphVertex IEdge<NetworkGraphVertex>.Source => source;

        NetworkGraphVertex IEdge<NetworkGraphVertex>.Target => target;
    }
}
