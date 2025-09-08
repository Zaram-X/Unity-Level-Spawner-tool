using UnityEngine;
using UnityEditor;

public class LevelSpawnerWindow : EditorWindow
{
   // private bool showSpawnSettings = true;
   // private bool showOptions = true;
    private bool showRandomization = true;

    // Randomization both location and scale 
    private bool randomizeRotation = false;
    private Vector3 minRotation = Vector3.zero;
    private Vector3 maxRotation = Vector3.zero;

    private bool randomizeScale = false;
    private Vector3 minScale = Vector3.one;
    private Vector3 maxScale = Vector3.one;

    // Prefab slots
    private GameObject coinPrefab;
    private GameObject enemyPrefab;
    private GameObject platformPrefab;

    // Spawn position settings
    private Vector3 spawnPosition = Vector3.zero;

    // Options
    private bool randomizePosition = false;
    private Vector3 randomRange = new Vector3(5, 0, 5);
    private int spawnCount = 1;

    [MenuItem("Tools/Level Spawner")]
    public static void ShowWindow()
    {
        GetWindow<LevelSpawnerWindow>("Level Spawner");
    }

    private void OnGUI()
    {
        GUILayout.Label("Level Spawner Tool", EditorStyles.boldLabel);

        // === Prefab Section ===
        GUILayout.Label("Prefabs", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");
        coinPrefab = (GameObject)EditorGUILayout.ObjectField(
            new GUIContent("Coin Prefab", "Drag your coin prefab here"),
            coinPrefab, typeof(GameObject), false);

        enemyPrefab = (GameObject)EditorGUILayout.ObjectField(
            new GUIContent("Enemy Prefab", "Drag your enemy prefab here"),
            enemyPrefab, typeof(GameObject), false);

        platformPrefab = (GameObject)EditorGUILayout.ObjectField(
            new GUIContent("Platform Prefab", "Drag your platform prefab here"),
            platformPrefab, typeof(GameObject), false);
        EditorGUILayout.EndVertical();

        GUILayout.Space(10);

        // === Position Section ===
        GUILayout.Label("Spawn Settings", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");
        spawnPosition = EditorGUILayout.Vector3Field("Spawn Position", spawnPosition);

        if (GUILayout.Button("Use Selected Object Position"))
        {
            if (Selection.activeGameObject != null)
            {
                spawnPosition = Selection.activeGameObject.transform.position;
                Debug.Log($" Spawn position set to {spawnPosition} (from {Selection.activeGameObject.name})");
            }
            else
            {
                Debug.LogWarning("No object selected in the scene.");
            }
        }
        EditorGUILayout.EndVertical();

        GUILayout.Space(10);

        // === Options Section ===
        GUILayout.Label("Options", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical("box");
        randomizePosition = EditorGUILayout.Toggle("Randomize Position", randomizePosition);
        if (randomizePosition)
        {
            randomRange = EditorGUILayout.Vector3Field("Random Range", randomRange);
        }

        spawnCount = EditorGUILayout.IntField("Spawn Count", spawnCount);
        spawnCount = Mathf.Max(1, spawnCount); // Prevent negative/zero
        EditorGUILayout.EndVertical();

        GUILayout.Space(10);

        // === Randomization settings ===
        showRandomization = EditorGUILayout.Foldout(showRandomization, "Randomization Settings", true);
        if (showRandomization)
        {
            EditorGUILayout.BeginVertical("box");

            // Rotation
            randomizeRotation = EditorGUILayout.Toggle("Randomize Rotation", randomizeRotation);
            if (randomizeRotation)
            {
                minRotation = EditorGUILayout.Vector3Field("Min Rotation", minRotation);
                maxRotation = EditorGUILayout.Vector3Field("Max Rotation", maxRotation);
            }

            // Scale
            randomizeScale = EditorGUILayout.Toggle("Randomize Scale", randomizeScale);
            if (randomizeScale)
            {
                minScale = EditorGUILayout.Vector3Field("Min Scale", minScale);
                maxScale = EditorGUILayout.Vector3Field("Max Scale", maxScale);
            }

            EditorGUILayout.EndVertical();
        }


        GUILayout.Space(10);


        // === Spawn Buttons ===
        GUILayout.Label("Spawn Prefabs", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("Spawn Coin")) { SpawnPrefab(coinPrefab); }
        if (GUILayout.Button("Spawn Enemy")) { SpawnPrefab(enemyPrefab); }
        if (GUILayout.Button("Spawn Platform")) { SpawnPrefab(platformPrefab); }
        EditorGUILayout.EndHorizontal();
    }

    private void SpawnPrefab(GameObject prefab)
    {
        if (prefab != null)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                Vector3 finalPosition = spawnPosition;

                if (randomizePosition)
                {
                    finalPosition += new Vector3(
                        Random.Range(-randomRange.x, randomRange.x),
                        Random.Range(-randomRange.y, randomRange.y),
                        Random.Range(-randomRange.z, randomRange.z)
                    );
                }

                // Instantiate first
                GameObject spawned = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
                Undo.RegisterCreatedObjectUndo(spawned, "Spawned " + prefab.name);
                spawned.transform.position = finalPosition;

                if (randomizeRotation)
                {
                    float rotX = Random.Range(minRotation.x, maxRotation.x);
                    float rotY = Random.Range(minRotation.y, maxRotation.y);
                    float rotZ = Random.Range(minRotation.z, maxRotation.z);
                    spawned.transform.rotation = Quaternion.Euler(rotX, rotY, rotZ);
                }

                if (randomizeScale)
                {
                    float scaleX = Random.Range(minScale.x, maxScale.x);
                    float scaleY = Random.Range(minScale.y, maxScale.y);
                    float scaleZ = Random.Range(minScale.z, maxScale.z);
                    spawned.transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
                }

                UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(spawned.scene);
                Debug.Log($" Spawned: {prefab.name} at {finalPosition}");
            }
        }
        else
        {
            Debug.LogWarning(" No prefab assigned.");
        }


    }
}
