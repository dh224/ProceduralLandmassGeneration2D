using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CustomEditor(typeof(MapGenernator))]
public class MapGeneratorEditor :Editor
{
    public override void OnInspectorGUI()
    {
        MapGenernator mapGen = (MapGenernator) target;
        if (DrawDefaultInspector())
        {
            if (mapGen.autoUpdate)
            {
                mapGen.GenerateMap();
            }
        }
        if (GUILayout.Button("Genernate"))
        {
            mapGen.GenerateMap();
        }
    }
}
