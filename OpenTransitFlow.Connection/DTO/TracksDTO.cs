using System.Numerics;

namespace OpenTransitFlow.Connection.DTO
{
    public class BaseTrackJson
    {
        public CubicBezierCurve BezierCurve { get; set; }
        public string InboundDiverging { get; set; }
        public string InboundTrack { get; set; }
        public int LerpSteps { get; set; } = 100;
        public string OutboundDiverging { get; set; }
        public string OutboundTrack { get; set; }
        public string Type { get; set; }
        public string Uuid { get; set; }
    }

    public class CubicBezierCurve
    {
        public Vector3 V0 { get; set; }
        public Vector3 V1 { get; set; }
        public Vector3 V2 { get; set; }
        public Vector3 V3 { get; set; }
    }
}
