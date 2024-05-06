using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Chat : MonoBehaviour
{
    public string username;

    public int maxMessages = 25;

    public GameObject chatPanel, textObject;

    public InputField chatBox;

    public Color PlayerMessage, info;

   [SerializeField]
   List<Message> messagesList = new List<Message>();
    void Start()
    {
        
    }

    
    void Update()
    {
        if(chatBox.text != "") 
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendMessageToChat(username + ": " + chatBox.text, Message.MessageType.playerMessage);
                chatBox.text = "";
            }
        }
        else
        {
            if(!chatBox.isFocused && Input.GetKeyDown(KeyCode.Return))
                chatBox.ActivateInputField();
        }

        if(!chatBox.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SendMessageToChat("Bạn chưa nhập nội dung !", Message.MessageType.info);
                Debug.Log("Space");
            }
        }
       
    }

    public void SendMessageToChat(string text, Message.MessageType messageType)
    {
        if (messagesList.Count >= maxMessages)
        {
            Destroy(messagesList[0].textObject.gameObject);
            messagesList.Remove(messagesList[0]);
        }

        Message newMessage = new Message();

        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newText.GetComponent<Text>();

        newMessage.textObject.text = newMessage.text;

        newMessage.textObject.color=MessageTypeColor(messageType);

        messagesList.Add(newMessage);
    }

    Color MessageTypeColor(Message.MessageType messageType)
    {
        Color color = info;

        switch(messageType)
        {
            case Message.MessageType.playerMessage:
                color = PlayerMessage;
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
