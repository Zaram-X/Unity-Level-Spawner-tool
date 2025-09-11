using UnityEngine;
using UnityEditor;

public class LevelSpawnerWindow : EditorWindow
{
    // preset selection(ScriptableObject)
    private SpawnerSettings selectedPreset = null;
    private Transform spawnParent;

    // Grid Spawning
    private bool useGridSpawning = false;
    private int gridRows = 1;
    private int gridColumns = 1;
    private Vector2 gridSpacing = new Vector2(2f, 2f);
    // private enum GridPlane { XZ, XY }
    private GridPlane gridPlane = GridPlane.XZ;


    private bool showSpawnSettings = true;
    private bool showOptions = true;
    private bool showRandomization = true;
    private bool showPrefabs = true;
    private bool showGridSettings = true;

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
        // GUILayout.Label("Prefabs", EditorStyles.boldLabel);
        showPrefabs = EditorGUILayout.Foldout(showPrefabs, "Prefabs", true);
        if (showPrefabs)
        {
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
        }

        //GUILayout.Space(10);
        EditorGUILayout.Separator();

        // === Position Section ===
        // GUILayout.Label("Spawn Settings", EditorStyles.boldLabel);
        showSpawnSettings = EditorGUILayout.Foldout(showSpawnSettings, "Spawn Settings", true);
        if (showSpawnSettings)
        {
            EditorGUILayout.BeginVertical("box");
            spawnPosition = EditorGUILayout.Vector3Field(
                 new GUIContent("Spawn Position", "The base position where prefabs will be spawned"),
                 spawnPosition);

            if (GUILayout.Button(new GUIContent("Use Selected Object Position", "Set spawn position from selected object in the scene")))
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
        }
        //GUILayout.Space(10);
        EditorGUILayout.Separator();

        // === Options Section ===
        //GUILayout.Label("Options", EditorStyles.boldLabel);
        showOptions = EditorGUILayout.Foldout(showOptions, "Options", true);
        if (showOptions)
        {
            EditorGUILayout.BeginVertical("box");
            randomizePosition = EditorGUILayout.Toggle(
                new GUIContent("Randomize Position", "Enable random spawn positions within a defined range"),
                randomizePosition);

            if (randomizePosition)
            {
                randomRange = EditorGUILayout.Vector3Field(
                    new GUIContent("Random Range", "Range for random position offsets (X/Y/Z)"),
                    randomRange);
            }

            //Disable spawn count when grid mode is active
            using (new EditorGUI.DisabledScope(useGridSpawning))
            {
                spawnCount = EditorGUILayout.IntField(
             new GUIContent("Spawn Count", "How many prefabs to be spawn at once"), spawnCount);
                spawnCount = Mathf.Max(1, spawnCount); // Prevent negative/zero
            }
            EditorGUILayout.EndVertical();
        }

        //GUILayout.Space(10);
        EditorGUILayout.Separator();

        // === Randomization settings ===
        showRandomization = EditorGUILayout.Foldout(showRandomization, "Randomization Settings", true);
        if (showRandomization)
        {
            EditorGUILayout.BeginVertical("box");


            // Rotation
            randomizeRotation = EditorGUILayout.Toggle(
                new GUIContent("Randomize Rotation", "Apply random rotation within a given range"),
                randomizeRotation);

            if (randomizeRotation)
            {
                minRotation = EditorGUILayout.Vector3Field(new GUIContent("Min Rotation", "Minimum rotation (X/Y/Z)"), minRotation);
                maxRotation = EditorGUILayout.Vector3Field(new GUIContent("Max Rotation", "Maximum rotation (X/Y/Z)"), maxRotation);
            }

            // Scale
            randomizeScale = EditorGUILayout.Toggle(
                new GUIContent("Randomize Scale", "Apply random scale within a given range"),
                randomizeScale);

            if (randomizeScale)
            {
                minScale = EditorGUILayout.Vector3Field(new GUIContent("Min Scale", "Minimum scale (X/Y/Z)"), minScale);
                maxScale = EditorGUILayout.Vector3Field(new GUIContent("Max Scale", "Maximum scale (X/Y/Z)"), maxScale);
            }

            EditorGUILayout.EndVertical();
        }
        // GUILayout.Space(10);
        EditorGUILayout.Separator();

