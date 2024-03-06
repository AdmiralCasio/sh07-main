using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class Builder
{
    private static void BuildAndroid()
    {
        // Set architecture in BuildSettings
        bool success = EditorUserBuildSettings.SwitchActiveBuildTarget(
            BuildTargetGroup.Android,
            BuildTarget.Android
        );

        // Setup build options (e.g. scenes, build output location)
        var options = new BuildPlayerOptions
        {
            // Change to scenes from your project
            scenes = new[] { "Assets/Scenes/AppScene.unity" },
            // Change to location the output should go
            locationPathName = "./AndroidBuilds/Build.apk",
            options = BuildOptions.CleanBuildCache | BuildOptions.StrictMode,
            target = BuildTarget.Android
        };

        var report = BuildPipeline.BuildPlayer(options);

        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"Build successful - Build written to {options.locationPathName}");
        }
        else if (report.summary.result == BuildResult.Failed)
        {
            Debug.LogError($"Build failed.");
        }
    }

    private static void BuildIOS()
    {
        bool success = EditorUserBuildSettings.SwitchActiveBuildTarget(
            BuildTargetGroup.iOS,
            BuildTarget.iOS
        );

        var options = new BuildPlayerOptions
        {
            // Change to scenes from your project
            scenes = new[] { "Assets/Scenes/AppScene.unity" },
            // Change to location the output should go
            locationPathName = "./IOSBuilds/Build.xcodeproj",
            options = BuildOptions.CleanBuildCache | BuildOptions.StrictMode,
            target = BuildTarget.iOS
        };

        var report = BuildPipeline.BuildPlayer(options);

        if (report.summary.result == BuildResult.Succeeded)
        {
            Debug.Log($"Build successful - Build written to {options.locationPathName}");
        }
        else if (report.summary.result == BuildResult.Failed)
        {
            Debug.LogError($"Build failed.");
        }
    }

    // This function will be called from the build process
    public static void Build()
    {
        BuildAndroid();
        BuildIOS();
    }
}
