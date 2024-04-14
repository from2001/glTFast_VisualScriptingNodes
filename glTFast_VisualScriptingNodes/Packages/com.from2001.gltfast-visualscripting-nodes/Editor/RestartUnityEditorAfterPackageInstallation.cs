using System.Linq;
using UnityEditor;
using UnityEditor.PackageManager;

/// <summary>
/// Restart Unity Editor after package installation
/// </summary>
class RestartUnityEditorAfterPackageInstallation
{
    static readonly string PackageName = "com.from2001.gltfast-visualscripting-nodes";
    
    [InitializeOnLoadMethod]
    static void CheckNeedRestart()
    {
        string packageVersion = GetPackageVersion(PackageName);

        if (EditorPrefs.GetString("VersionOf_" + PackageName, "") != packageVersion)
        {
            EditorPrefs.SetString("VersionOf_" + PackageName, packageVersion);
            if (EditorUtility.DisplayDialog("Restart Unity",
                "You need to restart Unity to apply the new changes. Restart now?",
                "Restart", "Later"))
            {
                // Restart Unity Editor
                EditorApplication.OpenProject(System.Environment.CurrentDirectory);
            }
            else
            {
                // Inform the user to restart Unity manually
                EditorUtility.DisplayDialog("Manual Restart Required",
                    "Please close and reopen Unity to complete the update.",
                    "OK");
            }
        }
    }

    /// <summary>
    /// Get the version of the package
    /// </summary>
    /// <param name="packageName"></param>
    /// <returns></returns>
    private static string GetPackageVersion(string packageName)
    {
        var request = Client.List(true, true);
        while (!request.IsCompleted) { }
        return request.Result.FirstOrDefault(package => package.name == packageName)?.version;
    }
}