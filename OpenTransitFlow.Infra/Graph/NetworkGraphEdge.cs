using System.Diagnostics;
using System.Numerics;
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
        internal double weight { get; set; } = 1 / new Random().NextDouble() * 100;

        /// <summary>
        /// Returns the direct length of the edge, disregardings its real shape
        /// ONLY USE FOR ESTIMATES AND NON CURVING ELEMENTS
        /// </summary>
        internal double directLength => Math.Abs(Vector2.Distance(source.position, target.position));

        /// <summary>
        /// Returns vmax of track. returns 160 if no vmaxFunction is defined.
        /// </summary>
        internal int vmax => vmaxFunction is null ? 160 : vmaxFunction.Invoke(this);

        /// <summary>
        /// Function to calculate max speed
        /// </summary>
        internal Func<NetworkGraphEdge, int>? vmaxFunction;

        NetworkGraphVertex IEdge<NetworkGraphVertex>.Source => source;

        NetworkGraphVertex IEdge<NetworkGraphVertex>.Target => target;

        public override string ToString()
        {
            return _uuid;
        }
    }
}
