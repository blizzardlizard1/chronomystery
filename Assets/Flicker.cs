using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flicker: MonoBehaviour
{
    public Material filmMaterial; // Assign your custom shader material

    [Range(0f, 1f)] public float flickerSpeed = 0.1f;
    [Range(0f, 0.5f)] public float flickerIntensity = 0.5f;

    private float nextFlicker;
    private float currentBrightness = 1f;

    void Update()
    {
        if (Time.time > nextFlicker)
        {
            currentBrightness = 1f - Random.Range(0f, flickerIntensity);
            nextFlicker = Time.time + Random.Range(0.05f, flickerSpeed);
        }

        if (filmMaterial != null)
            filmMaterial.SetFloat("_Brightness", currentBrightness);
    }

    void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (filmMaterial != null)
            Graphics.Blit(src, dest, filmMaterial);
        else
            Graphics.Blit(src, dest);
    }
}