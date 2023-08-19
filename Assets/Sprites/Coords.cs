using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coords
{
    float x, y;
    public Coords(float x, float y)
    {
        this.x = x;
        this.y = y;
    }

    public override string ToString()
    {
        return this.x + " " + this.y;
    }


    static public void DrawLine(Coords start, Coords end, float width, Color c)
    {
        GameObject line = new GameObject("Line:(X:" + start.x + "," + end.x + ";Y:" + start.y + "," + end.y + ")");
        LineRenderer lRenderer = line.AddComponent<LineRenderer>();
        lRenderer.material = new Material(Shader.Find("Unlit/Color"));
        lRenderer.material.color = c;

        lRenderer.SetPosition(0, new Vector3(start.x, start.y, 0.0f));
        lRenderer.SetPosition(1, new Vector3(end.x, end.y, 0.0f));
        lRenderer.startWidth = width;
        lRenderer.endWidth = width;
    }


    static public void DrawLines(Coords[] points, float width, Color c)
    {

        for (int i = 0; i < points.Length - 1; i++)
        {
            DrawLine(new Coords(points[i].x, points[i].y), new Coords(points[i + 1].x, points[i + 1].y), 1, Color.yellow); //line
                                                                                                                           // DrawLine(new Coords(points[i].x -1 ,points[i].y -1),new Coords(points[i].x+1,points[i].y+1), 2, Color.blue); //dot
        }


    }
}