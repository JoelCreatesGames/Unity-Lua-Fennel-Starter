using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.AssetImporters;

[ScriptedImporter(1, "fnl")]
public class FennelScriptedImporter : ScriptedImporter
{
    [SerializeField] private TextAsset fennel;

    public override void OnImportAsset(AssetImportContext ctx)
    {
        fennel = new TextAsset(File.ReadAllText(ctx.assetPath));
        Texture2D icon = Resources.Load<Texture2D>("Icons/Fennel");
        ctx.AddObjectToAsset("Fennel Script", fennel, icon);
        ctx.SetMainObject(fennel);
    }

    [MenuItem("Assets/Create/Fennel Script", false, 80)]
    public static void Create()
    {
        string filename = "script.fnl";
        string content = "(fn my-fn [] (print :hello-world))\n\n{: my-fn}\n";
        ProjectWindowUtil.CreateAssetWithContent(filename, content);
    }
}