# Unity-Lua-Fennel-Starter
A Unity project to get started with Lua and Fennel

### Why
I integrated Lua (and then Fennel) into my current project. This became a really powerful tool in my game and I knew I was going to want it for game jams. Makes sense to share what I learned more broadly, I hope this helps you.

### Why not a package?
There are some assumptions/nuances that would make me feel bad if you used this as a package in it's current state. Read on for more information

### How to use
1) Clone/Download
2) Open [UnityProject/](UnityProject/) in Unity
3) Open the SampleScene in Unity
4) Play the game and press backtick <code>`</code> to open the console (backtick or escape to close)
5) Enter some fennel code (default)
6) Change to lua with `(lang)` (or the default in `NLuaController`)

![Screenshot in game](Screenshots/InGameConsole.png)

### Nuance 1) It's not using the fennel repl
yep, I didn't want to make that assumption for you. If you switch to lua it won't behave like one either, and this project focuses on working for both. You'll note I save to the global state an `_1` you can access the previous expressions results from. You can also set things to `_G` if you want to keep it between lines otherwise

### Nuance 2) It copies the lua/fnl files to Application.persistentDataPath/scripting/lib
yep, if you can figure out how to load lua/fennel modules from a string this can be removed, and instead load the modules from Unity TextAsset via NLua's CLR string exposure.
