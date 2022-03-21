using UnityEngine;

public class Game
{
    public void Log(object log)
    {
        Debug.Log("Scripting Log: " + log.ToString());
    }

    public void ConsoleDisplay(object value)
    {
        ConsoleEvents.Display.Invoke(value.ToString());
    }
}