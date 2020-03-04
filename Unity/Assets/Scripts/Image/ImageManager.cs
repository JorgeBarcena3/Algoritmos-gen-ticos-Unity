using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Color = UnityEngine.Color;

/// <summary>
/// Lee una imagen y la transforma 
/// </summary>
public class ImageManager : MonoBehaviour
{
    /// <summary>
    /// Sprite donde se va a dibujar el resultado
    /// </summary>
    public Image resultSprite;

    /// <summary>
    /// Sprite donde se va a dibujar la imagen original
    /// </summary>
    public Image originalSprite;

    /// <summary>
    /// Sprite por chunk
    /// </summary>
    public Image temporalSprite;

    /// <summary>
    /// Sprite por chunk
    /// </summary>
    public Image chunkSprite;

    /// <summary>
    /// Textura original
    /// </summary>
    public Texture2D originalTexture { get; set; }

    /// <summary>
    /// Textura del chunk
    /// </summary>
    public Texture2D chunkOriginalTexture { get; set; }

    /// <summary>
    /// Textura original
    /// </summary>
    public Texture2D temporalTexture { get; set; }

    /// <summary>
    /// Textura final
    /// </summary>
    public Texture2D finalTexture { get; set; }

    /// <summary>
    /// Posiciones de los chunks
    /// </summary>
    private List<Vector2Int> positions;

    /// <summary>
    /// Si la imagen se ha finalizado o no
    /// </summary>
    public bool finaliced { get; set; } = false;

    /// <summary>
    /// Posicion del chunk en el que nos encontramos
    /// </summary>
    private int indexPosition = 0;

    /// <summary>
    /// Cambia al siguiente chunk
    /// </summary>
    public void NextChunk()
    {

        //COgemos una region
        Color[] pixels = temporalTexture.GetPixels(
            0,
            0,
             (positions[indexPosition].x + GeneticController.Instance.pixelChunk > finalTexture.width) ? finalTexture.width - positions[indexPosition].x : GeneticController.Instance.pixelChunk,
             (positions[indexPosition].y + GeneticController.Instance.pixelChunk > finalTexture.height) ? finalTexture.height - positions[indexPosition].y : GeneticController.Instance.pixelChunk
            );

        //GUARDAMOS EL CHUNK ACTUAL
        finalTexture.SetPixels(
            positions[indexPosition].x,
            positions[indexPosition].y,
            (positions[indexPosition].x + GeneticController.Instance.pixelChunk > finalTexture.width) ? finalTexture.width - positions[indexPosition].x : GeneticController.Instance.pixelChunk,
            (positions[indexPosition].y + GeneticController.Instance.pixelChunk > finalTexture.height) ? finalTexture.height - positions[indexPosition].y : GeneticController.Instance.pixelChunk,
            pixels);

        reloadFinalTexture();


        ///GENERAMOS EL SIGUIENTE CHUNK
        indexPosition++;

        if (indexPosition >= positions.Count)
        {
            finaliced = true;
            GameManager.Instance.state = STATE.OFF;
            byte[] _bytes = finalTexture.EncodeToPNG();
            System.IO.File.WriteAllBytes(Application.streamingAssetsPath + "/" + DateTime.Now.ToString("finalOutput_yyyyMMddHHmmss") + ".png", _bytes);
        }
        else
        {

            chunkOriginalTexture = new Texture2D(GeneticController.Instance.pixelChunk, GeneticController.Instance.pixelChunk);


            chunkOriginalTexture.SetPixels(
                0,
                0,
                (positions[indexPosition].x + GeneticController.Instance.pixelChunk > originalTexture.width) ? originalTexture.width - positions[indexPosition].x : GeneticController.Instance.pixelChunk,
                (positions[indexPosition].y + GeneticController.Instance.pixelChunk > originalTexture.height) ? originalTexture.height - positions[indexPosition].y : GeneticController.Instance.pixelChunk,
                originalTexture.GetPixels(
                    positions[indexPosition].x,
                    positions[indexPosition].y,
                    (positions[indexPosition].x + GeneticController.Instance.pixelChunk > originalTexture.width) ? originalTexture.width - positions[indexPosition].x : GeneticController.Instance.pixelChunk,
                    (positions[indexPosition].y + GeneticController.Instance.pixelChunk > originalTexture.height) ? originalTexture.height - positions[indexPosition].y : GeneticController.Instance.pixelChunk
                ));

            reloadChunkTexture();

            /// Limpiamos el temporal chunk
            /// 
            fillTexture(Color.black, temporalTexture);

            reloadTemporalTexture();

        }

    }

    /// <summary>
    /// Se guarda la imagen final
    /// En caso de que estemos en el editor, se abre una ventana para seleccionar donde guardarlo
    /// </summary>
    public void saveFinalImage()
    {
        byte[] _bytes = finalTexture.EncodeToPNG();

        string path = Application.streamingAssetsPath;

#if UNITY_EDITOR
        path = UnityEditor.EditorUtility.OpenFolderPanel("Selecciona una carpeta para guardar", "", ""); ;
#endif

        System.IO.File.WriteAllBytes(path + "/" + DateTime.Now.ToString("Output_yyyyMMddHHmmss") + ".png", _bytes);
    }
       
