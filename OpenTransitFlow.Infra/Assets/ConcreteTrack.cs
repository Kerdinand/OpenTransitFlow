using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OpenTransitFlow.Infra.Assets
{
    public class ConcreteTrack : BaseTrack
    {
        public ConcreteTrack() { }
        protected override BaseTrack CreateNew() => new ConcreteTrack();
    }
}
