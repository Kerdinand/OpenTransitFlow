using System.Numerics;
using QuikGraph;
using System.Geometry;

namespace OpenTransitFlow.Infra.Graph
{
    public class NetworkGraphEdge : IEdge<BaseNetworkGraphVertex>
    {
        public NetworkGraphEdge(BaseNetworkGraphVertex source, BaseNetworkGraphVertex target, string uuid, bool isTwoWay = false) : base()
        {
            this.source = source;
            this.target = target;
            this._uuid = uuid;
            source.outboundEdges.Add(this.UUID, this);
            target.inboundEdges.Add(this.UUID, this);
            this.IsTwoWay = isTwoWay;
        }

        public NetworkGraphEdge(BaseNetworkGraphVertex source, BaseNetworkGraphVertex target, string uuid, IList<Vector2> points, bool isTwoWay = false) : base()
        {
            this.source = source;
            this.target = target;
            this._uuid = uuid;
            source.outboundEdges.Add(this.UUID, this);
            target.inboundEdges.Add(this.UUID, this);
            this.IsTwoWay = isTwoWay;
            this.shape = new Bezier(points);
        }

        private string _uuid;

        public string UUID => _uuid;

        private BaseNetworkGraphVertex source;
        public BaseNetworkGraphVertex Source => source;
        private BaseNetworkGraphVertex target;
        public BaseNetworkGraphVertex Target => target;

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
        /// If shape is defined length is set to the length of the actual shape.
        /// If shape is not defined, directLenght <see cref="directLength"/> is returned.
        /// </summary>
        internal double length => shape == null ? directLength : shape.Length;
        /// <summary>
        /// Returns vmax of track. returns 160 if no vmaxFunction is defined.
        /// </summary>
        internal int vmax => vmaxFunction is null ? 160 : vmaxFunction.Invoke(this);

        /// <summary>
        /// Function to calculate max speed
        /// </summary>
        internal Func<NetworkGraphEdge, int>? vmaxFunction;

        internal Bezier shape = null;

        internal volatile bool IsBlocked = false;

        BaseNetworkGraphVertex IEdge<BaseNetworkGraphVertex>.Source => source;

        BaseNetworkGraphVertex IEdge<BaseNetworkGraphVertex>.Target => target;

        public bool IsTwoWay = false;
        public NetworkGraphEdge oppositeDirectionEdge = null;

        public override string ToString()
        {
            return _uuid;
        }
    }
}
