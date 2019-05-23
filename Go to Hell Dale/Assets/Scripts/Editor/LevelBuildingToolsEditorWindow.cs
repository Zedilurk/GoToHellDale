using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class LevelBuildingToolsEditorWindow : EditorWindow
{
    private bool genericGroupEnabled = false;
    private bool gluttonyGroupEnabled = false;

    private enum GenericEnemyTypesEnum { Launcher, Bouncer, Runner, Thrower, JumpStack };
    private GenericEnemyTypesEnum SelectedGenericEnemy;

    private enum GluttonyEnemyTypesEnum { Shotgun, Bouncer, Runner, Thrower, JumpStack  };
    private GluttonyEnemyTypesEnum SelectedGluttonyEnemy;

    /// <summary>
    /// Display the Scene Editor Window when the Show Window method is called via opening the menu in the Unity Editor UI
    /// </summary>
    [MenuItem("Go To Hell, Dale/Level Building Tools", false, 1)]
    public static void ShowWindow()
    {
        EditorWindow window = EditorWindow.GetWindow(typeof(LevelBuildingToolsEditorWindow), false, "Dale - Level Building Tools");
        window.minSize = new Vector2(500, 300);
        window.maxSize = new Vector2(500, 300);

        window.minSize = new Vector2(100, 100);
        window.maxSize = new Vector2(10000, 10000);
    }

    void OnGUI()
    {
        GUILayout.Label("Level Structure", EditorStyles.boldLabel);

        if (GUILayout.Button("Create New Tilemap"))
            CreateNewTileMap();

        EditorGUILayout.Separator();

        GUILayout.Label("Level Design", EditorStyles.boldLabel);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("New Moving Platform"))
            CreateMovingPlatform();

        if (GUILayout.Button("New Checkpoint"))
            CreateCheckpoint();

        if (GUILayout.Button("New Boss Zone"))
            CreateBossZone();
        GUILayout.EndHorizontal();

        EditorGUILayout.Separator();

        GUILayout.Label("Enemies", EditorStyles.boldLabel);

        genericGroupEnabled = EditorGUILayout.Foldout(genericGroupEnabled, "Generic Enemies");
        if (genericGroupEnabled)
        {
            GUILayout.BeginHorizontal();
            SelectedGenericEnemy = (GenericEnemyTypesEnum)EditorGUILayout.EnumPopup(SelectedGenericEnemy);
            if (GUILayout.Button("Create " + SelectedGenericEnemy, GUILayout.Height(20)))
                CreateGenericEnemy(SelectedGenericEnemy);
            GUILayout.EndHorizontal();
        }

        gluttonyGroupEnabled = EditorGUILayout.Foldout(gluttonyGroupEnabled, "Gluttony Enemies");
        if (gluttonyGroupEnabled)
        {
            GUILayout.BeginHorizontal();
            SelectedGluttonyEnemy = (GluttonyEnemyTypesEnum)EditorGUILayout.EnumPopup(SelectedGluttonyEnemy);
            if (GUILayout.Button("Create " + SelectedGluttonyEnemy, GUILayout.Height(20)))
                CreateGluttonyEnemy(SelectedGluttonyEnemy);
            GUILayout.EndHorizontal();
        }
    }

    void CreateNewTileMap ()
    {
        GameObject defaultTilemapPrefab = Resources.Load<GameObject>("Scene Setup/Default Tilemap");
        GameObject tilemap = GameObject.Instantiate(defaultTilemapPrefab);
        Selection.activeGameObject = tilemap;
    }

    void CreateGenericEnemy (GenericEnemyTypesEnum enemyType)
    {
        GameObject enemyPrefab = null;
        switch (enemyType)
        {
            case GenericEnemyTypesEnum.Bouncer:
                enemyPrefab = Resources.Load<GameObject>("Enemies/Generic/Bouncer");
                break;
            case GenericEnemyTypesEnum.JumpStack:
                enemyPrefab = Resources.Load<GameObject>("Enemies/Generic/Jump Stack");
                break;
            case GenericEnemyTypesEnum.Launcher:
                enemyPrefab = Resources.Load<GameObject>("Enemies/Generic/Launcher");
                break;
            case GenericEnemyTypesEnum.Runner:
                enemyPrefab = Resources.Load<GameObject>("Enemies/Generic/Runner");
                break;
            case GenericEnemyTypesEnum.Thrower:
                enemyPrefab = Resources.Load<GameObject>("Enemies/Generic/Thrower");
                break;
        }

        Camera sceneCamera = SceneView.lastActiveSceneView.camera;
        Vector3 spawnPos = new Vector3(sceneCamera.transform.position.x, sceneCamera.transform.position.y, 0);

        if (enemyPrefab != null)
        {
            GameObject obj = GameObject.Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            Selection.activeGameObject = obj;
        }            
    }

    void CreateGluttonyEnemy(GluttonyEnemyTypesEnum enemyType)
    {
        GameObject enemyPrefab = null;
        switch (enemyType)
        {
            case GluttonyEnemyTypesEnum.Bouncer:
                enemyPrefab = Resources.Load<GameObject>("Enemies/Gluttony/Bouncer");
                break;
            case GluttonyEnemyTypesEnum.JumpStack:
                enemyPrefab = Resources.Load<GameObject>("Enemies/Gluttony/Jump Stack");
                break;
            case GluttonyEnemyTypesEnum.Shotgun:
                enemyPrefab = Resources.Load<GameObject>("Enemies/Gluttony/Shotgun");
                break;
            case GluttonyEnemyTypesEnum.Runner:
                enemyPrefab = Resources.Load<GameObject>("Enemies/Gluttony/Runner");
                break;
            case GluttonyEnemyTypesEnum.Thrower:
                enemyPrefab = Resources.Load<GameObject>("Enemies/Gluttony/Thrower");
                break;
        }

        Camera sceneCamera = SceneView.lastActiveSceneView.camera;
        Vector3 spawnPos = new Vector3(sceneCamera.transform.position.x, sceneCamera.transform.position.y, 0);

        if (enemyPrefab != null)
        {
            GameObject obj = GameObject.Instantiate(enemyPrefab, spawnPos, Quaternion.identity);
            Selection.activeGameObject = obj;
        }
    }

    void CreateMovingPlatform ()
    {
        GameObject go = Resources.Load<GameObject>("Level Building/MovingPlatform");
        Camera sceneCamera = SceneView.lastActiveSceneView.camera;
        Vector3 spawnPos = new Vector3(sceneCamera.transform.position.x, sceneCamera.transform.position.y, 0);

        if (go != null)
        {
            GameObject obj = GameObject.Instantiate(go, spawnPos, Quaternion.identity);
            Selection.activeGameObject = obj;
        }
    }

    void CreateCheckpoint()
    {
        GameObject go = Resources.Load<GameObject>("Level Building/Checkpoint");
        Camera sceneCamera = SceneView.lastActiveSceneView.camera;
        Vector3 spawnPos = new Vector3(sceneCamera.transform.position.x, sceneCamera.transform.position.y, 0);

        if (go != null)
        {
            GameObject obj = GameObject.Instantiate(go, spawnPos, Quaternion.identity);
            Selection.activeGameObject = obj;
        }
    }

    void CreateBossZone ()
    {
        GameObject go = Resources.Load<GameObject>("Level Building/BossZone");
        Camera sceneCamera = SceneView.lastActiveSceneView.camera;
        Vector3 spawnPos = new Vector3(sceneCamera.transform.position.x, sceneCamera.transform.position.y, 0);

        if (go != null)
        {
            GameObject obj = GameObject.Instantiate(go, spawnPos, Quaternion.identity);
            Selection.activeGameObject = obj;
        }
    }
}
