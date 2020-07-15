using System;
using System.Collections;
using System.Collections.Generic;
using FullSerializer;
using Networking;
using Networking.Client;
using UnityEngine;
using UnityEngine.UI;
using Utils;

/// <summary>
/// This is the ChatBoxManager script, it contains functionality that is specific to the chatbox
/// </summary>
public class ChatBoxManager : MonoBehaviour
{
    [Header("Variables")]
    // Player username
    public string username;
    // Limit number of messages to show on the chat
    public int maxMessages = 25;

    [Header("References")]
    private GameObject chatPanel;
    private GameObject textObject;
    private InputField chatBox;

    [Header("Message type color")]
    public Color server;
    public Color playerMessage;
    public Color privateMessage;
    private bool sendNewMessage = false;
    private string sendNewText = "";

    [Header("Message list")]
    [SerializeField] List<Message> messageList = new List<Message>();

    // Start is called before the first frame update
    void Start()
    {
        chatPanel = UIManager.MyInstance.chatPanel;
        textObject = UIManager.MyInstance.textObject;
        chatBox = UIManager.MyInstance.chatBox;

        // Welcome message
        SendMessageToChat("Bienvenue dans Tower", Message.MessageType.server);
        StartCoroutine(WaitingForChatMessage());
        StartCoroutine(SendNewMessage());
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
    public IEnumerator SendNewMessage()
    {
        while (!sendNewMessage)
        {
            yield return new WaitForSeconds(0.5f);
        }
        SendMessageToChat(sendNewText, Message.MessageType.playerMessage);
        sendNewMessage = false;
        sendNewText = "";
        yield return SendNewMessage();
    }
    public IEnumerator WaitingForChatMessage()
    {
        while (NetworkingController.ConnectionStart == false)
        {
            yield return new WaitForSeconds(0.1f);
        }

        TowersWebSocket.wsChat.OnMessage += (sender, args) =>
        {
            if (args.Data.Contains("callbackChatMessage"))
            {
                fsSerializer serializer = new fsSerializer();
                fsData data;
                CallbackChatMessages callbackChatMessage = null; 
                try
                {
                    data = fsJsonParser.Parse(args.Data);
                    serializer.TryDeserialize(data, ref callbackChatMessage);
                    callbackChatMessage = Tools.Clone(callbackChatMessage);
                    sendNewText = callbackChatMessage.callbackChatMessage.sender + " : " +
                                  callbackChatMessage.callbackChatMessage.message;
                    sendNewMessage = true;
                    Debug.Log(callbackChatMessage.callbackChatMessage.sender + " : " + callbackChatMessage.callbackChatMessage.message);
                }
                catch (Exception e)
                {
                    Debug.Log("Can't read callback : " + e.Message);
                }
            }
        };
    }

    /// <summary>
    /// User want to send a message to the chatbox
    /// </summary>
    public void SendTheMessage()
    {
        // If the chatbox content isn't empty and the user press the return key
        if (!string.IsNullOrWhiteSpace(chatBox.text) && Input.GetKeyDown(KeyCode.Return))
        {
            // Check if it is a private message
            if (chatBox.text.Length > 3 && chatBox.text[0] == '/' && chatBox.text[1] == 'w' && chatBox.text[2] == ' ' && chatBox.text[3] != ' ')
            {
                string user = "";
                string message = "";
                bool space = false;
                bool privateMess = false;

                // Until the message is finished
                for (int i = 3; i < chatBox.text.Length; i++)
                {
                    // Check if we have a username
                    if (chatBox.text[i] != ' ' && space == false)
                    {
                        user += chatBox.text[i];
                    }

                    // Check if we have a space
                    else if (chatBox.text[i] == ' ' && i != 3 && space == false)
                    {
                        space = true;
                    }

                    // Check if we have a message
                    else if (space == true)
                    {
                        message += chatBox.text[i];
                        privateMess = true;
                    }
                }


                if (privateMess == true)
                {
                    // Check if we have a user with this username and if he is online
                    if (true)
                    {
                        // Send the user message as a private message
                        SendMessageToChat("À " + user + " : " + message, Message.MessageType.privateMessage);

                        // TODO : Have the recipient receive the message
                    }
                    else
                    {
                        // Send a warning message
                        SendMessageToChat("Le destinataire est introuvable.", Message.MessageType.server);
                    }
                }
                else
                {
                    TowersWebSocket.ChatSender("ALL", "GENERAL", NetworkingController.NickName, chatBox.text);
                }
            }

            // Check if it is a invite to a party
            else if (chatBox.text.Length > 8 && chatBox.text[0] == '/' && chatBox.text[1] == 'i' && chatBox.text[2] == 'n' && chatBox.text[3] == 'v' && chatBox.text[4] == 'i' && chatBox.text[5] == 't' && chatBox.text[6] == 'e' && chatBox.text[7] == ' ' && chatBox.text[8] != ' ')
            {
                string user = "";

                // Until the message is finished
                for (int i = 8; i < chatBox.text.Length; i++)
                {
                    // Check if we have a username
                    if (chatBox.text[i] != ' ')
                    {
                        user += chatBox.text[i];
                    }

                    // Check if we have a space
                    else if (chatBox.text[i] == ' ' && i != 8)
                    {
                        break;
                    }
                }

                // Check if we have a user with this username and if he is online
                if (true)
                {
                    // Send the invitation to the party
                    SendMessageToChat("Une invitation a été envoyé à " + user + ".", Message.MessageType.server);

                    // TODO : Have the recipient receive the invitation
                }
                else
                {
                    // Send a warning message
                    SendMessageToChat("Le destinataire est introuvable.", Message.MessageType.server);
                }        
            }

            else
            {
                // Send the user message as a general message
                TowersWebSocket.ChatSender("ALL", "GENERAL", NetworkingController.NickName, chatBox.text);
            }

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