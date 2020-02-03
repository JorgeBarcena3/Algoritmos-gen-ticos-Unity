using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Elemento que se utilizara para el algoritmo
/// </summary>
public abstract class GeneticElement
{
    /// <summary>
    /// Lista de los coromosomas del elemento
    /// </summary>
    public Gen genes;

    /// <summary>
    /// Inicializa el objeto
    /// </summary>
    public abstract void Initialize();

       /// <summary>
    /// Funcion que pinta el individuo en una textura
    /// </summary>
    /// <param name="texture"></param>
    public abstract void paint(Texture2D texture);

}
