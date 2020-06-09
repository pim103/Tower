using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is the Achievment script, it contains functionality that is specific to the Achievment
/// </summary>
public class Achievment
{
    private string name;
    private string description;
    private bool unlocked;
    private int points;
    private int spriteIndex;
    private GameObject achievmentRef;
    private string child;
    private List<Achievment> dependencies = new List<Achievment>();
    private int currentProgression;
    private int maxProgression;

    public string Name
    {
        get
        {
            return name;
        }
        set
        {
            name = value;
        }
    }

    public string Description
    {
        get
        {
            return description;
        }
        set
        {
            description = value;
        }
    }

    public bool Unlocked
    {
        get
        {
            return unlocked;
        }
        set
        {
            unlocked = value;
        }
    }

    public int Points
    {
        get
        {
            return points;
        }
        set
        {
            points = value;
        }
    }

    public int SpriteIndex
    {
        get
        {
            return spriteIndex;
        }
        set
        {
            spriteIndex = value;
        }
    }

    public string Child
    {
        get
        {
            return child;
        }
        set
        {
            child = value;
        }
    }

    public Achievment(string name, string description, int points, int spriteIndex, GameObject achievmentRef, int maxProgression)
    {
        this.name = name;
        this.description = description;
        this.unlocked = false;
        this.points = points;
        this.spriteIndex = spriteIndex;
        this.achievmentRef = achievmentRef;
        this.maxProgression = maxProgression;
        this.currentProgression = PlayerPrefs.GetInt("Progression" + this.name);

        LoadAchievment();
    }

    public void AddDependency(Achievment dependency)
    {
        dependencies.Add(dependency);
    }

    public bool EarnAchievment()
    {
        if (!unlocked && !dependencies.Exists(x => x.unlocked == false) && CheckProgress())
        {
            achievmentRef.GetComponent<Image>().color = AchievmentManager.Instance.UnlockedColor;
            SaveAchievment(true);

            if (child != null)
            {
                AchievmentManager.Instance.EarnAchievment(child);
            }

            return true;
        }

        return false;
    }

    public void SaveAchievment(bool value)
    {
        unlocked = value;

        if (unlocked)
        {
            int tempPoints = PlayerPrefs.GetInt("Points");
            PlayerPrefs.SetInt("Points", tempPoints += points);
        }
        
        PlayerPrefs.SetInt("Progression" + Name, currentProgression);
        PlayerPrefs.SetInt(name, value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void LoadAchievment()
    {
        unlocked = PlayerPrefs.GetInt(name) == 1 ? true : false;

        if (unlocked)
        {
            AchievmentManager.Instance.textPoints.text = "Points: " + PlayerPrefs.GetInt("Points");
            currentProgression = PlayerPrefs.GetInt("Progression" + Name);
            achievmentRef.GetComponent<Image>().color = AchievmentManager.Instance.UnlockedColor;
        }
    }

    public bool CheckProgress()
    {
        currentProgression++;

        if (maxProgression > 0)
        {
            achievmentRef.transform.GetChild(0).GetComponent<Text>().text = Name + " " + currentProgression + "/" + maxProgression;
        }

        SaveAchievment(false);

        if (maxProgression == 0)
        {
            return true;
        }
        if (currentProgression >= maxProgression)
        {
            return true;
        }

        return false;
    }
}