using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public Color playerMessage;
    public Color info;

    // Message list
    [SerializeField] List<Message> messageList = new List<Message>();

    // Start is called before the first frame update
    void Start()
    {
        // Welcome message
        SendMessageToChat("Bienvenue dans Tower", Message.MessageType.info);
    }

    // Update is called once per frame
    void Update()
    {
        if (!string.IsNullOrWhiteSpace(chatBox.text))
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                // Send the user message
                SendMessageToChat(username + ": " + chatBox.text, Message.MessageType.playerMessage);
                // Clean input field
                chatBox.text = "";
            }
        }
        else
        {
            if (!chatBox.isFocused && Input.GetKeyDown(KeyCode.Return))
            {
                // Clean input field
                chatBox.text = "";
                chatBox.ActivateInputField();
            }
        }

        if (!chatBox.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SendMessageToChat("You pressed the space bar", Message.MessageType.info);
            }
        }
    }

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

        messageList.Add(newMessage);
    }

    Color MessageTypeColor(Message.MessageType messageType)
    {
        Color color = info;

        switch (messageType)
        {
            case Message.MessageType.playerMessage:
                color = playerMessage;
                break;
        }

        return color;
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
    public MessageType messageType;

    public enum MessageType
    {
        playerMessage,
        info
    }
}