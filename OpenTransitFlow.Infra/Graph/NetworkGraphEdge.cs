using QuikGraph;

namespace OpenTransitFlow.Infra.Graph
{
    internal class NetworkGraphEdge : IEdge<NetworkGraphVertex>
    {
        public NetworkGraphEdge()
        {
        }

        NetworkGraphVertex IEdge<NetworkGraphVertex>.Source => throw new NotImplementedException();

        NetworkGraphVertex IEdge<NetworkGraphVertex>.Target => throw new NotImplementedException();
    }
}
