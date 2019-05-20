using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using UnityEditor.SceneManagement;

public class SceneCreatorEditorWindow : EditorWindow
{
    private string SceneName = "New Scene";
    string[] sceneTypeOptions = new string[]
    {
         "Gameplay", "Scripted (Not setup yet)"
    };
    int selectedSceneType = 0;

    /// <summary>
    /// Display the Scene Editor Window when the Show Window method is called via opening the menu in the Unity Editor UI
    /// </summary>
    [MenuItem("Go To Hell, Dale/Scene Creation Wizard", false, 0)]
    public static void ShowWindow()
    {
        EditorWindow window = EditorWindow.GetWindow(typeof(SceneCreatorEditorWindow), false, "Dale - Scene Creation Wizard");
        window.minSize = new Vector2(300, 100);
        window.maxSize = new Vector2(300, 100);

        window.minSize = new Vector2(100, 100);
        window.maxSize = new Vector2(10000, 10000);
    }

    void OnGUI()
    {
        GUILayout.Label("Scene Building Settings", EditorStyles.boldLabel);
        SceneName = EditorGUILayout.TextField("Scene Name", SceneName);
        selectedSceneType = EditorGUILayout.Popup("Scene Type", selectedSceneType, sceneTypeOptions);

        if (GUILayout.Button("Create Scene"))
            CreateNewScene(SceneName, sceneTypeOptions[selectedSceneType]);

        //EditorGUILayout.DropdownButton

        /*
        groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);
        myBool = EditorGUILayout.Toggle("Toggle", myBool);
        myFloat = EditorGUILayout.Slider("Slider", myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();
        */
    }

    public void CreateNewScene(string sceneName, string sceneType)
    {
        string savePath = "Assets/Scenes/" + sceneName + ".unity";
        Scene newScene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
        newScene.name = sceneName;

        switch (sceneType)
        {
            case "Gameplay":
                GameObject baseSceneDependenciesPrefab = Resources.Load<GameObject>("Scene Setup/Base Scene Dependencies");
                GameObject debugMenuPrefab = Resources.Load<GameObject>("Scene Setup/DebugMenu");
                GameObject defaultTilemapPrefab = Resources.Load<GameObject>("Scene Setup/Default Tilemap");

                GameObject sceneDependencies = GameObject.Instantiate(baseSceneDependenciesPrefab);
                GameObject debugMenu = GameObject.Instantiate(debugMenuPrefab);
                GameObject defaultTilemap = GameObject.Instantiate(defaultTilemapPrefab);

                LuminosityEditor.IO.UIInputModuleVersionManager.FixEventSystem();
                break;
            case "Scripted":

                break;
            default:

                break;
        }        

        EditorSceneManager.SaveScene(newScene, savePath, false);
    }
}
