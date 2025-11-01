using OpenTransitFlow.Infra.Graph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace OpenTransitFlow.Infra.Tests.Graph
{
    internal static class GraphHelper
    {
        public static void AddTestTrack(this GraphFactory graphFactory)
        {
            graphFactory.AddNode("stationA", [0, 0]);
            graphFactory.AddNode("stationB-I", [10, 0]);
            graphFactory.AddNode("stationB-II", [10, 0]);
            graphFactory.AddNode("stationC", [20, 0]);

            graphFactory.AddNode("switch1", [0, 5]);
            graphFactory.AddNode("switch2", [15,0]);

            graphFactory.AddEdge("1", "stationA", "switch1", true);
            graphFactory.AddEdge("2", "switch1", "stationB-I", true);
            graphFactory.AddEdge("3", "stationB-II", "switch1");
            graphFactory.AddEdge("4", "stationB-I", "switch2", true);
            graphFactory.AddEdge("5", "switch2", "stationB-II");
            graphFactory.AddEdge("6", "switch2", "stationC", true);
        }
    }
}
