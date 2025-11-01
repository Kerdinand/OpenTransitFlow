using System.Diagnostics;
using QuikGraph;

namespace OpenTransitFlow.Infra.Graph
{
    public class NetworkGraphEdge : IEdge<NetworkGraphVertex>
    {
        public NetworkGraphEdge(NetworkGraphVertex source, NetworkGraphVertex target, string uuid) : base()
        {
            this.source = source;
            this.target = target;
            this._uuid = uuid;
        }


        private string _uuid;

        public string UUID => _uuid;

        private NetworkGraphVertex source;
        private NetworkGraphVertex target;

/// <summary>
/// Dummy value for now. Should represent time/priority to take such link.
/// </summary>
        public double weight { get; set; } = 1 / new Random().NextDouble() * 100;

        NetworkGraphVertex IEdge<NetworkGraphVertex>.Source => source;

        NetworkGraphVertex IEdge<NetworkGraphVertex>.Target => target;

        public override string ToString()
        {
            return _uuid;
        }
    }
}
