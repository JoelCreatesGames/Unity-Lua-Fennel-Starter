using NLua;

public static class LuaExtensions
{
    public static void DoFnlString(this Lua lua, string chunk, string saveTo = null)
    {
        string prefix = string.IsNullOrEmpty(saveTo) ? "" : saveTo + " = ";
        chunk = chunk.Replace("\"", "\\\"")
                     .Replace(System.Environment.NewLine, "");
        lua.DoString(prefix + "fennel.eval(\"" + chunk + "\")");
    }
}
