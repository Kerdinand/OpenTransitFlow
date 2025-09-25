using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OpenTransitFlow.Infra.Assets
{
    public class BezierCurve
    {
        public Vector2 Start { get; set; } = Vector2.Zero;
        public Vector2 Control1 { get; set; } = Vector2.Zero;
        public Vector2 Control2 { get; set; } = Vector2.Zero;
        public Vector2 End { get; set; } = Vector2.Zero;

        public BezierCurve(Vector2 start, Vector2 control1, Vector2 control2, Vector2 end)
        {
            Start = start;
            Control1 = control1;
            Control2 = control2;
            End = end;
        }

        public BezierCurve()
        {
        }

        public Vector2 GetRelativeControlVectorStart()
        {
            return Start + (Control1 - Start)*-1;
        }

        public Vector2 GetRelativeControlVectorEnd()
        {
            return End + (Control2 - End)*-1;
        }

        // Evaluate the point on the curve at t in [0, 1]
        public Vector2 Evaluate(float t)
        {
            float u = 1 - t;
            return
                u * u * u * Start +
                3 * u * u * t * Control1 +
                3 * u * t * t * Control2 +
                t * t * t * End;
        }

        public float CalculateLength(int steps = 20)
        {
            float length = 0f;
            Vector2 prev = Evaluate(0f);

            for (int i = 1; i <= steps; i++)
            {
                float t = i / (float)steps;
                Vector2 point = Evaluate(t);
                length += Vector2.Distance(prev, point);
                prev = point;
            }

            return length;
        }
    }
}