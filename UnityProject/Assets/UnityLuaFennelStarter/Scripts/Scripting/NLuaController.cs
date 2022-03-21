using UnityEngine;
using NLua;
using System.IO;

public enum ScriptingLanguage
{
    Disabled,
    Lua,
    Fennel
}

public class NLuaController : MonoBehaviour
{
    Lua lua = new Lua();
    Bridge bridge;

    public TextAsset fennelLibrary;
    public TextAsset[] fennelScripts;

    public ScriptingLanguage defaultLanguage;

    void Awake()
    {
        bridge = new Bridge(lua);
        EnsureFilesExistInPath();
        ConnectLuaToCLR();
        SetupPathsAndScripts();
        SetupDefaultScriptingLanguage();
    }

    void OnEnable()
    {
        ConsoleEvents.Eval.AddListener(Eval);
    }

    void OnDisable()
    {
        ConsoleEvents.Eval.RemoveListener(Eval);
    }

    void EnsureFilesExistInPath()
    {
        if (Directory.Exists(Application.persistentDataPath + "/scripting/lib")) Directory.Delete(Application.persistentDataPath + "/scripting/lib", true);
        if (!Directory.Exists(Application.persistentDataPath + "/scripting")) Directory.CreateDirectory(Application.persistentDataPath + "/scripting/lib");
        Directory.CreateDirectory(Application.persistentDataPath + "/scripting/lib");

        // fennel.lua Library
        File.WriteAllText(Application.persistentDataPath + "/scripting/lib/" + fennelLibrary.name + ".lua", fennelLibrary.text);

        // .fnl scripts
        for (int i = 0; i < fennelScripts.Length; i++)
        {
            File.WriteAllText(Application.persistentDataPath + "/scripting/lib/" + fennelScripts[i].name + ".fnl", fennelScripts[i].text);
        }
    }

    void ConnectLuaToCLR(bool preventImport = true)
    {
        lua.LoadCLRPackage();
        if (preventImport) lua.DoString("import = function () end");
    }

    void SetupPathsAndScripts()
    {
        // Append package.path with where we will store our .lua/.fnl files
        lua.DoString("package.path = package.path .. \";" + Application.persistentDataPath + "/scripting/lib/?/?.lua;" + Application.persistentDataPath + "/scripting/lib/?.lua\"");

        // Require fennel, fix path, and allow us to use require on .fnl files
        // TODO safety via try/catch
        lua.DoString("fennel = require(\"fennel\")");
        lua.DoString("table.insert(package.loaders or package.searchers, fennel.searcher)");
        lua.DoString("fennel.path = fennel.path .. \";" + Application.persistentDataPath + "/scripting/lib/?/?.fnl;" + Application.persistentDataPath + "/scripting/lib/?.fnl\"");

        // Load fennel scripts through unity placed files
        // TODO safety via try/catch
        for (int i = 0; i < fennelScripts.Length; i++)
        {
            Debug.Log("Requiring fennel package:" + fennelScripts[i].name);
            lua.DoString(fennelScripts[i].name + " = require(\"" + fennelScripts[i].name + "\")");
        }
    }

    void SetupDefaultScriptingLanguage()
    {
        switch (defaultLanguage)
        {
            case ScriptingLanguage.Lua:
                lua["usefennel"] = false;
                break;
            case ScriptingLanguage.Fennel:
                lua["usefennel"] = true;
                break;
            case ScriptingLanguage.Disabled:
            default:
                // TODO
                break;
        }
    }

    void Eval(string chunk)
    {
        try
        {
            bool? isFennel = (bool?)lua["usefennel"];
            if (isFennel.HasValue && isFennel.Value) lua.DoFnlString(chunk, "_1");
            else lua.DoString("_1 = " + chunk);
            ConsoleEvents.Display.Invoke("=> " + lua["_1"]);
        }
        catch (System.Exception e)
        {
            ConsoleEvents.Display.Invoke("Error " + e);
        }
    }
}