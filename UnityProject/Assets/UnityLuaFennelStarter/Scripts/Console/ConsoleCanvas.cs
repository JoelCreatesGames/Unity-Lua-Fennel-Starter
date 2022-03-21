using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsoleCanvas : MonoBehaviour
{
    // Common
    bool open;
    public CanvasGroup canvasGroup;
    // Input
    public InputField inputField;
    // History
    public Transform chatLineContainer;
    public GameObject chatLinePrefab;
    public ScrollRect scrollRect;
    Queue<ConsoleMessage> chatMessageQueue = new Queue<ConsoleMessage>();

    void Awake()
    {
        SetOpen(false);
    }

    private void OnEnable()
    {
        ConsoleEvents.Display.AddListener(AddSecondaryLine);
    }

    private void OnDisable()
    {
        ConsoleEvents.Display.RemoveListener(AddSecondaryLine);
    }

    void Update()
    {
        if (ShouldSendInput()) SendInput();
        else if (ShouldOpen()) SetOpen(true);
        else if (ShouldClose()) SetOpen(false);
    }

    void SetOpen(bool open)
    {
        this.open = open;
        canvasGroup.alpha = open ? 1 : 0;

        inputField.interactable = open;
        if (open) inputField.ActivateInputField();
        else inputField.DeactivateInputField();
    }

    bool ShouldSendInput()
    {
        return open &&
            (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) &&
            inputField.text.Length > 0;
    }

    bool ShouldOpen()
    {
        return !open &&
            Input.GetKeyDown(KeyCode.BackQuote);
    }

    bool ShouldClose()
    {
        return open &&
            (Input.GetKeyDown(KeyCode.BackQuote) ||
            Input.GetKeyDown(KeyCode.Escape) ||
            ((Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) && inputField.text.Length == 0));
    }

    void SendInput()
    {
        AddPrimaryLine(inputField.text);
        ConsoleEvents.Eval.Invoke(inputField.text);
        inputField.text = "";
        inputField.ActivateInputField();
    }

    // History
    void AddPrimaryLine(string line)
    {
        chatMessageQueue.Enqueue(new ConsoleMessage(ConsoleMessageType.Primary, "", line));
        StartCoroutine(HandleScrollToBottom());
    }

    void AddSecondaryLine(string line)
    {
        chatMessageQueue.Enqueue(new ConsoleMessage(ConsoleMessageType.Secondary, "", line));
        StartCoroutine(HandleScrollToBottom());
    }

    void InstantiateConsoleLine(ConsoleMessage message)
    {
        GameObject g = Instantiate(chatLinePrefab, transform.position, Quaternion.identity, chatLineContainer);
        ConsoleLine c = g.GetComponent<ConsoleLine>();
        c.Setup(message);
    }

    void UpdateScrollToBottom()
    {
        scrollRect.verticalNormalizedPosition = 0;
    }

    IEnumerator HandleScrollToBottom()
    {
        while (chatMessageQueue.Count > 0)
        {
            InstantiateConsoleLine(chatMessageQueue.Dequeue());
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();
            UpdateScrollToBottom();
        }
        yield return null;
    }
}
