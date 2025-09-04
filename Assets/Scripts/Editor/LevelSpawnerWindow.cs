using UnityEngine;
using UnityEditor;

public class LevelSpawnerWindow : EditorWindow
{
    // Prefab slots
    private GameObject coinPrefab;
    private GameObject enemyPrefab;
    private GameObject platformPrefab;

    // new spawn postion field
    private Vector3 spawnPosition = Vector3.zero;

    [MenuItem("Tools/Level Spawner")]
    //Add a menu item under tools > level spawner
    public static void ShowWindow()
    {
        // Opens the Editor Window 
        GetWindow<LevelSpawnerWindow>("Level Spawner");
    }

    private void OnGUI()
    {
        // UI content will go here Later
        GUILayout.Label("Level spawner Tool", EditorStyles.boldLabel); //Adds a bold title at the top of the window.

        // Prefab input field
        coinPrefab = (GameObject)EditorGUILayout.ObjectField("Coin Prefab", coinPrefab, typeof(GameObject), false);
        enemyPrefab = (GameObject)EditorGUILayout.ObjectField("Enemy prefab", enemyPrefab, typeof(GameObject), false);
        platformPrefab = (GameObject)EditorGUILayout.ObjectField("Platform Prefab", platformPrefab, typeof(GameObject), false);


        GUILayout.Space(10);
        // GUILayout.Label("Drag your prefabs here!", EditorStyles.helpBox);

        // spawn postion fied
        spawnPosition = EditorGUILayout.Vector3Field("Spawn Position", spawnPosition);

        //spawn at selected object's position
        if (GUILayout.Button("Use Selected Object"))
        {
            if (Selection.activeGameObject != null)
            {
                spawnPosition = Selection.activeGameObject.transform.position;
                Debug.Log($" Spawn position set to {spawnPosition} (from {Selection.activeGameObject.name})");
            }
            else
            {
                Debug.LogWarning("No Object selected in this scene");
            }
        }

        GUILayout.Space(10);

        //Spawn buttons
        if (GUILayout.Button("Spawn Coin"))
        {
            SpawnPrefab(coinPrefab);
        }
        if (GUILayout.Button("Spawn Enemy"))
        {
            SpawnPrefab(enemyPrefab);
        }
        if (GUILayout.Button("Spawn Platform"))
        {
            SpawnPrefab(platformPrefab);
        }

    }

    private void SpawnPrefab(GameObject prefab)
    {
        if (prefab != null)
        {
            // spawn prefab at chosen position
            GameObject spawned = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            spawned.transform.position = spawnPosition;

            //mark scene dirty(so Unity knows it changed)
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(spawned.scene);
             Debug.Log($"✅ Spawned: {prefab.name} at {spawnPosition}");
        }
        else
        {
            Debug.LogWarning("No prefab assigned");
        }
    }
}
