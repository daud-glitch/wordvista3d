#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
public static class WordVistaAutoSetup
{
    static WordVistaAutoSetup()
    {
        EditorApplication.delayCall += ConfigureScenes;
    }

    [MenuItem("WordVista/Configure All Scenes in Build")]
    public static void ConfigureScenes()
    {
        string[] scenes = new string[]
        {
            "Assets/Scenes/Bootstrap.unity",
            "Assets/Scenes/MainMenu.unity",
            "Assets/Scenes/WorldMap.unity",
            "Assets/Scenes/Gameplay.unity",
            "Assets/Scenes/DailyPuzzle.unity"
        };

        var buildScenes = new EditorBuildSettingsScene[scenes.Length];
        for (int i = 0; i < scenes.Length; i++)
        {
            buildScenes[i] = new EditorBuildSettingsScene(scenes[i], true);
        }

        EditorBuildSettings.scenes = buildScenes;
        AssetDatabase.SaveAssets();
        Debug.Log("<color=green>[WordVista] Successfully registered all 5 scenes in Build Settings!</color>");
    }

    [MenuItem("WordVista/Build Windows Standalone (.exe)")]
    public static void BuildWindowsExecutable()
    {
        ConfigureScenes();
        string buildPath = "Builds/Windows/WordVista3D.exe";
        Directory.CreateDirectory("Builds/Windows");

        var report = BuildPipeline.BuildPlayer(
            EditorBuildSettings.scenes,
            buildPath,
            BuildTarget.StandaloneWindows64,
            BuildOptions.None
        );

        Debug.Log($"[WordVista] Windows Build result: {report.summary.result}");
    }
}
#endif
