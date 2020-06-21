using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is the SocialManager script, it contains functionality that is specific to the social
/// </summary>
public class SocialManager : MonoBehaviour
{
    [Header("Variables")]
    // Player username
    public string username;

    [Header("References")]
    public ChatBoxManager chatBoxManager;
    //public GameObject SocialPanel;
    //public GameObject SocialBoxContent;
    //public GameObject usernameObject;
    //public GameObject AddSocialPanel;
    //public InputField AddSocialPanelInput;

    [Header("Social list")]
    [SerializeField] List<Social> socialList = new List<Social>();

    // Start is called before the first frame update
    void Start()
    {
        UpdateSocialPanel("Pytchoun");
        UpdateSocialPanel("Kaze");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Toggle Social Panel
    /// </summary>
    public void ToggleSocialPanel()
    {
        if (UIManager.MyInstance.SocialPanel != null)
        {
            bool isActive = UIManager.MyInstance.SocialPanel.activeSelf;

            UIManager.MyInstance.SocialPanel.SetActive(!isActive);
        }
    }

    /// <summary>
    /// Update Social Panel
    /// </summary>
    public void UpdateSocialPanel(string text)
    {
        Social newSocial = new Social();

        newSocial.username = text;

        GameObject newText = Instantiate(UIManager.MyInstance.usernameObject, UIManager.MyInstance.SocialBoxContent.transform);

        newSocial.usernameObject = newText.GetComponent<Text>();

        newSocial.usernameObject.text = newSocial.username;

        socialList.Add(newSocial);
    }

    /// <summary>
    /// Toggle Add Social Panel
    /// </summary>
    public void ToggleAddSocialPanel()
    {
        if (UIManager.MyInstance.AddSocialPanel != null)
        {
            bool isActive = UIManager.MyInstance.AddSocialPanel.activeSelf;

            UIManager.MyInstance.AddSocialPanel.SetActive(!isActive);
        }
    }

    /// <summary>
    /// Add a friend
    /// </summary>
    public void AddAFriend()
    {
        // Check if input isn't empty
        if (!string.IsNullOrWhiteSpace(UIManager.MyInstance.AddSocialPanelInput.text))
        {
            // Check if user exist
            if (true)
            {
                // Add the user as a friend
                Debug.Log("Ajout de l'ami.");

                // Send a confirmation message
                chatBoxManager.SendMessageToChat("L'utilisateur a été ajouté à votre liste d'ami.", Message.MessageType.server);
            }
            else
            {
                // Send an error message
                chatBoxManager.SendMessageToChat("Le destinataire est introuvable.", Message.MessageType.server);
            }

            // Clean the input field
            UIManager.MyInstance.AddSocialPanelInput.text = "";
        }
    }
}

// Content of social panel
[System.Serializable]
public class Social
{
    public string username;
    public Text usernameObject;
}