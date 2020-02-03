using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum STATE
{
    OFF,
    ON
}

/// <summary>
/// Manager de archivos
/// </summary>
public class GameManager : Singelton<GameManager>
{

    /// <summary>
    /// Instancia del lector de imagenes
    /// </summary>
    public ImageManager imageReader;

    /// <summary>
    /// Controlador del algoritmo 
    /// </summary>
    public GeneticController geneticController;

    /// <summary>
    /// Semilla de generacion del mundo
    /// </summary>
    public int seed = 0;

    public STATE state { get; set; }

    /// <summary>
    /// Funcion del start
    /// </summary>
    public void Start()
    {
        Random.InitState(seed != 0 ? seed : System.DateTime.Now.Millisecond);

    }

    /// <summary>
    /// Abrir archivo de imagen
    /// </summary>
    public void openFile()
    {
        imageReader.readImage();
        geneticController.Initialize();

    }

    public void saveImage()
    {
        imageReader.saveFinalImage();
        state = STATE.OFF;
    }

    private float currentTime = 0;
    

    public void startGeneration()
    {
        if (!imageReader.finaliced)
            state = STATE.ON;
    }

    private void Update()
    {

        if (state == STATE.ON)
        {
            currentTime += Time.deltaTime;

            if (currentTime > geneticController.chunkTimeInSeconds)
            {
                imageReader.NextChunk();
                geneticController.Initialize();
                currentTime = 0;
            }

            geneticController.doGeneration();

        }

    }

}
