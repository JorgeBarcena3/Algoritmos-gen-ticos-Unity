using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Individuo 
/// </summary>
public class GeneticIndividual
{

    /// <summary>
    /// Lineas que se estan utilizando para reproducir la imagen
    /// </summary>
    public List<GeneticElement> geneticElements { get; private set; }

    /// <summary>
    /// Tamaño de su poblacion
    /// </summary>
    int populationSize;

    /// <summary>
    /// Puntuacion del individuo
    /// </summary>
    public float score;

    /// <summary>
    /// Parametros para la mutacion de la posicion y radio
    /// </summary>
    float maxDelta;

    /// <summary>
    /// Parametros para la mutacion de la posicion y radio
    /// </summary>
    float halfDelta;

    /// <summary>
    /// Parametros para la mutacion de los colores
    /// </summary>
    float maxColorDelta;

    /// <summary>
    /// Parametros para la mutacion de los colores
    /// </summary>
    float halfMaxColorDelta;

    /// <summary>
    /// Constructor por defecto
    /// </summary>
    /// <param name="_populationSize"></param>
    public GeneticIndividual(int _populationSize)
    {

        maxDelta = ((GameManager.Instance.imageReader.chunkOriginalTexture.width + GameManager.Instance.imageReader.chunkOriginalTexture.height) / 2) / 2;
        halfDelta = maxDelta / 2 + 2;
        maxColorDelta = 100;
        halfMaxColorDelta = maxColorDelta / 2;

        populationSize = _populationSize;

        geneticElements = new List<GeneticElement>(populationSize);

        for (int i = 0; i < geneticElements.Capacity; ++i)
        {
            geneticElements.Add(new Sphere());
            geneticElements[i].Initialize();
        }

        geneticElements = geneticElements.OrderBy(m => m.genes.z).ToList();

    }

    /// <summary>
    /// Constructor con una generacion base
    /// </summary>
    /// <param name="newGeneration"></param>
    public GeneticIndividual(List<GeneticElement> newGeneration)
    {

        maxDelta = ((GameManager.Instance.imageReader.chunkOriginalTexture.width + GameManager.Instance.imageReader.chunkOriginalTexture.height) / 2) / 2;
        halfDelta = maxDelta / 2 + 2;
        maxColorDelta = 100;
        halfMaxColorDelta = maxColorDelta / 2;

        geneticElements = new List<GeneticElement> (newGeneration);
        populationSize = newGeneration.Count;

        geneticElements = geneticElements.OrderBy(m => m.genes.z).ToList();

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

    /// <summary>
    /// Muta a todo el individuo
    /// </summary>
    /// <param name="mutationRatio"></param>
    /// <returns></returns>
    public GeneticIndividual mutate(float mutationRatio)
    {
        List<GeneticElement> newGeneration = new List<GeneticElement>();

        geneticElements.ForEach(m => newGeneration.Add(new Sphere(mutateElement(m, mutationRatio).genes)));

        return new GeneticIndividual(newGeneration);

    }

    /// <summary>
    /// Muta los genes
    /// </summary>
    public GeneticElement mutateElement(GeneticElement element, float mutationRatio)
    {
        GeneticElement newElement = new Sphere(new Gen(element.genes));

        if (UnityEngine.Random.Range(.0f, 1) <= mutationRatio)
        {
            int genMutation = UnityEngine.Random.Range(0, 7);

            switch (genMutation)
            {

                case 0:

                    newElement.genes.x = pseudoRandom(element.genes.x + UnityEngine.Random.Range(0, maxDelta) - halfDelta, 0, GameManager.Instance.imageReader.temporalTexture.width - element.genes.r);
                    break;

                case 1:

                    newElement.genes.y = pseudoRandom(element.genes.y + UnityEngine.Random.Range(0, maxDelta) - halfDelta, 0, GameManager.Instance.imageReader.temporalTexture.height - element.genes.r);
                    break;

                case 2:

                    newElement.genes.r = pseudoRandom(element.genes.r + UnityEngine.Random.Range(0, maxDelta) - halfDelta, 5, GameManager.Instance.imageReader.temporalTexture.width);
                    break;

                case 3:

                    newElement.genes.z = UnityEngine.Random.Range(0, 1000);
                    break;

                case 4:

                    mutateColor(newElement, element.genes.c, 0);
                    break;

                case 5:

                    mutateColor(newElement, element.genes.c, 1);
                    break;

                case 6:

                    mutateColor(newElement, element.genes.c, 2);
                    break;

            }

        }

        return newElement;
    }

    /// <summary>
    /// Pinta todos sus elementos en una textura
    /// </summary>
    /// <param name="finalTexture"></param>
    public void paint(Texture2D finalTexture)
    {
        geneticElements = geneticElements.OrderByDescending(m => m.genes.z).ToList();

        foreach (var item in geneticElements)
        {
            item.paint(finalTexture);
        }
    }

    /// <summary>
    /// Muta un color
    /// </summary>
    /// <param name="newElement"></param>
    /// <param name="c"></param>
    /// <param name="i"></param>
    private void mutateColor(GeneticElement newElement, Color255 c, int i)
    {
        newElement.genes.c.set(i, pseudoRandom(c.get(i) + UnityEngine.Random.Range(0, maxColorDelta) - halfMaxColorDelta, 0, 255));
    }

   
}
