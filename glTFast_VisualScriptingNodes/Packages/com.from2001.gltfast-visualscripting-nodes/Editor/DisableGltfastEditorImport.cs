using System.Linq;
using UnityEditor;

[InitializeOnLoad]
public class DefineSymbolsEditor
{
    static DefineSymbolsEditor()
    {
        string[] symbolsToAdd = new string[] { 
            "GLTFAST_EDITOR_IMPORT_OFF",                // Disable GLTFast import
            "UNIGLTF_DISABLE_DEFAULT_GLTF_IMPORTER",    // Disable importer in UniVRM
            "UNIGLTF_DISABLE_DEFAULT_GLB_IMPORTER",     // Disable importer in UniVRM
        };
        AddDefineSymbols(symbolsToAdd);
    }

    static void AddDefineSymbols(string[] symbols)
    {
        // Get the current list of defined symbols for the active build target
        string definesString = PlayerSettings.GetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup);
        var allDefines = definesString.Split(';').ToList();

        bool symbolsAdded = false;

        foreach (var symbol in symbols)
        {
            if (!allDefines.Contains(symbol))
            {
                allDefines.Add(symbol);
                symbolsAdded = true;
            }
        }

        // Only update the define symbols if new symbols were actually added
        if (symbolsAdded)
        {
            PlayerSettings.SetScriptingDefineSymbolsForGroup(EditorUserBuildSettings.selectedBuildTargetGroup, string.Join(";", allDefines.ToArray()));
        }
    }
}
