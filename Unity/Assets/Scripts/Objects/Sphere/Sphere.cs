using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Pinta una esfera
/// </summary>
public class Sphere : GeneticElement
{
    /// <summary>
    /// Constructor por defecto
    /// </summary>
    public Sphere()
    {
        genes = new Gen();
    }

    /// <summary>
    /// Constructor con unos genes en concreto
    /// </summary>
    /// <param name="gen"></param>
    public Sphere(Gen gen)
    {
        genes = new Gen(gen);
    }


    /// <summary>
    /// Inicializa la esfera
    /// </summary>
    public override void Initialize()
    {

        genes = new Gen();


        //Creamos los puntos de posicion
        genes.x = UnityEngine.Random.Range(0, GameManager.Instance.imageReader.chunkOriginalTexture.width);
        genes.y = UnityEngine.Random.Range(0, GameManager.Instance.imageReader.chunkOriginalTexture.height);
        genes.r = pseudoRandom(UnityEngine.Random.Range(0, GameManager.Instance.imageReader.temporalTexture.width / 4), 2, (GameManager.Instance.imageReader.temporalTexture.width + GameManager.Instance.imageReader.temporalTexture.height) / 2);
        genes.z = UnityEngine.Random.Range(0, 1000);
        genes.c = new Color255(Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255), Random.Range(0, 255));


    }
    
    /// <summary>
    /// Pinta la esfera en una textura
    /// </summary>
    /// <param name="texture"></param>
    public override void paint(Texture2D texture)
    {
        GameManager.Instance.imageReader.drawSphere(new Vector2(genes.x, genes.y), genes.r, genes.c.getColorFormat(), texture);
    }

    /// <summary>
    /// Devuelve el valor, si esta entre el minimo y el maximo
    /// </summary>
    /// <param name="value"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public float pseudoRandom(float value, float min, float max)
    {
        if (value < min) { return min; }
        if (value > max) { return max; }
        return value;
    }
}
