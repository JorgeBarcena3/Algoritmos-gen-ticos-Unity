using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Genes de la esfera
/// </summary>
public class Gen
{
    /// <summary>
    /// Posicion X
    /// </summary>
    public float x;

    /// <summary>
    /// Posicion Y
    /// </summary>
    public float y;

    /// <summary>
    /// Tamaño del radio
    /// </summary>
    public float r;

    /// <summary>
    /// Posicion de la z
    /// </summary>
    public float z;

    /// <summary>
    /// Color de la esfera
    /// </summary>
    public Color255 c;

    /// <summary>
    /// Constructor del gen
    /// </summary>
    public Gen()
    {
        c = new Color255(0, 0, 0, 0);
    }

    /// <summary>
    /// Constructor de copia
    /// </summary>
    /// <param name="genes"></param>
    public Gen(Gen genes)
    {
        x = genes.x;
        y = genes.y;
        z = genes.z;
        r = genes.r;
        c = new Color255(genes.c);
    }

    /// <summary>
    /// Devuelve el componente de color
    /// </summary>
    /// <returns></returns>
    public Color255 getColor()
    {
        return c;
    }

    /// <summary>
    /// Pone un color
    /// </summary>
    /// <param name="color"></param>
    public void setColor(Color255 color)
    {
        c = new Color255(color);
    }

    /// <summary>
    /// Devuelve el gen i
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float get(int index)
    {
        if (index == 0)
        {
            return x;
        }
        else if (index == 1)
        {
            return y;
        }
        else if (index == 2)
        {
            return r;
        }
        else
        {
            return z;
        }
    }

    /// <summary>
    /// Setea el gen i
    /// </summary>
    /// <param name="i"></param>
    /// <param name="v"></param>
    public void set(int i, float v)
    {
        if (i == 0)
        {
            x = v;
        }
        else if (i == 1)
        {
            y = v;
        }
        else if (i == 3)
        {
            z = v; ;
        }
        else
        {
            r = v;
        }
    }

    /// <summary>
    /// Devuelve el componente de un color
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public float getColor(int index)
    {
        return c.get(index);
    }


    /// <summary>
    /// Setea el componete del color
    /// </summary>
    /// <param name="index"></param>
    /// <param name="v"></param>
    public void setColor(int index, float v)
    {
        c.set(index, v);
    }
}
