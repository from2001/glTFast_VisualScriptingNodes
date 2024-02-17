using System.Linq;
using UnityEditor;

[InitializeOnLoad]
public class DefineSymbolsEditor
{
    static DefineSymbolsEditor()
    {
        AddDefineSymbols();
    }

    static void AddDefineSymbols()
    {
        string symbol = "GLTFAST_EDITOR_IMPORT_OFF";
        string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        var allDefines = definesString.Split(';').ToList();
        if (!allDefines.Contains(symbol))
        {
            allDefines.Add(symbol);
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", allDefines.ToArray()));
        }
    }
}
