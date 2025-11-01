using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OpenTransitFlow.Infra.Graph
{
    public interface INetworkGraphVertex
    {
        public Vector2 position { get; set; }
        public string uuid { get; set; }
    }
}
