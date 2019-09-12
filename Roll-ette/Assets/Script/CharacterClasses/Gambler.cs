using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gambler : Character
{
    //strength, speed, knowledge, will
    private int[] stats = { 3, 5, 0, 0 };

    // Start is called before the first frame update
    void Awake()
    {
        totalHealth = 10;
        currentHealth = totalHealth;
        curStatArr = stats;
        cName = "Gambler";
        themeName = "Gambler";
    }

    // Update is called once per frame
    void Update()
    {

    }

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
        menuName.text = "Gambler";
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
            {2 * flipIfPlayer2, 0 },
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
            {-1 * flipIfPlayer2, 0 },
            {-2 * flipIfPlayer2, 0 },
        };

        attackRanges.Add(forwardRange);
        attackRanges.Add(aboveRange);
        attackRanges.Add(belowRange);
        attackRanges.Add(behindRange);

        return attackRanges;
    }
}
