using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;


namespace Rec.Editor{
public static class Launcher
{
    [MenuItem("Rec/Beta/Launch")]
    private static void Launch(){
            for(int i = 0; i < EditorBuildSettings.scenes.Length; i++){
            var scene = EditorBuildSettings.scenes[i];
            if(scene.path.IndexOf("Init",StringComparison.Ordinal) != -1){
                var sceneAsset = AssetDatabase.LoadAssetAtPath(scene.path,typeof(SceneAsset)) as SceneAsset;
                EditorSceneManager.playModeStartScene = sceneAsset;
                EditorApplication.ExecuteMenuItem("Edit/Play");
            }
        }
    }
}
}
