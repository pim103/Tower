using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is the AchievmentManager script, it contains functionality that is
/// specific to the Achievment
/// </summary>
public class AchievmentManager : MonoBehaviour {
  [Header("References")]
  public GameObject AchievmentPrefab;
  public GameObject visualAchievment;
  public Sprite UnlockedSprite;
  public Color32 UnlockedColor = new Color32(0, 255, 249, 255);
  [System.NonSerialized]
  public ScrollRect scrollRect;
  [System.NonSerialized]
  public Text textPoints;

  [Header("AchievmentScriptableObjects List")]
  public AchievmentScriptableObject[] achievmentScriptableObjects;

  [Header("Sprites List")]
  public Sprite[] sprites;

  private AchievmentButton activeButton;

  public Dictionary<string, Achievment>Achievments =
      new Dictionary<string, Achievment>();

  private int fadeTime = 2;

  [HideInInspector]
  public bool initAchievment = false;

  private static AchievmentManager instance;
  public static AchievmentManager Instance {
    get {
      if (instance == null) {
        instance = GameObject.FindObjectOfType<AchievmentManager>();
      }

      return AchievmentManager.instance;
    }
  }

  void Awake() {
    // PlayerPrefs.DeleteAll();

    // PlayerPrefs.DeleteKey("Première fabrication");

    // PlayerPrefs.DeleteKey("ProgressionPremière fabrication");

    // PlayerPrefs.DeleteKey("Points");

    scrollRect = UIManager.MyInstance.scrollRect;
    textPoints = UIManager.MyInstance.textPoints;

    InitAchievments();
  }

  // Start is called before the first frame update
  void Start() {}

  // Update is called once per frame
  void Update() {}

  public void InitAchievments() {
    if (initAchievment == false) {
      UIManager.MyInstance.AchievmentPanel.SetActive(true);

      activeButton =
          GameObject.Find("ButtonGeneral").GetComponent<AchievmentButton>();

      // CreateAchievment("General", "Première fabrication", "Commencer
      // l'artisanat", 2, 1, 0); CreateAchievment("General", "Maître de
      // l'artisanat", "Fabriquer 3 objets", 4, 0, 3);

      // CreateAchievment("General", "Deckier", "Mon premier deck", 3, 4, 0);
      // CreateAchievment("General", "Aventurier", "Première partie", 4, 4, 0);
      // CreateAchievment("General", "Combattant", "Première victoire", 6, 4,
      // 0); CreateAchievment("General", "Gladiateur", "Avoir 10 victoires", 6,
      // 4, 0); CreateAchievment("General", "Et de 3", "Avoir une série de 3
      // victoires", 6, 4, 0);

      // CreateAchievment("Other", "Premier achat", "Premier achat dans la
      // boutique", 5, 3, 0); CreateAchievment("Other", "Commerçant",
      // "Connaisseur des affaires", 10, 3, 0, new string[] { "Premier achat",
      // "Maître de l'artisanat" });

      foreach(AchievmentScriptableObject achievmentScriptableObject in
                  achievmentScriptableObjects) {
        CreateAchievmentSO(achievmentScriptableObject.Category,
                           achievmentScriptableObject.Title,
                           achievmentScriptableObject.Description,
                           achievmentScriptableObject.Points,
                           achievmentScriptableObject.SpriteIndex,
                           achievmentScriptableObject.Progress,
                           achievmentScriptableObject.AchievmentDependencies);
      }

      foreach(GameObject achievmentList in GameObject.FindGameObjectsWithTag(
          "AchievmentList")) {
        achievmentList.SetActive(false);
      }

      activeButton.Click();
      initAchievment = true;

      UIManager.MyInstance.AchievmentPanel.SetActive(false);
    }
  }

  public void EarnAchievment(string title) {
    if (Achievments[title].EarnAchievment()) {
      GameObject achievment = (GameObject) Instantiate(visualAchievment);
      SetAchievmentinfo("EarnAchievmentCanvas", achievment, title);
      textPoints.text = "Points: " + PlayerPrefs.GetInt("Points");
      StartCoroutine(FadeAchievment(achievment));
    }
  }

