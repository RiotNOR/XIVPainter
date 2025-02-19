﻿using System.Collections.Generic;

namespace XIVPainter.Element3D;

public class Drawing3DAnnulus : Drawing3DPolyline
{
    public Vector3 Center { get; set; }
    public float Radius1 { get; set; }
    public float Radius2 { get; set; }
    public Vector2[] ArcStartSpan { get; set; }

    public Drawing3DAnnulus(Vector3 center, float radius1, float radius2, uint color,
        float thickness, params Vector2[] arcStartSpan)
        : base(null, color, thickness)
    {
        Center = center;
        Radius1 = radius1;
        Radius2 = radius2;
        ArcStartSpan = arcStartSpan;
        if (arcStartSpan == null || arcStartSpan.Length == 0)
        {
            ArcStartSpan = new Vector2[] { new Vector2(0, MathF.Tau) };
        }
    }

    public override void UpdateOnFrame(XIVPainter painter)
    {
        base.UpdateOnFrame(painter);

        if (Radius1 == 0 || Radius2 == 0)
        {
            BorderPoints = FillPoints = Array.Empty<Vector3[]>();
            return;
        }

        IEnumerable<IEnumerable<Vector3>> boarder = Array.Empty<IEnumerable<Vector3>>(),
            fill = Array.Empty<IEnumerable<Vector3>>();
        foreach (var pair in ArcStartSpan)
        {
            var circleSegment = (int)(MathF.Tau * MathF.Max(Radius1, Radius2) / painter.SampleLength);
            circleSegment = Math.Min(circleSegment, 72);

            var sect1 = painter.SectorPlots(Center, Radius1, pair.X, pair.Y, circleSegment);
            var sect2 = painter.SectorPlots(Center, Radius2, pair.X, pair.Y, circleSegment);
            boarder = boarder.Append(sect1.Reverse().ToArray());
            boarder = boarder.Append(sect2);
            fill = fill.Union(GetAnnulusFill(sect1, sect2));
        }
        BorderPoints = boarder;
        FillPoints = fill;
    }

    //SLOW!
    private static IEnumerable<IEnumerable<Vector3>> GetAnnulusFill(Vector3[] ptsA, Vector3[] ptsB)
    {
        if (ptsA.Length != ptsB.Length) return Array.Empty<Vector3[]>();
        var length = ptsA.Length;

        var result = new Vector3[ptsA.Length][];

        for (int i = 0; i < length; i++)
        {
            var p1 = ptsA[i];
            var p2 = ptsB[i];
            var p3 = ptsB[(i + 1) % length];
            var p4 = ptsA[(i + 1) % length];

            result[i] = new Vector3[] { p1, p2, p3, p4 };
        }
        return result;
    }
}
