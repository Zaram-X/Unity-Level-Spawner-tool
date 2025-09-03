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
        enemyPrefab = (GameObject)EditorGUILayout.ObjectField ("Enemy prefab", enemyPrefab, typeof(GameObject), false);
        platformPrefab= (GameObject)EditorGUILayout.ObjectField ("Platform Prefab", platformPrefab, typeof(GameObject), false);


        GUILayout.Space(10);
        GUILayout.Label("Drag your prefabs here!", EditorStyles.helpBox);
    }
}
