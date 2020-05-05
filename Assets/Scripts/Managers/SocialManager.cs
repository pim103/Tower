using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is the SocialManager script, it contains functionality that is specific to the social
/// </summary>
public class SocialManager : MonoBehaviour
{
    // Player username
    public string username;

    // References
    [Header("References")]
    public GameObject SocialPanel;
    public GameObject SocialBoxContent;
    public GameObject usernameObject;

    // Social list
    [Header("Social list")]
    [SerializeField] List<Social> socialList = new List<Social>();

    // Start is called before the first frame update
    void Start()
    {
        UpdateSocialPanel("Benoît");
        UpdateSocialPanel("Medhi");
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
        if (SocialPanel != null)
        {
            bool isActive = SocialPanel.activeSelf;

            SocialPanel.SetActive(!isActive);
        }
    }

    /// <summary>
    /// Update Social Panel
    /// </summary>
    public void UpdateSocialPanel(string text)
    {
        Social newSocial = new Social();

        newSocial.username = text;

        GameObject newText = Instantiate(usernameObject, SocialBoxContent.transform);

        newSocial.usernameObject = newText.GetComponent<Text>();

        newSocial.usernameObject.text = newSocial.username;

        socialList.Add(newSocial);
    }
}

// Content of social
[System.Serializable]
public class Social
{
    public string username;
    public Text usernameObject;
}