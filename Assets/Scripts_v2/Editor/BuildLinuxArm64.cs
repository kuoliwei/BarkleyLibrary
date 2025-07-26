using System.IO;
using UnityEditor;
using UnityEditor.Build.Reporting;

public static class BuildLinuxArm64
{
    //[MenuItem("Build/Build Linux ARM64")]
    //public static void Build()
    //{
    //    // 強制設定 Architecture
    //    PlayerSettings.SetArchitecture(BuildTargetGroup.Standalone, 2); // 0: x86, 1: x86_64, 2: ARM64
    //    PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingImplementation.IL2CPP);

    //    // 輸出路徑與場景
    //    var buildPlayerOptions = new BuildPlayerOptions
    //    {
    //        scenes = new[] { "Assets/Scenes_v2/BarkleyLibrary_v2.unity" }, // 請換成你的主場景
    //        locationPathName = "C:/Users/liweikuo/Documents/小貓書房linux_arm64/BarkleyLibrary_ARM64",
    //        target = BuildTarget.StandaloneLinux64,
    //        options = BuildOptions.None
    //    };

    //    var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
    //    if (report.summary.result == BuildResult.Succeeded)
    //        UnityEngine.Debug.Log("Build 成功（ARM64）！");
    //    else
    //        UnityEngine.Debug.LogError("Build 失敗！");
    //}

    [MenuItem("Build/Build Linux ARM64 (Safe Mode)")]
    public static void Build()
    {
        string folderPath = "C:\\Users\\liweikuo\\Documents\\小貓書房linux_arm64";
        string exePath = Path.Combine(folderPath, "BarkleyLibrary_ARM64");

        // 檢查與建立輸出資料夾
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        // 強制設定架構與 backend
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone, ScriptingImplementation.IL2CPP);
        PlayerSettings.SetArchitecture(BuildTargetGroup.Standalone, 2); // 0:x86, 1:x86_64, 2:ARM64

        // 驗證是否設定成功
        int arch = PlayerSettings.GetArchitecture(BuildTargetGroup.Standalone);
        if (arch != 2)
        {
            UnityEngine.Debug.LogError("架構設定失敗，目前不是 ARM64！");
            return;
        }

        var buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = new[] { "Assets/Scenes_v2/BarkleyLibrary_v2.unity" },
            locationPathName = exePath,
            target = BuildTarget.StandaloneLinux64,
            options = BuildOptions.None
        };

        var report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        if (report.summary.result == BuildResult.Succeeded)
            UnityEngine.Debug.Log("Build 成功（應為 ARM64）！");
        else
            UnityEngine.Debug.LogError("Build 失敗！");
    }
}