        //Grid Settings
        showGridSettings = EditorGUILayout.Foldout(showGridSettings, "Grid Settings", true);
        if (showGridSettings)
        {
            EditorGUILayout.BeginVertical("box");

            useGridSpawning = EditorGUILayout.Toggle(
        new GUIContent("Enable Grid Spawning", "Toggle between single and grid spawning"),
        useGridSpawning);

            gridRows = EditorGUILayout.IntField(new GUIContent("Rows", "Number of rows"), gridRows);
            gridColumns = EditorGUILayout.IntField(new GUIContent("Columns", "Number of columns"), gridColumns);
            gridSpacing = EditorGUILayout.Vector2Field(new GUIContent("Spacing", "Distance between objects"), gridSpacing);
            gridPlane = (GridPlane)EditorGUILayout.EnumPopup(new GUIContent("Grid Plane", "XZ = floor, XY = wall"), gridPlane);

            gridRows = Mathf.Max(1, gridRows);
            gridColumns = Mathf.Max(1, gridColumns);

            EditorGUILayout.EndVertical();
        }
        EditorGUILayout.Separator();

        // Presets UI
        GUILayout.Label("Presets", EditorStyles.boldLabel);

        selectedPreset = (SpawnerSettings)EditorGUILayout.ObjectField(
            new GUIContent("Preset", "Select a SpawnerSettings asset to load/save"),
            selectedPreset, typeof(SpawnerSettings), false);

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button(new GUIContent("Save New Preset...", "Create a new SpawnerSettings asset from current values")))
        {
            SaveNewPreset();
        }

        using (new EditorGUI.DisabledScope(selectedPreset == null))
        {
            if (GUILayout.Button(new GUIContent("Save to Selected", "Overwrite the selected preset with current values")))
            {
                SaveToSelectedPreset();
            }

            if (GUILayout.Button(new GUIContent("Load Preset", "Load values from selected preset into the tool")))
            {
                LoadSelectedPreset();
            }
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Separator();


        // === Spawn Buttons === 
        GUILayout.Label("Spawn Prefabs", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();

        using (new EditorGUI.DisabledScope(coinPrefab == null))
        {
            if (GUILayout.Button("Spawn Coin", GUILayout.Width(120), GUILayout.Height(25)))
                SpawnPrefab(coinPrefab);
        }


        using (new EditorGUI.DisabledScope(coinPrefab == null))
        {
            if (GUILayout.Button("Spawn Enemy", GUILayout.Width(120), GUILayout.Height(25)))
                SpawnPrefab(enemyPrefab);
        }

        using (new EditorGUI.DisabledScope(coinPrefab == null))
        {
            if (GUILayout.Button("Spawn Platform", GUILayout.Width(120), GUILayout.Height(25)))
                SpawnPrefab(platformPrefab);
        }
        
        EditorGUILayout.EndHorizontal();
    }

    private void SpawnPrefab(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogWarning("No prefab assigned.");
            return;
        }

        if (useGridSpawning)
        {
            for (int row = 0; row < gridRows; row++)
            {
                for (int col = 0; col < gridColumns; col++)
                {
                    Vector3 finalPosition = spawnPosition;

                    if (gridPlane == GridPlane.XZ)
                        finalPosition += new Vector3(col * gridSpacing.x, 0f, row * gridSpacing.y);
                    else
                        finalPosition += new Vector3(col * gridSpacing.x, row * gridSpacing.y, 0f);

                    if (randomizePosition)
                    {
                        finalPosition += new Vector3(
                            Random.Range(-randomRange.x, randomRange.x),
                            Random.Range(-randomRange.y, randomRange.y),
                            Random.Range(-randomRange.z, randomRange.z)
                        );
                    }

                    SpawnSingle(prefab, finalPosition);
                }
            }
        }
        else
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

                SpawnSingle(prefab, finalPosition);
            }
        }
    }


    private void SpawnSingle(GameObject prefab, Vector3 position)
    {
        GameObject spawned = (GameObject)PrefabUtility.InstantiatePrefab(prefab);
        Undo.RegisterCreatedObjectUndo(spawned, "Spawned " + prefab.name);
        spawned.transform.position = position;

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
        Debug.Log($" Spawned: {prefab.name} at {position}");

        EnsureParentExists();
        Undo.SetTransformParent(spawned.transform, spawnParent, "Parent Spawned Object");
        Selection.activeGameObject = spawned;



    }

    private void EnsureParentExists()
    {
        if (spawnParent == null)
        {
            GameObject parent = GameObject.Find("SpawnedObjects");
            if (parent == null)
                parent = new GameObject("SpawnedObjects");
            spawnParent = parent.transform;
        }
    }


    // Preset save/load helpers

    private void SaveNewPreset()
    {
        string path = EditorUtility.SaveFilePanelInProject(
            "Save Spawner Preset",
            "SpawnerSettings",
            "asset",
            "Choose where to save the preset asset in your project");

        if (string.IsNullOrEmpty(path))
            return;

        SpawnerSettings asset = ScriptableObject.CreateInstance<SpawnerSettings>();
        CopyWindowToSettings(asset);
        AssetDatabase.CreateAsset(asset, path);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
        selectedPreset = asset; // auto-select the newly created preset
        Debug.Log("Spawner preset created: " + path);
    }

    private void SaveToSelectedPreset()
    {
        if (selectedPreset == null) return;

        CopyWindowToSettings(selectedPreset);
        EditorUtility.SetDirty(selectedPreset);
        AssetDatabase.SaveAssets();
        Debug.Log("Spawner preset saved: " + AssetDatabase.GetAssetPath(selectedPreset));
    }

    private void LoadSelectedPreset()
    {
        if (selectedPreset == null) return;

        CopySettingsToWindow(selectedPreset);
        Repaint();
        Debug.Log("Spawner preset loaded: " + AssetDatabase.GetAssetPath(selectedPreset));
    }

    private void CopyWindowToSettings(SpawnerSettings s)
    {
        // Prefabs
        s.coinPrefab = coinPrefab;
        s.enemyPrefab = enemyPrefab;
        s.platformPrefab = platformPrefab;

        // Basic
        s.spawnPosition = spawnPosition;
        s.spawnCount = spawnCount;

        // Random position
        s.randomizePosition = randomizePosition;
        s.randomRange = randomRange;

        // Rotation
        s.randomizeRotation = randomizeRotation;
        s.minRotation = minRotation;
        s.maxRotation = maxRotation;

        // Scale
        s.randomizeScale = randomizeScale;
        s.minScale = minScale;
        s.maxScale = maxScale;

        // Grid
        s.useGrid = useGridSpawning;
        s.gridRows = gridRows;
        s.gridColumns = gridColumns;
        s.gridSpacing = gridSpacing;
        s.gridPlane = gridPlane;
    }

    private void CopySettingsToWindow(SpawnerSettings s)
    {
        // Prefabs
        coinPrefab = s.coinPrefab;
        enemyPrefab = s.enemyPrefab;
        platformPrefab = s.platformPrefab;

        // Basic
        spawnPosition = s.spawnPosition;
        spawnCount = Mathf.Max(1, s.spawnCount);

        // Random position
        randomizePosition = s.randomizePosition;
        randomRange = s.randomRange;

        // Rotation
        randomizeRotation = s.randomizeRotation;
        minRotation = s.minRotation;
        maxRotation = s.maxRotation;

        // Scale
        randomizeScale = s.randomizeScale;
        minScale = s.minScale;
        maxScale = s.maxScale;

        // Grid
        useGridSpawning = s.useGrid;
        gridRows = Mathf.Max(1, s.gridRows);
        gridColumns = Mathf.Max(1, s.gridColumns);
        gridSpacing = s.gridSpacing;
        gridPlane = s.gridPlane;
    }




}
