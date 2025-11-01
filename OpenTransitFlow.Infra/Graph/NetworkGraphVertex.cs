using System.Numerics;

namespace OpenTransitFlow.Infra.Graph
{
    public class NetworkGraphVertex : BaseNetworkGraphVertex
    {
        public NetworkGraphVertex(string uuid, Vector2 pos)
    : base(uuid, pos)
        {
        }

        public NetworkGraphVertex(string uuid, double[] pos)
            : base(uuid, new Vector2((float)pos[0], (float)pos[1]))
        {
        }


    }
}
