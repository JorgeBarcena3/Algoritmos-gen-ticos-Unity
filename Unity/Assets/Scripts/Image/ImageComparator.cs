using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Comparador de las imagenes y la posicion de las lineas
/// </summary>
public class ImageComparator : Singelton<ImageComparator>
{

    /// <summary>
    /// Compara dos texturas y devuelve el error
    /// </summary>
    /// <param name="original"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    public float compareTextures(Texture2D original, Texture2D target)
    {

        float error = 0;

        for (int x = 0; x < original.width; x += 2)
        {
            for (int y = 0; y < original.height; y += 2)
            {
                Color rgb = original.GetPixel(x, y);

                Color rgb2 = target.GetPixel(x, y);

                error += Math.Abs(rgb.r - rgb2.r);
                error += Math.Abs(rgb.g - rgb2.g);
                error += Math.Abs(rgb.b - rgb2.b);

            }

        }

        return error;


    }
}