﻿namespace XIVPainter.Element2D;

internal class ImageDrawing : IDrawing2D
{
    nint _textureId;
    Vector2 _pt1, _pt2, _uv1 = default, _uv2 = Vector2.One;
    uint _col;

    public ImageDrawing(nint textureId, Vector2 pt1, Vector2 pt2, 
        Vector2 uv1, Vector2 uv2, uint col = uint.MaxValue)
        :this(textureId, pt1, pt2, col)
    {
        _uv1 = uv1;
        _uv2 = uv2;
    }

    public ImageDrawing(nint textureId, Vector2 pt1, Vector2 pt2, uint col = uint.MaxValue)
    {
        _textureId = textureId;
        _pt1 = pt1;
        _pt2 = pt2;
        _col = col;
    }

    public void Draw()
    {
        ImGui.GetWindowDrawList().AddImage(_textureId, _pt1, _pt2, _uv1, _uv2, _col);
    }
}
