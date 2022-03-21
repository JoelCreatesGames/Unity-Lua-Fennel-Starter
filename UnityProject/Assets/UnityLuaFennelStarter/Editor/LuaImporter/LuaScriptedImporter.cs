using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;

[ScriptedImporter(1, "lua")]
public class LuaScriptedImporter : ScriptedImporter
{
    [SerializeField] private TextAsset lua;

    public override void OnImportAsset(AssetImportContext ctx)
    {
        lua = new TextAsset(File.ReadAllText(ctx.assetPath));
        Texture2D icon = Resources.Load<Texture2D>("Icons/Lua");
        ctx.AddObjectToAsset("Lua Script", lua, icon);
        ctx.SetMainObject(lua);
    }

    [MenuItem("Assets/Create/Lua Script", false, 80)]
    public static void Create()
    {
        string filename = "myluamodule.lua";
        string content = "print(\"Hello World from Lua!\")\n";
        ProjectWindowUtil.CreateAssetWithContent(filename, content);
    }
}