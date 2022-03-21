using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class ConsoleEvents
{
    public static UnityEvent<string> Eval = new UnityEvent<string>();
    public static UnityEvent<string> Display = new UnityEvent<string>();
}
