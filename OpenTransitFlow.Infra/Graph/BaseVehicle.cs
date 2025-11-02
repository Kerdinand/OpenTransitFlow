using QuikGraph;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTransitFlow.Infra.Graph
{
    public class BaseVehicle : IVehicle
    {
        public BaseVehicle(string name, string description, int vehicleMaxSpeed, IEnumerable<NetworkGraphEdge> currentPath, NetworkGraphEdge currentEdge)
        {
            Name = name;
            Description = description;
            this.vehicleMaxSpeed = vehicleMaxSpeed;
            CurrentPath = currentPath;
            this.currentEdge = currentEdge;
            this.currentEdge.IsBlocked = true;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public int vehicleMaxSpeed { get; set; }

        public IEnumerable<NetworkGraphEdge> CurrentPath { get; set; } = new List<NetworkGraphEdge> ();

        public NetworkGraphEdge currentEdge { get; set; } = null;

        /// <summary>
        /// Function to move vehicle to next possible edge
        /// </summary>
        public virtual VehicleMoveStatus MoveVehicle()
        {
            var currentNextNode = currentEdge.Target;
            var currentIndex = CurrentPath.ToList().IndexOf(currentEdge);
            if (currentIndex == CurrentPath.ToList().Count - 1) return VehicleMoveStatus.REACHED_DESTINATION;
            var nextEdge = CurrentPath.ElementAt(currentIndex + 1);
            if (currentEdge.Target is NetworkGraphVertexSignal && ((NetworkGraphVertexSignal)currentEdge.Target).AllowedVMax(currentEdge, CurrentPath.ToList()) != 0)
            {
                currentEdge.IsBlocked = false;
                currentEdge = nextEdge;
                currentEdge.IsBlocked = true;
                return VehicleMoveStatus.MOVING;
            }
            if (currentEdge.Target is NetworkGraphVertexSignal && ((NetworkGraphVertexSignal)currentEdge.Target).AllowedVMax(currentEdge, CurrentPath.ToList()) == 0)
            {
                return VehicleMoveStatus.STOPPED_AT_SIGNAL;
            }
            if (currentEdge.IsBlocked && !nextEdge.IsBlocked)
            {
                currentEdge.IsBlocked = false;
                currentEdge = nextEdge;
                currentEdge.IsBlocked = true;
                return VehicleMoveStatus.MOVING;
            }
            return VehicleMoveStatus.UNKNOWN;
        }

        public IEnumerable<NetworkGraphEdge> GetAllEdgesWithinSignalBounds()
        {
            var result = new HashSet<NetworkGraphEdge>();
            var forwardEdges = new HashSet<NetworkGraphEdge>(new[] {currentEdge});

            /*
             * this.currentEdge.Target.GetValidEdges(currentEdge).ToHashSet() ?? 
            if (currentEdge.Target is NetworkGraphVertexSignal && ((NetworkGraphVertexSignal)currentEdge.Target).SignalIsInDirectionOfEdge(currentEdge))
            {
                forwardEdges.Clear();
            }
            */
            while (forwardEdges.Count > 0)
            {
                var entry = forwardEdges.First();
                result.Add(entry);
                if (entry.Target is NetworkGraphVertexSignal && ((NetworkGraphVertexSignal)entry.Target).SignalIsInDirectionOfEdge(entry))
                {
                    forwardEdges.Remove(entry);
                    continue;
                }
                
                forwardEdges = forwardEdges.Union(entry.Target.GetValidEdges(entry)).ToHashSet();
                forwardEdges.Remove(entry);
            }

         

            return result;
        }

        /// <summary>
        /// Removes Vehicle from current Edge
        /// </summary>
        public virtual void Remove()
        {
            currentEdge.IsBlocked = false;
            currentEdge = null;
        }
    }
}
