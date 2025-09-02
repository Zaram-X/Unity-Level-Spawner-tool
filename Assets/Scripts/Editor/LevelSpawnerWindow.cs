using UnityEngine;
using UnityEditor;

public class LevelSpawnerWindow : EditorWindow
{
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
        GUILayout.Label("This is your custom Editor window");
    }
}
