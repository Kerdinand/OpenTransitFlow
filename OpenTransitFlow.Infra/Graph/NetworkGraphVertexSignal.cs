using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Metadata.Ecma335;
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

        public NetworkGraphEdge SignalEdge;

        /// <summary>
        /// Checks if the edge the node is approached on, is from the direction the signal is pointing.
        /// </summary>
        public bool SignalIsInDirectionOfEdge(NetworkGraphEdge edge)
        {
            return edge == SignalEdge;
        } 

        /// <summary>
        /// Gets allowed speed to pass this signal. 
        /// </summary>
        public int AllowedVMax(NetworkGraphEdge edge)
        {
            if (CheckIfTracksAreBlocked(edge)) return 0;
            return 100;
        }

        /// <summary>
        /// Explores all tracks that are within signals of the current edge.
        /// </summary>
        private bool CheckIfTracksAreBlocked(NetworkGraphEdge edge)
        {
            var tracksToCheck = new HashSet<NetworkGraphEdge>(edge.Target.GetValidEdges(edge));
            
            while (tracksToCheck.Count > 0)
            {
                var track = tracksToCheck.First();
                if (track.IsBlocked || track.oppositeDirectionEdge != null && track.oppositeDirectionEdge.IsBlocked) return true;

                if (track.Target is NetworkGraphVertexSignal && ((NetworkGraphVertexSignal)track.Target).SignalIsInDirectionOfEdge(track))
                {
                    tracksToCheck.Remove(track);
                    continue;
                }
                foreach (var newTrack in track.Target.GetValidEdges(track))
                {
                    if (newTrack.oppositeDirectionEdge != null && newTrack.oppositeDirectionEdge.IsBlocked) return true; 
                    tracksToCheck.Add(newTrack);
                }
                tracksToCheck.Remove(track);
            }
            return false;
        }
    }
}
