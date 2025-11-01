using System.Numerics;

namespace OpenTransitFlow.Infra.Graph
{
    public class NetworkGraphVertex
    {
        public NetworkGraphVertex(string uuid, Vector2 pos)
        {
            this.uuid = uuid;
            position = pos;
        }

        public NetworkGraphVertex(string uuid, int[] pos)
        {
            this.uuid = uuid;
            this.position = new Vector2(pos[0], pos[1]);
        }

        public Vector2 position { get; set; }
        public string uuid { get; set; }
    }
}
