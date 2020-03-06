using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// This is the account script, it contains functionality that is specific to the account
/// </summary>
public class AccountManager : MonoBehaviour
{
    private static AccountManager instance;

    public static AccountManager MyInstance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AccountManager>();
            }

            return instance;
        }
    }

    [SerializeField]
    private int goldBar;

    [SerializeField]
    private int goldNugget;

    [SerializeField]
    private int stoneOre;

    /*[SerializeField]
    private Craft craft;*/

    [SerializeField]
    private Text goldBarText;

    [SerializeField]
    private Text goldNuggetText;

    [SerializeField]
    private Text stoneOreText;

    protected Coroutine actionRoutine;
    public Coroutine MyInitRoutine { get; set; }

    public int MyGoldBar
    {
        get
        {
            return goldBar;
        }
    }

    public int MyGoldNugget
    {
        get
        {
            return goldNugget;
        }
    }

    public int MyStoneOre
    {
        get
        {
            return stoneOre;
        }
    }

    public IEnumerator CraftRoutine(ICastable castable)
    {
        yield return actionRoutine = StartCoroutine(ActionRoutine(castable));

        //craft.AdddItemsToInventory();
    }

    private IEnumerator ActionRoutine(ICastable castable)
    {
        /* SpellBook.MyInstance.Cast(castable);

        IsAttacking = true; //Indicates if we are attacking

        MyAnimator.SetBool("attack", IsAttacking); //Starts the attack animation

        foreach (GearSocket g in gearSockets)
        {
            g.MyAnimator.SetBool("attack", IsAttacking);
        } */

        yield return new WaitForSeconds(castable.MyCastTime);

        //StopAction();
    }

    // Start is called before the first frame update
    void Start()
    {
        goldBar = 100;
        goldNugget = 50;
        stoneOre = 20;

        goldBarText.text = MyGoldBar.ToString();
        goldNuggetText.text = MyGoldNugget.ToString();
        stoneOreText.text = MyStoneOre.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
