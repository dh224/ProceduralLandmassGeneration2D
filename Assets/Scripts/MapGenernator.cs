﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class MapGenernator : MonoBehaviour
{
    public enum DrawMode
    {
        NoiseMap,
        ColorMap,
        Mesh,
    };

    public DrawMode drawMode;

    public const int mapChunkSize = 241;
    [Range(0,6)]
    public int levelOfDetail;
    public float noiseScale;
    public bool autoUpdate;
    public int octaves;
    [Range(0,1)]
    public float persistance;
    [Range(1,50)]
    public float heightMultiplier = 5;

    public AnimationCurve meshHeightCurve;
    
    public float lacunarity;
    public int seed;
    public Vector2 offset;

    public TerrainType[] regions;
    
    public void GenerateMap()
    {
        float[,] noiseMap = Noise.GenerateNoiseMap(mapChunkSize, mapChunkSize,seed, noiseScale,octaves,persistance,lacunarity,offset);
        Color[] colorMap = new Color[mapChunkSize * mapChunkSize];
        for (int y = 0; y < mapChunkSize; y++)
        {
            for (int x = 0; x < mapChunkSize; x++)
            {
                float currentHeight = noiseMap[x, y];
                for (int i = 0; i < regions.Length; i++)
                {
                    if (currentHeight <= regions[i].height)
                    {
                        colorMap[y * mapChunkSize + x] = regions[i].color;
                        break;
                    }
                }
            }
        }
        MapDisplay display = FindObjectOfType<MapDisplay>();
        switch (drawMode)
        {
            case DrawMode.NoiseMap: display.DrawTexture(TextureGenerator.TextureFromHeightMap(noiseMap)); break;
            
            case DrawMode.ColorMap: display.DrawTexture(TextureGenerator.textureFromColorMap(colorMap,mapChunkSize,mapChunkSize));
                break;
            case DrawMode.Mesh: display.DrawMesh(MeshGenerator.GenerateTerrainMesh(noiseMap,heightMultiplier,meshHeightCurve,levelOfDetail),TextureGenerator.textureFromColorMap(colorMap,mapChunkSize,mapChunkSize));
                break;
        }
        
    }

    private void OnValidate()
    {
        // if (mapChunkSize < 1) mapChunkSize = 1;
        // if (mapChunkSize < 1) mapChunkSize = 1;
        if (lacunarity < 1) lacunarity = 1;
        if (octaves < 0) octaves = 0;
    }
}
[Serializable]
public struct TerrainType
{
    public string name;
    public float height;
    public Color color;
}
