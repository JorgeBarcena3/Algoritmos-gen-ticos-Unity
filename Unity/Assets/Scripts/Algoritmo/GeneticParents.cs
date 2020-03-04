using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Padre de los individuos
/// </summary>
public class GeneticParents
{

    /// <summary>
    /// Lineas que se estan utilizando para reproducir la imagen
    /// </summary>
    public List<GeneticIndividual> individuals { get; private set; }

    /// <summary>
    /// Poblacion de los individuos
    /// </summary>
    int populationSize;

    /// <summary>
    /// Puntuacion de todos los individuos
    /// </summary>
    float score;

    /// <summary>
    /// Constructor por defecto
    /// </summary>
    /// <param name="_populationSize"></param>
    /// <param name="_adn"></param>
    public GeneticParents(int _populationSize, int _adn)
    {

        populationSize = _populationSize;

        individuals = new List<GeneticIndividual>(populationSize);

        for (int i = 0; i < individuals.Capacity; ++i)
        {
            individuals.Add(new GeneticIndividual(_adn));

        }

    }

    /// <summary>
    /// Constructor que acepta otra lista
    /// </summary>
    /// <param name="newGeneration"></param>
    public GeneticParents(List<GeneticIndividual> newGeneration)
    {

        individuals = new List<GeneticIndividual>(newGeneration);
        populationSize = newGeneration.Count;

    }

    /// <summary>
    /// Ordena los individuos por la score
    /// </summary>
    public void orderByScore()
    {

        individuals = individuals.OrderBy(m => m.score).ToList();

    }

    /// <summary>
    /// Evalua el score
    /// </summary>
    /// <param name="originalTexture"></param>
    public void evaluateScore(Texture2D originalTexture)
    {
        individuals.ForEach(m =>
            {
                GameManager.Instance.imageReader.fillTexture(Color.black, GameManager.Instance.imageReader.temporalTexture);
                m.paint(GameManager.Instance.imageReader.temporalTexture);
                m.score = ImageComparator.Instance.compareTextures(GameManager.Instance.imageReader.temporalTexture, originalTexture);
            });

        orderByScore();
    }
    
    /// <summary>
    /// Se muta el padre partiendo de dos individuos
    /// </summary>
    /// <param name="one"></param>
    /// <param name="two"></param>
    /// <param name="mutationRatio"></param>
    /// <returns></returns>
    public GeneticIndividual Mutate(GeneticIndividual one, GeneticIndividual two, float mutationRatio)
    {

        var genesLength = one.geneticElements.Count;

        List<GeneticElement> genes = new List<GeneticElement>();

        for (int i = 0; i < genesLength; i++)
        {
            if (UnityEngine.Random.Range(0.0f, 1.0f) > .5)
            {
                genes.Add(one.mutate(mutationRatio).geneticElements[i]);

            }
            else
            {
                genes.Add((two.mutate(mutationRatio)).geneticElements[i]);
            }
        }

        return new GeneticIndividual(new List<GeneticElement>(genes));


    }

    /// <summary>
    /// Se selecciona a los padres
    /// </summary>
    /// <returns></returns>
    public GeneticIndividual select()
    {
        float n = populationSize * GeneticController.Instance.parentRatio;
        float i = 0;

        foreach (var m in individuals)
        {
            if (i > 0)
            {
                if (UnityEngine.Random.Range(0.0f, 1) <= (populationSize - i) / (populationSize + n))
                {
                    return new GeneticIndividual(m.geneticElements);
                }
            }
            i++;
        }

        return new GeneticIndividual(individuals.First().geneticElements);
    }

    /// <summary>
    /// Se pinta todo el padre en una textura
    /// </summary>
    /// <param name="finalTexture"></param>
    /// <param name="item"></param>
    public void paint(Texture2D finalTexture, int item)
    {
        individuals[item].paint(finalTexture);
    }

}