    /// <summary>
    /// Se dibuja la esfera en una posicion
    /// </summary>
    /// <param name="center"></param>
    /// <param name="radius"></param>
    /// <param name="color"></param>
    public void drawSphere(Vector2 center, float radius, UnityEngine.Color color, Texture2D texture)
    {
        var pixels = getPixelsInRange(center, radius, texture);

        for (int i = 0; i < pixels.Count; i++)
        {
            color.a = 1;
            texture.SetPixel((int)pixels[i].x, (int)pixels[i].y, color);
        }

    }

    /// <summary>
    /// Crea la textura final y la pinta
    /// </summary>
    public void reloadFinalTexture()
    {
        byte[] resultsByte = finalTexture.EncodeToPNG();

        finalTexture.LoadImage(resultsByte);

        resultSprite.sprite = Sprite.Create(finalTexture, new Rect(0.0f, 0.0f, finalTexture.width, finalTexture.height), new Vector2(0.5f, 0.5f), 10.0f);

    }

    /// <summary>
    /// Crea la textura final y la pinta
    /// </summary>
    public void reloadChunkTexture()
    {
        byte[] resultsByte = chunkOriginalTexture.EncodeToPNG();

        chunkOriginalTexture.LoadImage(resultsByte);

        chunkSprite.sprite = Sprite.Create(chunkOriginalTexture, new Rect(0.0f, 0.0f, chunkOriginalTexture.width, chunkOriginalTexture.height), new Vector2(0.5f, 0.5f), 10.0f);

    }

    /// <summary>
    /// Crea la textura final y la pinta
    /// </summary>
    public void reloadTemporalTexture()
    {
        byte[] resultsByte = temporalTexture.EncodeToPNG();

        temporalTexture.LoadImage(resultsByte);

        temporalSprite.sprite = Sprite.Create(temporalTexture, new Rect(0.0f, 0.0f, temporalTexture.width, temporalTexture.height), new Vector2(0.5f, 0.5f), 10.0f);

    }


    /// <summary>
    /// Devuelve los pixeles dentro de una esfera
    /// </summary>
    /// <returns></returns>
    private List<Vector2> getPixelsInRange(Vector2 center, float radius, Texture2D texture)
    {

        List<Vector2> pixels = new List<Vector2>();

        float squareRadius = radius * radius;

        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                float dx = x - center.x;
                float dy = y - center.y;
                float distanceSquared = dx * dx + dy * dy;

                if (distanceSquared <= squareRadius)
                {
                    pixels.Add(new Vector2(x, y));
                }
            }
        }

        return pixels;
    }

    /// <summary>
    /// Lee una imagen y la transforma
    /// En caso de que estemos en el editor deja elegir que imagen
    /// Si no, coge la carpeta de streamingAssets el archivo original
    /// </summary>
    public void readImage()
    {

        finaliced = false;

        string filepath = Application.streamingAssetsPath + "/original.png";

#if UNITY_EDITOR
        filepath = UnityEditor.EditorUtility.OpenFilePanel("Selecciona un archivo", "", "png");
#endif
        

        byte[] bytes = File.ReadAllBytes(filepath);

        originalTexture = new Texture2D(100, 100);

        originalTexture.LoadImage(bytes);

        originalSprite.sprite = Sprite.Create(originalTexture, new Rect(0.0f, 0.0f, originalTexture.width, originalTexture.height), new Vector2(0.5f, 0.5f), 10.0f);

        dividirTextura(originalTexture, GeneticController.Instance.pixelChunk);

        //Textura final

        finalTexture = new Texture2D(originalTexture.width, originalTexture.height);

        fillTexture(Color.black, finalTexture);

        reloadFinalTexture();


        //Textura por chunks

        temporalTexture = new Texture2D(GeneticController.Instance.pixelChunk, GeneticController.Instance.pixelChunk);

        fillTexture(Color.black, temporalTexture);

        reloadTemporalTexture();

        chunkOriginalTexture = new Texture2D(GeneticController.Instance.pixelChunk, GeneticController.Instance.pixelChunk);

        chunkOriginalTexture.SetPixels(0, 0, chunkOriginalTexture.width, chunkOriginalTexture.height, originalTexture.GetPixels(positions[0].x, positions[0].y, GeneticController.Instance.pixelChunk, GeneticController.Instance.pixelChunk));

        reloadChunkTexture();

    }

    /// <summary>
    /// Se divide la textura en partes iguales
    /// </summary>
    /// <param name="texture"></param>
    /// <param name="size"></param>
    private void dividirTextura(Texture2D texture, int size)
    {
        positions = new List<Vector2Int>();

        for (int x = 0; x < texture.width; x += size)
        {
            for (int y = 0; y < texture.height; y += size)
            {
                positions.Add(new Vector2Int(x, y));
            }
        }

        indexPosition = 0;
    }

    /// <summary>
    /// Se llena una textura de un color en especifico
    /// </summary>
    /// <param name="color"></param>
    /// <param name="texture"></param>
    public void fillTexture(Color color, Texture2D texture)
    {
        for (int x = 0; x < texture.width; x++)
        {
            for (int y = 0; y < texture.height; y++)
            {
                texture.SetPixel(x, y, color);
            }
        }
    }

}
