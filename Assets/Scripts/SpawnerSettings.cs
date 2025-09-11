using UnityEngine;

[CreateAssetMenu(fileName = "SpawnerSettings", menuName = "Level Spawner/Spawner Settings", order = 1)]
public class SpawnerSettings : ScriptableObject
{
    [Header("Prefabs")]
    public GameObject coinPrefab;
    public GameObject enemyPrefab;
    public GameObject platformPrefab;

    [Header("Basic Spawn Settings")]
    public Vector3 spawnPosition = Vector3.zero;
    public int spawnCount = 1;

    [Header("Randomization Settings")]
    public bool randomizePosition = false;
    public Vector3 randomRange = Vector3.zero;

    public bool randomizeRotation = false;
    public Vector3 minRotation = Vector3.zero;
    public Vector3 maxRotation = Vector3.zero;

    public bool randomizeScale = false;
    public Vector3 minScale = Vector3.one;
    public Vector3 maxScale = Vector3.one;

    [Header("Grid Settings")]
    public bool useGrid = false;
    public int gridRows = 1;
    public int gridColumns = 1;
    public Vector2 gridSpacing = Vector2.one;
    public GridPlane gridPlane = GridPlane.XZ; // <- reuse your enum from EditorWindow
}

// NOTE: If GridPlane enum only exists inside LevelSpawnerWindow right now,
// move it into this file (outside the class) so both can use it:
public enum GridPlane { XZ, XY }
