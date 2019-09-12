using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cowboy : Character
{
    //strength, speed, knowledge, will
    private int[] stats = { 3, 4, 3, 3 };


    // Start is called before the first frame update
    void Awake()
    {
        totalHealth = 15;
        currentHealth = totalHealth;
        curStatArr = stats;
        cName = "Cowboy";
        themeName = "Cowboy";
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

    public override int GetMight()
    {
        if (GetCanMove() && LevelManager.currentPlayer == GetPlayer())
        {
            return curStatArr[0] + 3;
        }
        return curStatArr[0];
    }

    public override void DisplayStats()
    {
        menuName.text = "Cowboy";
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
            {3 * flipIfPlayer2, 0 },
            {4 * flipIfPlayer2, 0 },
        };

        attackRanges.Add(forwardRange);

        return attackRanges;
    }
}
