using OpenTransitFlow.Infra.Graph;
using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTransitFlow.Infra.Vehicles
{
    public class Train : BaseVehicle
    {
        public Train(string name, string description, int vehicleMaxSpeed, IEnumerable<NetworkGraphEdge> currentPath, NetworkGraphEdge currentEdge) : base(name, description, vehicleMaxSpeed, currentPath, currentEdge)
        {
        }
    }
}
