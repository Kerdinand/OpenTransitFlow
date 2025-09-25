using OpenTransitFlow.Infra.GeoJson;
using System.Windows;
using System.Numerics;

namespace OpenTransitFlow.Infra.Assets
{
    public abstract class BaseTrack
    {
        public BezierCurve Shape = new BezierCurve();
        public int ID;
        public BaseTrack InboundTrack;
        public BaseTrack OutboundTrack;
        public float Length => Shape.CalculateLength();

        protected abstract BaseTrack CreateNew();

        public void SetStraightTrack(Vector2 start, Vector2 end)
        {
            Shape = new BezierCurve()
            {
                Start = start,
                End = end,
                Control1 = start + (end - start),
                Control2 = end + (start - end),
            };
        }

        public BaseTrack AddTrackAtStart(Vector2 x_3, Vector2 x_4, float f = 1)
        {
            var track = CreateNew();
            track.Shape.Start = this.Shape.Start;
            track.Shape.Control1 = this.Shape.Start + (this.Shape.Control1 - this.Shape.Start)*-1*f;
            track.Shape.Control2 = x_3;
            track.Shape.End = x_4;
            track.OutboundTrack = this;
            this.InboundTrack = track;
            return track;
        }

        public BaseTrack AddTrackAtEnd(Vector2 x_3, Vector2 x_4, float f = 1)
        {
            var track = CreateNew();
            track.Shape.Start = this.Shape.End;
            track.Shape.Control1 = this.Shape.End + (this.Shape.Control2-this.Shape.End)*-1*f;
            track.Shape.Control2 = x_3;
            track.Shape.End = x_4;
            track.InboundTrack = this;
            this.OutboundTrack = track;
            return track;
        }

        public BaseTrack Join(BaseTrack elementToJoin, JoinPairEnum joinConfig)
        {
            switch(joinConfig)
            {
                case JoinPairEnum.EndToStart:
                    return this.AddTrackAtEnd(elementToJoin.Shape.GetRelativeControlVectorStart(), elementToJoin.Shape.Start);
                case JoinPairEnum.StartToEnd:
                    return this.AddTrackAtStart(elementToJoin.Shape.GetRelativeControlVectorEnd(), elementToJoin.Shape.End);
                case JoinPairEnum.EndToEnd:
                    return this.AddTrackAtEnd(elementToJoin.Shape.GetRelativeControlVectorEnd(),elementToJoin.Shape.End);
                default:
                    return this.AddTrackAtStart(elementToJoin.Shape.GetRelativeControlVectorStart(), elementToJoin.Shape.Start);
            }
        }
    }
}
