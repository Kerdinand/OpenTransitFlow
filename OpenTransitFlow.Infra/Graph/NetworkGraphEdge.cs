using System.Diagnostics;
using QuikGraph;

namespace OpenTransitFlow.Infra.Graph
{
    internal class NetworkGraphEdge : IEdge<NetworkGraphVertex>
    {
        public NetworkGraphEdge(NetworkGraphVertex source, NetworkGraphVertex target)
        {
            this.source = source;
            this.target = target;
        }

        public string UUID { get; set; }

        private NetworkGraphVertex source;
        private NetworkGraphVertex target;

        NetworkGraphVertex IEdge<NetworkGraphVertex>.Source => source;

        NetworkGraphVertex IEdge<NetworkGraphVertex>.Target => target;
    }
}
