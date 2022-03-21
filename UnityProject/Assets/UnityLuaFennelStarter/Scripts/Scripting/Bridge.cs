using NLua;

public class Bridge
{
    public Lua lua;

    public Game game;

    public Bridge(Lua lua)
    {
        this.lua = lua;
        InitModules();
        SetupLuaInstance();
    }

    void InitModules()
    {
        game = new Game();
    }

    void SetupLuaInstance()
    {
        lua["Game"] = game;
    }
}
