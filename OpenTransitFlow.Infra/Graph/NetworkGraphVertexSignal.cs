using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OpenTransitFlow.Infra.Graph
{
    public class NetworkGraphVertexSignal : BaseNetworkGraphVertex
    {
        public NetworkGraphVertexSignal(string uuid, Vector2 pos)
    : base(uuid, pos)
        {
        }

        public NetworkGraphVertexSignal(string uuid, double[] pos)
            : base(uuid, new Vector2((float)pos[0], (float)pos[1]))
        {
        }

        /// <summary>
        /// Gets allowed speed to pass this signal. 
        /// </summary>
        public int AllowedVMax()
        {
            return 100;
        }
    }
}
