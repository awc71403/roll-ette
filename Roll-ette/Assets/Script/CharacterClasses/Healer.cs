using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healer : Character
{
    //strength, speed, knowledge, will
    private int[] healerStats = { 4, 4, 3, 3 };

    // Start is called before the first frame update
    void Awake()
    {
        totalHealth = 15;
        currentHealth = totalHealth;
        curStatArr = healerStats;
        cName = "Bartender";
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
        menuName.text = "Healer";
        mNum.text = GetMight().ToString();
        sNum.text = GetSpeed().ToString();
        kNum.text = GetKnowledge().ToString();
        wNum.text = GetWill().ToString();

        //open menu for character, display stats, etc.
    }

    public override List<int[,]> GetAttackRange()
    {
        List<int[,]> attackRanges = new List<int[,]>();

        int[,] forwardRange = {
            {1, 0 },
            {2, 0 },
        };

        int[,] aboveRange = {
            {0, 1},
            {0, 2},
        };

        int[,] belowRange = {
            {0, -1},
            {0, -2},
        };

        int[,] behindRange = {
            {-1, 0},
            {-2, 0},
        };

        attackRanges.Add(forwardRange);
        attackRanges.Add(aboveRange);
        attackRanges.Add(belowRange);
        attackRanges.Add(behindRange);

        return attackRanges;
    }
}
