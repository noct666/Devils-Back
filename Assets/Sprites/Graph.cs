using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graph : MonoBehaviour
{

    Coords[] points = new Coords[10];

    int xLenght = 16;
    int yLenght = 9;

    public int step;
    public int dimensionalSize;

    void Start()
    {
        Coords.DrawLine(new Coords(-xLenght * dimensionalSize, 0), new Coords(xLenght * dimensionalSize, 0), 0.2f, Color.green);
        Coords.DrawLine(new Coords(0, -yLenght * dimensionalSize), new Coords(0, yLenght * dimensionalSize), 0.2f, Color.red);


        int xOfset = (int)(xLenght / step);
        int yOfset = (int)(yLenght / step);


        for (int i = -(xOfset * step) * dimensionalSize; i <= (xOfset * step) * dimensionalSize; i += step)
        {
            Coords.DrawLine(new Coords(i, -yLenght * dimensionalSize), new Coords(i, yLenght * dimensionalSize), 0.1f, Color.white);
        }
        for (int j = -(yOfset * step) * dimensionalSize; j <= (yOfset * step) * dimensionalSize; j += step)
        {
            Coords.DrawLine(new Coords(-xLenght * dimensionalSize, j), new Coords(xLenght * dimensionalSize, j), 0.1f, Color.white);
        }

    }


}