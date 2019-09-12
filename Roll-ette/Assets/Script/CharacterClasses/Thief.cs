using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thief : Character
{
    //strength, speed, knowledge, will
    private int[] thiefStats = { 2, 6, 3, 3 };

    // Start is called before the first frame update
    void Awake()
    {
        totalHealth = 40;
        currentHealth = totalHealth;
        curStatArr = thiefStats;
        cName = "Thief";
        themeName = "Thief";
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
    public override void ResetStats()
    {
        hp = 10;
    } */

    public override void TakeDamage(int damage, int stat)
    {
        curStatArr[stat] -= damage;
    }

    public override void Ability()
    {
        throw new System.NotImplementedException();
    }

    public override void DisplayStats()
    {
        menuName.text = "Thief";
        mNum.text = GetMight().ToString();
        sNum.text = GetSpeed().ToString();
        kNum.text = GetKnowledge().ToString();
        wNum.text = GetWill().ToString();

        //open menu for character, display stats, etc.
    }

    public override List<int[,]> GetAttackRange()
    {
        int flipIfPlayer2 = 1;
        if (player == 2)
        {
            flipIfPlayer2 = -1;
        }

        List<int[,]> attackRanges = new List<int[,]>();

        int[,] forwardRange = {
            {1 * flipIfPlayer2, 0 },
        };

        int[,] aboveRange = {
            {0, 1},
        };

        int[,] belowRange = {
            {0, -1},
        };

        int[,] behindRange = {
            {-1 * flipIfPlayer2, 0 },
            {-1 * flipIfPlayer2, 1 },
            {-1 * flipIfPlayer2, -1 },
        };

        attackRanges.Add(forwardRange);
        attackRanges.Add(aboveRange);
        attackRanges.Add(belowRange);
        attackRanges.Add(behindRange);

        return attackRanges;
    }
}
