using System;
using System.Collections;
using System.Collections.Generic;
using Sample5_Shooter;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GenerateMeshFile))]
public class GenerateMeshFileEditor : Editor
{
    private GenerateMeshFile _generateMeshFile;
    
    private void OnEnable()
    {
        _generateMeshFile = target as GenerateMeshFile;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate"))
        {
            var mf = _generateMeshFile.GetComponent<MeshFilter>();
            var sharedMesh = Instantiate(mf.sharedMesh);
            AssetDatabase.CreateAsset(sharedMesh, Application.dataPath + "/" + sharedMesh.name + ".mesh");
            AssetDatabase.SaveAssets();
        }
    }
}
