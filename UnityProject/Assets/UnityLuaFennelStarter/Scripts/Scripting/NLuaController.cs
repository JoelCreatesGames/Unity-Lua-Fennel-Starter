using UnityEngine;
using NLua;

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

    public TextAsset luaSearcher;
    public TextAsset fennelSearcher;
    public TextAsset fennelLibrary;
    public TextAsset[] luaScripts;
    public TextAsset[] fennelScripts;

    public ScriptingLanguage defaultLanguage;

    #region Unity Messages
    void Awake()
    {
        bridge = new Bridge(lua);
        ConnectLuaToCLR();
        LinkModulesToLua();
        LoadLua();
        LoadFennel();
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
    #endregion

    #region Lua and Fennel Setup
    void ConnectLuaToCLR(bool preventImport = true)
    {
        lua.LoadCLRPackage();
        if (preventImport) lua.DoString("import = function () end");
    }

    void LinkModulesToLua()
    {
        // Fennel library
        lua["__gamemodules_lua__fennel"] = fennelLibrary;

        // Lua scripts
        for (int i = 0; i < luaScripts.Length; i++)
        {
            lua["__gamemodules_lua__" + luaScripts[i].name] = luaScripts[i];
        }

        // Fennel scripts
        for (int i = 0; i < fennelScripts.Length; i++)
        {
            lua["__gamemodules_fnl__" + fennelScripts[i].name] = fennelScripts[i];
        }
    }

    void LoadLua()
    {
        lua.DoString(luaSearcher.text);
        for (int i = 0; i < luaScripts.Length; i++)
        {
            Debug.Log("Requiring lua package:" + luaScripts[i].name);
            try
            {
                lua.DoString(luaScripts[i].name + " = require(\"" + luaScripts[i].name + "\")");
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
        }
    }

    void LoadFennel()
    {
        lua.DoString("fennel = require(\"fennel\")");
        // this is needed if you plan to load fennel from the filesystem, example mods
        // lua.DoString("table.insert(package.loaders or package.searchers, fennel.searcher)");
        lua.DoString(fennelSearcher.text);
        for (int i = 0; i < fennelScripts.Length; i++)
        {
            Debug.Log("Requiring fennel package:" + fennelScripts[i].name);
            try
            {
                lua.DoString(fennelScripts[i].name + " = require(\"" + fennelScripts[i].name + "\")");
            }
            catch (System.Exception e)
            {
                Debug.LogError(e);
            }
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
    #endregion

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