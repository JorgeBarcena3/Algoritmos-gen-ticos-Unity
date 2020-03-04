using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clase que convierte un color basado en componentes del 0 al 155 en un color de unity
/// </summary>
public class Color255
{
    /// <summary>
    /// Componente r
    /// </summary>
    public float r;

    /// <summary>
    /// Componente g
    /// </summary>
    public float g;

    /// <summary>
    /// Componente b
    /// </summary>
    public float b;

    /// <summary>
    /// Componente a
    /// </summary>
    public float a;

    /// <summary>
    /// Constructor con los parametros del color
    /// </summary>
    /// <param name="_r"></param>
    /// <param name="_g"></param>
    /// <param name="_b"></param>
    /// <param name="_a"></param>
    public Color255(float _r,
              float _g,
              float _b,
              float _a)
    {

        r = _r;
        g = _g;
        b = _b;
        a = _a;

    }

    /// <summary>
    /// Constructor de copia
    /// </summary>
    /// <param name="c"></param>
    public Color255(Color255 c)
    {
        r = c.r;
        g = c.g;
        b = c.b;
        a = c.a;
    }

    /// <summary>
    /// Transforma el color a un Color de unity
    /// </summary>
    /// <returns></returns>
    public Color getColorFormat()
    {
        return new Color(r / 255, g / 255, b / 255, a / 255);
    }

    /// <summary>
    /// Hace un set de los valores
    /// </summary>
    /// <param name="i"></param>
    /// <param name="v"></param>
    public void set(int i, float v)
    {
        if (i == 0)
            r = v;
        else if (i == 1)
            g = v;
        else if (i == 2)
            b = v;
        else
            a = v;
    }

    /// <summary>
    /// Devuelve el componente de un color
    /// </summary>
    /// <param name="i"></param>
    /// <returns></returns>
    public float get(int i)
    {
        if (i == 0)
            return r;
        if (i == 1)
            return g;
        if (i == 2)
            return b;

        return a;
    }
}
