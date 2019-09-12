using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Allocation : MonoBehaviour
{
    #region AllocationVariables
    private GameObject selectedUnit;
    // incase of undo
    private static int[] saveStats;
    // tracks how much damage you need to allocate left
    private int damageTaken;
    // incase of undo
    private int damageCounter;
    // damageType = "physical" Physical
    //damageType = "mental" Mental
    public static string damageType;
    #endregion

    #region UI_variables
    public RectTransform allocationUI;
    public Slider MSlider;
    public Slider SSlider;
    public Slider KSlider;
    public Slider WSlider;
    public Text damageLeftText;
    public GameObject diceAttack;
    #endregion

    #region Allocation
    // called once the damage in complete
    // opens the Allocation UI for character damage
    // sets up all the variables
    public void startAllocation(GameObject unit, int damage, string type)
    {
        selectedUnit = unit;
        saveStats = unit.GetComponent<Character>().GetCopyStats();
        damageCounter = damage;
        damageTaken = damage;
        damageType = type;
        LevelManager.singleton.testSingleton = true;
        MSlider.value = unit.GetComponent<Character>().GetMight();
        SSlider.value = unit.GetComponent<Character>().GetSpeed();
        KSlider.value = unit.GetComponent<Character>().GetKnowledge();
        WSlider.value = unit.GetComponent<Character>().GetWill();
        damageLeftText.text = damage.ToString();
        allocationUI.gameObject.SetActive(true);
    }

    // button for allocating Might
    public void AllocateM()
    {
        if (damageType == "mental" || damageTaken <= 0)
        {
            print("cannot allocate to M");
            return;
        }
        else
        {
            selectedUnit.GetComponent<Character>().TakeDamage(1, 0);
            damageTaken -= 1;
            damageLeftText.text = damageTaken.ToString();
            MSlider.value -= 1;
            print("allocated to M, " + damageTaken + "left, saveMight is " + saveStats[0]);
        }
    }

    // button for allocating Speed
    public void AllocateS()
    {
        if (damageType == "mental" || damageTaken <= 0)
        {
            print("cannot allocate to S");
            return;
        }
        else
        {
            selectedUnit.GetComponent<Character>().TakeDamage(1, 1);
            damageTaken -= 1;
            damageLeftText.text = damageTaken.ToString();
            SSlider.value -= 1;
            print("allocated to S, " + damageTaken + "left");
        }
    }

    // button for allocating Knowledge
    public void AllocateK()
    {
        if (damageType == "physical" || damageTaken <= 0)
        {
            print("cannot allocate to K");
            return;
        }
        else
        {
            selectedUnit.GetComponent<Character>().TakeDamage(1, 2);
            damageTaken -= 1;
            damageLeftText.text = damageTaken.ToString();
            KSlider.value -= 1;
            print("allocated to K, " + damageTaken + "left");
        }
    }

    // button for allocating Will
    public void AllocateW()
    {
        if (damageType == "physical" || damageTaken <= 0)
        {
            print("cannot allocate to W");
            return;
        }
        else
        {
            selectedUnit.GetComponent<Character>().TakeDamage(1, 3);
            damageTaken -= 1;
            damageLeftText.text = damageTaken.ToString();
            WSlider.value -= 1;
            print("allocated to W, " + damageTaken + "left");
        }
    }

    // undos all damage dealt incase of a player mistake
    public void undo()
    {
        print("reset and redo allocation");
        print("saveStat " + saveStats[0] + " " + saveStats[1] + " " + saveStats[2] + " " + saveStats[3]);
        selectedUnit.GetComponent<Character>().ModifyStats(saveStats);
        damageTaken = damageCounter;
        damageLeftText.text = damageTaken.ToString();
        MSlider.value = saveStats[0];
        SSlider.value = saveStats[1];
        KSlider.value = saveStats[2];
        WSlider.value = saveStats[3];
    }

    // can only be used when all damage is allocated
    public void exitAllocation()
    {
        if (damageTaken != 0)
        {
            print("still " + damageTaken + " damage to allocate");
            return;
        }
        else
        {
            print("exit allocation");
            LevelManager.singleton.testSingleton = false;
            diceAttack.GetComponent<DiceAttack>().UnshowDice();
            allocationUI.gameObject.SetActive(false);
        }
    }
    #endregion
}
