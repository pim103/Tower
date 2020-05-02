using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is the ChatBoxManager script, it contains functionality that is specific to the chatbox
/// </summary>
public class ChatBoxManager : MonoBehaviour
{
    // Player username
    public string username;

    // Limit number of messages to show on the chat
    public int maxMessages = 25;

    // References
    [Header("References")]
    public GameObject chatPanel;
    public GameObject textObject;
    public InputField chatBox;

    // Message type color
    [Header("Message type color")]
    public Color server;
    public Color playerMessage;
    public Color privateMessage;

    // Message list
    [SerializeField] List<Message> messageList = new List<Message>();

    // Start is called before the first frame update
    void Start()
    {
        // Welcome message
        SendMessageToChat("Bienvenue dans Tower", Message.MessageType.server);
    }

    // Update is called once per frame
    void Update()
    {
        // If the chatbox isn't focused and the user press the return key
        if (!chatBox.isFocused && Input.GetKeyDown(KeyCode.Return))
        {
            // Activate the chatbox input field
            chatBox.ActivateInputField();
        }
    }

    /// <summary>
    /// User want to send a message to the chatbox
    /// </summary>
    public void SendTheMessage()
    {
        // If the chatbox content isn't empty and the user press the return key
        if (!string.IsNullOrWhiteSpace(chatBox.text) && Input.GetKeyDown(KeyCode.Return))
        {
            // Send the user message
            SendMessageToChat(username + ": " + chatBox.text, Message.MessageType.playerMessage);

            // Clean the chatbox input field
            chatBox.text = "";
        }
    }

    /// <summary>
    /// Send the message to the chatbox
    /// </summary>
    public void SendMessageToChat(string text, Message.MessageType messageType)
    {
        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }

        Message newMessage = new Message();

        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newText.GetComponent<Text>();

        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = MessageTypeColor(messageType);

        newMessage.messageType = messageType;

        messageList.Add(newMessage);
    }

    /// <summary>
    /// Set the color to the message
    /// </summary>
    Color MessageTypeColor(Message.MessageType messageType)
    {
        Color color = server;

        switch (messageType)
        {
            case Message.MessageType.playerMessage:
                color = playerMessage;
                break;
            case Message.MessageType.privateMessage:
                color = privateMessage;
                break;
        }

        return color;
    }
}

// Content of a message
[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
    public MessageType messageType;

    public enum MessageType
    {
        server,
        playerMessage,
        privateMessage
    }
}