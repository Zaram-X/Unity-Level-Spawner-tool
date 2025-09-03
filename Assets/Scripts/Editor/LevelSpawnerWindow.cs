using UnityEngine;
using UnityEditor;

public class LevelSpawnerWindow : EditorWindow
{
    // Prefab slots
    private GameObject coinPrefab;
    private GameObject enemyPrefab;
    private GameObject platformPrefab;

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
        GUILayout.Label("Level spawner Tool", EditorStyles.boldLabel);

        // Prefab input field
        coinPrefab = (GameObject)EditorGUILayout.ObjectField("CoinP refab", coinPrefab, typeof(GameObject), false);
        enemyPrefab = (GameObject)EditorGUILayout.ObjectField("Enemy prefab", enemyPrefab, typeof(GameObject), false);
        platformPrefab = (GameObject)EditorGUILayout.ObjectField("Platform Prefab", platformPrefab, typeof(GameObject), false);


        GUILayout.Space(10);
        // GUILayout.Label("Drag your prefabs here!", EditorStyles.helpBox);

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
            // spawn prefab at world postion (0,0,0)
            GameObject spawned = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
            spawned.transform.position = Vector3.zero;

            //mark scene dirty(so Unity knows it changed)
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(spawned.scene);
             Debug.Log($"✅ Spawned: {prefab.name}");
        }
        else
        {
            Debug.LogWarning("No prefab assigned");
        }
    }
}
