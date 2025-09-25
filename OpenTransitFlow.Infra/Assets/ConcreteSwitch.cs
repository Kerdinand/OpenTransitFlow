using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTransitFlow.Infra.Assets
{
    public class ConcreteSwitch : BaseSwitch
    {
        public ConcreteSwitch() { }
        protected override BaseSwitch CreateNew() => new ConcreteSwitch();
    }
}
