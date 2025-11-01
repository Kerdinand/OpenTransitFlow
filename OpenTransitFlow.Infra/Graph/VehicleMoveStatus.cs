using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTransitFlow.Infra.Graph
{
    public enum VehicleMoveStatus
    {
        MOVING=0, STOPPED=1, REACHED_DESTINATION=2, STOPPED_AT_SIGNAL=3, UNKNOWN=-1
    }
}
