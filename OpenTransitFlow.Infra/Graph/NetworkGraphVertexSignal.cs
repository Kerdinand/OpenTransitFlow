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

        public NetworkGraphEdge Edge;

        /// <summary>
        /// Gets allowed speed to pass this signal. 
        /// </summary>
        public int AllowedVMax(NetworkGraphEdge edge)
        {
            if (checkIfTracksAreBlocked(edge)) return 0;
            return 100;
        }

        /// <summary>
        /// Check
        /// </summary>
        /// <returns></returns>
        private bool checkIfTracksAreBlocked(NetworkGraphEdge edge)
        {
            var tracksToCheck = new HashSet<NetworkGraphEdge>(outboundEdges.Values);
            if (edge.oppositeDirectionEdge != null) tracksToCheck.Remove(edge.oppositeDirectionEdge);
            while (tracksToCheck.Count > 0)
            {
                var track = tracksToCheck.First();
                if (track.IsBlocked) return true;
                
                // Only look until a new Signal is found. Tracks behind signal are out of bounds.
                if (track.Target is NetworkGraphVertexSignal)
                {
                    tracksToCheck.Remove(track);
                    continue;
                }
                foreach (var newTrackToCheck in track.Target.outboundEdges.Values)
                {
                    if (newTrackToCheck.oppositeDirectionEdge != null && tracksToCheck.Contains(newTrackToCheck.oppositeDirectionEdge))
                    {
                        if (newTrackToCheck.oppositeDirectionEdge.IsBlocked || newTrackToCheck.IsBlocked) return true;
                    } 
                    tracksToCheck.Add(newTrackToCheck);
                }
                tracksToCheck.Remove(track);
            }
            return false;
        }
    }
}
