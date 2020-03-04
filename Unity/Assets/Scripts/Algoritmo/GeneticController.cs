using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Contorlador del algoritmo genetico
/// </summary>
public class GeneticController : Singelton<GeneticController>
{

    [Header("Tamaño de las regiones")]
    /// <summary>
    /// Tamaño de los chunks
    /// </summary>
    [Range(1, 100)]
    public int pixelChunk = 50;

    /// <summary>
    /// Tiempo en segundos por chunk
    /// </summary>
    public float chunkTimeInSeconds = 10;

    [Header("Configuracion Inicial")]
    /// <summary>
    /// Ratio de mutaciones
    /// </summary>
    [Range(0, 1)]
    public float mutationRatio = 0.02f;

    /// <summary>
    /// Ratio de mutaciones
    /// </summary>
    [Range(0, 1)]
    public float parentRatio = 0.02f;

    /// <summary>
    /// Dos modos de ejecutar el algoritmo
    /// </summary>
    public bool padres = false;


    [Header("Sin Padres")]
    /// <summary>
    /// Tamaño de la poblacion de lineas
    /// </summary>
    public int populationSize = 20;

    /// <summary>
    /// Cantidad de esferas por individuo
    /// </summary>
    [Header("Con Padres")]
    public int spheresPerIndividuos = 20;

    /// <summary>
    /// En caso de que no tenga padres
    /// </summary>
    private float mejorScore;

    /// <summary>
    /// En caso de que tenga padres
    /// </summary>
    private GeneticIndividual mostFitElement;

    /// <summary>
    /// Poblacion en caso de que sea con padres
    /// </summary>
    private GeneticParents poblation;

    /// <summary>
    /// Se llama la primera vez que se instancia el objeto
    /// </summary>
    public void Initialize()
    {
        if (!padres)
        {
            mejorScore = float.MaxValue;
            mostFitElement = new GeneticIndividual(populationSize);
            FirstGenerationSinPadres();
        }
        else
        {
            poblation = new GeneticParents(populationSize, spheresPerIndividuos);
            FirstGenerationConPadres();
        }

    }

    /// <summary>
    /// Primera generacion con padres
    /// </summary>
    private void FirstGenerationConPadres()
    {

        GameManager.Instance.imageReader.fillTexture(Color.black, GameManager.Instance.imageReader.temporalTexture);
        poblation.paint(GameManager.Instance.imageReader.temporalTexture, 0);
        GameManager.Instance.imageReader.reloadTemporalTexture();

    }


    /// <summary>
    /// Crea la primera generacion
    /// </summary>
    public void FirstGenerationSinPadres()
    {
        mostFitElement = new GeneticIndividual(populationSize);

        mostFitElement.paint(GameManager.Instance.imageReader.temporalTexture);

        GameManager.Instance.imageReader.reloadTemporalTexture();
    }

    /// <summary>
    /// Realiza una generacion 
    /// </summary>
    public void doGeneration()
    {
        if (!padres)
        {
            NextGenerationSinPadres();
        }
        else
        {
            NextGenerationConPadres();

        }
    }

    /// <summary>
    /// Siguiente generacion con padres
    /// </summary>
    private void NextGenerationConPadres()
    {
        poblation.evaluateScore(GameManager.Instance.imageReader.chunkOriginalTexture);

        GameManager.Instance.imageReader.fillTexture(Color.black, GameManager.Instance.imageReader.temporalTexture);
        poblation.paint(GameManager.Instance.imageReader.temporalTexture, 0);
        GameManager.Instance.imageReader.reloadTemporalTexture();

        poblation = GenerarNextGeneracion();
    }

    /// <summary>
    /// Genera la siguiente generacion oara los padres
    /// </summary>
    /// <returns></returns>
    private GeneticParents GenerarNextGeneracion()
    {
        poblation.orderByScore();


        List<GeneticIndividual> newPopulation = new List<GeneticIndividual>();

        //Elitismo
        newPopulation.Add(new GeneticIndividual(poblation.individuals.First().geneticElements));

        while (newPopulation.Count < poblation.individuals.Count)
        {
            var one = poblation.select();
            var two = poblation.select();
            newPopulation.Add(poblation.Mutate(one, two, mutationRatio));
        }

        return new GeneticParents(newPopulation);
    }
    
    /// <summary>
    /// Procrea la siguiente generacion
    /// </summary>
    public void NextGenerationSinPadres()
    {

        GeneticIndividual child = mostFitElement.mutate(mutationRatio);

        GameManager.Instance.imageReader.fillTexture(Color.black, GameManager.Instance.imageReader.temporalTexture);
        child.paint(GameManager.Instance.imageReader.temporalTexture);
        float score = ImageComparator.Instance.compareTextures(GameManager.Instance.imageReader.chunkOriginalTexture, GameManager.Instance.imageReader.temporalTexture);


        if (score < mejorScore)
        {
            mejorScore = score;
            mostFitElement = child;

            GameManager.Instance.imageReader.fillTexture(Color.black, GameManager.Instance.imageReader.temporalTexture);
            mostFitElement.paint(GameManager.Instance.imageReader.temporalTexture);
            GameManager.Instance.imageReader.reloadTemporalTexture();
        }


        GameManager.Instance.imageReader.reloadTemporalTexture();


    }
}
