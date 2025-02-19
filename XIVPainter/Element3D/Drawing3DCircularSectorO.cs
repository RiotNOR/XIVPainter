﻿using Dalamud.Game.ClientState.Objects.Types;
using Dalamud.Logging;
using XIVPainter.Enum;

namespace XIVPainter.Element3D;

public class Drawing3DCircularSectorO : Drawing3DCircularSector
{
    public RadiusInclude Including { get; set; }
    public float RadiusTarget { get; set; }
    public GameObject Target { get; set; }
    public Vector2[] ArcStarSpanTarget { get; set; }

    public Drawing3DCircularSectorO(GameObject target, float radius, uint color, float thickness, RadiusInclude include = RadiusInclude.IncludeBoth, params Vector2[] arcStartSpan) 
        : base(target?.Position ?? default, include.GetRadius(target, radius), color, thickness)
    {
        RadiusTarget = radius;
        Target = target;
        Including = include;
        ArcStarSpanTarget = arcStartSpan;
        if (arcStartSpan == null || arcStartSpan.Length == 0)
        {
            ArcStarSpanTarget = new Vector2[] { new Vector2(0, MathF.Tau) };
        }
    }

    public override void UpdateOnFrame(XIVPainter painter)
    {
        Center = Target?.Position ?? default;
        Radius = Including.GetRadius(Target, RadiusTarget);

        ArcStartSpan = ArcStarSpanTarget.Select(pt => new Vector2(pt.X + Target?.Rotation ?? 0, pt.Y)).ToArray();

        base.UpdateOnFrame(painter);
    }
}