  public void CreateAchievment(string parent, string title, string description,
                               int points, int spriteIndex, int progress,
                               string[] dependencies = null) {
    GameObject achievment = (GameObject) Instantiate(AchievmentPrefab);

    Achievment newAchievment = new Achievment(
        title, description, points, spriteIndex, achievment, progress);

    Achievments.Add(title, newAchievment);

    SetAchievmentinfo(parent, achievment, title, progress);

    if (dependencies != null) {
      foreach(string achievmentTitle in dependencies) {
        Achievment dependency = Achievments[achievmentTitle];
        dependency.Child = title;
        newAchievment.AddDependency(dependency);
      }
    }
  }

  public void
  CreateAchievmentSO(string parent, string title, string description,
                     int points, int spriteIndex, int progress,
                     AchievmentScriptableObject[] dependencies = null) {
    GameObject achievment = (GameObject) Instantiate(AchievmentPrefab);

    Achievment newAchievment = new Achievment(
        title, description, points, spriteIndex, achievment, progress);

    Achievments.Add(title, newAchievment);

    SetAchievmentinfo(parent, achievment, title, progress);

    if (dependencies != null) {
      foreach(AchievmentScriptableObject achievmentTitle in dependencies) {
        Achievment dependency = Achievments[achievmentTitle.Title];
        dependency.Child = title;
        newAchievment.AddDependency(dependency);
      }
    }
  }

  public void SetAchievmentinfo(string parent, GameObject achievment,
                                string title, int progression = 0) {
    achievment.transform.SetParent(GameObject.Find(parent).transform);
    achievment.transform.localScale = new Vector3(1, 1, 1);

    string progress = progression > 0
                          ? " " + PlayerPrefs.GetInt("Progression" + title) +
                                "/" + progression.ToString()
                          : string.Empty;

    achievment.transform.GetChild(0).GetComponent<Text>().text =
        title + progress;
    achievment.transform.GetChild(1).GetComponent<Text>().text =
        Achievments[title].Description;
    achievment.transform.GetChild(2).GetComponent<Text>().text =
        Achievments[title].Points.ToString();
    achievment.transform.GetChild(3).GetComponent<Image>().sprite =
        sprites[Achievments[title].SpriteIndex];
  }

  public void ChangeCategory(GameObject button) {
    AchievmentButton achievmentButton = button.GetComponent<AchievmentButton>();

    scrollRect.content =
        achievmentButton.achievmentList.GetComponent<RectTransform>();

    achievmentButton.Click();
    activeButton.Click();
    activeButton = achievmentButton;
  }

  public IEnumerator HideAchievment(GameObject achievment) {
    yield return new WaitForSeconds(3);
    Destroy(achievment);
  }

  private IEnumerator FadeAchievment(GameObject achievment) {
    CanvasGroup canvasGroup = achievment.GetComponent<CanvasGroup>();

    float rate = 1.0f / fadeTime;

    int startAlpha = 0;
    int endAlpha = 1;

    for (int i = 0; i < 2; i++) {
      float progress = 0.0f;

      while (progress < 1.0) {
        canvasGroup.alpha = Mathf.Lerp(startAlpha, endAlpha, progress);
        progress += rate * Time.deltaTime;

        yield return null;
      }

      yield return new WaitForSeconds(2);
      startAlpha = 1;
      endAlpha = 0;
    }

    Destroy(achievment);
  }

  /// <summary>
  /// Toggle Achievment Panel
  /// </summary>
  public void ToggleAchievmentPanel() {
    if (UIManager.MyInstance.AchievmentPanel != null) {
      bool isActive = UIManager.MyInstance.AchievmentPanel.activeSelf;
      UIManager.MyInstance.AchievmentPanel.SetActive(!isActive);
    }
  }
}