using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTransitFlow.Infra.Graph
{
    internal interface IVehicle
    {
        /// <summary>
        /// Short Name of Vehicle
        /// </summary>
        string Name { get; }
        /// <summary>
        /// Description of Vehicle
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Max Speed of Vehicle
        /// </summary>
        int vehicleMaxSpeed { get; }
        /// <summary>
        /// The path the vehicle is currently taking
        /// </summary>
        IEnumerable<NetworkGraphEdge> CurrentPath { get; }
        /// <summary>
        /// Current Edge the vehicle is on.
        /// </summary>
        NetworkGraphEdge currentEdge { get; set; }
        
    }
}
