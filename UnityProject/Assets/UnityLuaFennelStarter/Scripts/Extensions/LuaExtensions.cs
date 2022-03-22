using NLua;

public static class LuaExtensions
{
    public static void DoFnlString(this Lua lua, string chunk, string saveTo = null)
    {
        string prefix = string.IsNullOrEmpty(saveTo) ? "" : saveTo + " = ";
        chunk = chunk.Replace("\"", "\\\"")
                     .Replace(System.Environment.NewLine, "");
        UnityEngine.Debug.Log("here is what we are going to eval:\n" + prefix + "fennel.eval(\"" + chunk + "\")");
        lua.DoString(prefix + "fennel.eval(\"" + chunk + "\")");
    }
}
