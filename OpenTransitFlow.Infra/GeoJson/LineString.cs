using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenTransitFlow.Infra.GeoJson
{
    public class LineString
    {
        List<(float x, float y)> line = new List<(float x, float y)>();

        public void AddPoint(float x, float y)
        {
            line.Add((x, y));
        }

        public List<(float x, float y)> GetPoints() { return line; }
    }
}
