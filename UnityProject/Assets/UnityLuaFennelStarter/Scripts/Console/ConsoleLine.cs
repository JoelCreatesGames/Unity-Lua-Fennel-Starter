using UnityEngine;
using UnityEngine.UI;

public enum ConsoleMessageType
{
    Primary,
    Secondary
}

public class ConsoleMessage
{
    public ConsoleMessageType type;
    public string timestamp;
    public string message;

    public ConsoleMessage(ConsoleMessageType type, string timestamp, string messsage)
    {
        this.type = type;
        this.timestamp = timestamp;
        this.message = messsage;
    }
}

public class ConsoleLine : MonoBehaviour
{
    public Text text;
    public Color primaryColor;
    public Color secondaryColor;

    public void Setup(ConsoleMessage message)
    {
        switch (message.type)
        {
            case ConsoleMessageType.Primary:
                //text.text = message.timestamp + ": " + message.message;
                text.text = message.message;
                text.color = primaryColor;
                break;
            case ConsoleMessageType.Secondary:
                //text.text = message.timestamp + ": " + message.message;
                text.text = message.message;
                text.color = secondaryColor;
                break;
        }
    }
}
