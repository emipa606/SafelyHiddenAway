using System;
using UnityEngine;
using Verse;

namespace TD.Utilities;

public static class TDWidgets
{
    public static void DrawGraph(Rect rect, string xString, string yString, string xFormat, string yFormat, float xMin,
        float xMax,
        Func<float, float> func, float? yMn = null, float? yMx = null, float roundUp = 1, int numDots = 50)
    {
        var graphRect = rect.RightPartPixels(rect.width - (Text.LineHeight * 2))
            .TopPartPixels(rect.height - (Text.LineHeight * 2));
        var gw = graphRect.width;
        var gh = graphRect.height;

        var xLabel = new Rect(graphRect.xMin, graphRect.yMax + Text.LineHeight, gw, Text.LineHeight);
        Widgets.Label(xLabel, xString);

        var yLabel = new Rect(graphRect.xMin - (Text.LineHeight * 2), graphRect.yMax, gh, Text.LineHeight);
        UI.RotateAroundPivot(-90, yLabel.position);
        Widgets.Label(yLabel, yString);
        UI.RotateAroundPivot(90, yLabel.position);

        if (xMin == xMax)
        {
            xMax++;
        }

        var xRange = xMax - xMin;
        var yMin = yMn ?? Math.Min(func(xMin), func(xMax));
        var yMax = yMx ?? Math.Max(func(xMin), func(xMax));
        var yRange = (float)Math.Ceiling((yMax - yMin) / roundUp) * roundUp;
        if (yRange == 0)
        {
            yRange = 0.02f;
            yMin -= 0.01f;
        }

        var before = Text.Anchor;

        var axisLabelX = new Rect(graphRect.xMin, graphRect.yMax, gw / 5, gw / 5);
        Text.Anchor = TextAnchor.UpperRight;
        for (var x = xMin + (xRange / 5); x <= xMax; x += xRange / 5)
        {
            Widgets.Label(axisLabelX, string.Format(xFormat, x));
            axisLabelX.x += gw / 5;
        }

        var axisLabelY = new Rect(graphRect.xMin - (gh / 5), graphRect.yMax - (gh / 5), gh / 5, gh / 5);
        Text.Anchor = TextAnchor.LowerRight;
        for (var y = yRange / 5; y <= yRange; y += yRange / 5)
        {
            UI.RotateAroundPivot(-90, axisLabelY.center);
            Widgets.Label(axisLabelY, string.Format(yFormat, y + yMin));
            UI.RotateAroundPivot(90, axisLabelY.center);
            axisLabelY.y -= gh / 5;
        }

        Widgets.DrawBoxSolid(graphRect, Color.black);
        Widgets.DrawLineHorizontal(graphRect.xMin, graphRect.yMax - (gh / 5), 5);
        Widgets.DrawLineHorizontal(graphRect.xMin, graphRect.yMax - (2 * gh / 5), 5);
        Widgets.DrawLineHorizontal(graphRect.xMin, graphRect.yMax - (3 * gh / 5), 5);
        Widgets.DrawLineHorizontal(graphRect.xMin, graphRect.yMax - (4 * gh / 5), 5);
        Widgets.DrawLineVertical(graphRect.xMin + (gw / 5), graphRect.yMax - 4, 5);
        Widgets.DrawLineVertical(graphRect.xMin + (2 * gw / 5), graphRect.yMax - 4, 5);
        Widgets.DrawLineVertical(graphRect.xMin + (3 * gw / 5), graphRect.yMax - 4, 5);
        Widgets.DrawLineVertical(graphRect.xMin + (4 * gw / 5), graphRect.yMax - 4, 5);
        var graphOrigin = new Vector2(graphRect.xMin, graphRect.yMax);
        var point = graphOrigin + new Vector2(0, -(func(xMin) - yMin) * gh / yRange);
        var dx = xMax / numDots;
        for (var x = dx; x <= xMax; x += dx)
        {
            var y = func(xMin + x) - yMin;
            var next = graphOrigin + new Vector2(x * gw / xMax, -y * gh / yRange);

            Widgets.DrawLine(point, next, Widgets.NormalOptionColor, 1.0f);

            point = next;
        }

        Widgets.DrawBox(graphRect);


        Text.Anchor = before;
    }
}