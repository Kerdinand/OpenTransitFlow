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
    }
}
