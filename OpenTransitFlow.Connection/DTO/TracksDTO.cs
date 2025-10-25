using System.Numerics;

namespace OpenTransitFlow.Connection
{
    public class BaseTrackJson
    {
        public CubicBezierCurve bezierCurve { get; set; }
        public string inboundDiverging { get; set; }
        public string inboundTrack { get; set; }
        public int lerpSteps { get; set; } = 100;
        public string outboundDiverging { get; set; }
        public string outboundTrack { get; set; }
        public string type { get; set; }
        public string uuid { get; set; }
    }

    public class CubicBezierCurve
    {
        public double[] v0 { get; set; }
        public double[] v1 { get; set; }
        public double[] v2 { get; set; }
        public double[] v3 { get; set; }
    }
}
