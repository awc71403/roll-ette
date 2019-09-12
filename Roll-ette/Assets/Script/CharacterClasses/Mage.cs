using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Mage : Character
{
    //strength, speed, knowledge, will
    private int[] mageStats = { 3, 3, 5, 4 };
    private int[] attackDice = { 1, 2, 3, 4, 5, 6 };
    public float diceSpacing = 20;
    public int abilDamage;
    public Image attackDiceSum;
    [SerializeField]
    private Image diceSumTextPrefab;

    [SerializeField]
    public GameObject levelManager;
    public bool running;
    private int saveRoll;
    private bool useRoll;
    private bool turnDone;
    [SerializeField]
    private GameObject diceAttackStartingPosition;
    private Image[] attackerDice;
    private Image[] diceFaces;

    [SerializeField]
    private Transform canvas;

    [SerializeField]
    private Image zeroRoll;

    [SerializeField]
    private Image oneRoll;

    [SerializeField]
    private Image twoRoll;

    [SerializeField]
    private Image threeRoll;

    [SerializeField]
    private Image fourRoll;

    [SerializeField]
    private Image fiveRoll;

    [SerializeField]
    private Image sixRoll;

    private int[] rolls;

    // Start is called before the first frame update
    void Awake()
    {
        totalHealth = 20;
        currentHealth = totalHealth;
        curStatArr = mageStats;
        useRoll = false;
        saveRoll = 0;
        turnDone = false;
        cName = "Mage";
        themeName = "Preacher";
        diceFaces = new Image[7];
        diceFaces[0] = zeroRoll;
        diceFaces[1] = oneRoll;
        diceFaces[2] = twoRoll;
        diceFaces[3] = threeRoll;
        diceFaces[4] = fourRoll;
        diceFaces[5] = fiveRoll;
        diceFaces[6] = sixRoll;
        abilDamage = 0;
        rolls = new int[GetKnowledge() + 2];
    }

    // Update is called once per frame
    void Update()
    {

    }


    public override void TakeDamage(int damage, int stat)
    {
        for (int i = 0; i < curStatArr.Length; i++)
        {
            if (stat == i)
            {
                curStatArr[i] -= damage;
            }
        }
    }

    public override void Ability()
    {
        StartCoroutine(ShowDiceRolls());
    }

    public void UsePrevRoll()
    {
        useRoll = true;
    }

    public void ResetUsePrev()
    {
        useRoll = false;
    }

    public bool GetUseRoll()
    {
        return useRoll;
    }

    public int GetPrevRoll()
    {
        return saveRoll;
    }

    public void SetAbilRoll(int n)
    {
        saveRoll = n;
    }

    public void SetTurnDone(bool b)
    {
        turnDone = b;
    }

    public bool CanUseAbil()
    {
        if (saveRoll > 0 || turnDone)
        {
            return false;
        }
        return true; ;
    }

    public bool GetTurnDone()
    {
        return turnDone;
    }

    public override void DisplayStats()
    {
        menuName.text = "Mage";
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
        };

        int[,] aboveRange = {
            {0, 1},
            {0, 2},
            {0, 3},
        };

        int[,] belowRange = {
            {0, -1},
            {0, -2},
            {0, -3},
        };

        attackRanges.Add(forwardRange);
        attackRanges.Add(aboveRange);
        attackRanges.Add(belowRange);

        return attackRanges;
    }

    IEnumerator ShowDiceRolls()
    {
        running = true;

        // Calculate # of turns
        int turns = GetKnowledge() + 2;

        // Set up
        int damage = 0;
        attackerDice = new Image[turns];
        print(turns);

        //Play dice roll sound
        diceAttack.GetComponent<DiceAttack>().PlayDiceRollSound();

        for (int i = 0; i < rolls.Length; i++)
        {
            int roll = diceAttack.GetComponent<DiceAttack>().DiceRoll(attackDice);
            rolls[i] = roll;
            damage += roll;
        }

        // Run through dice rolls
        for (int i = 0; i < turns; i++)
        {
            attackerDice[i] = Instantiate(diceFaces[rolls[i]], canvas);
            attackerDice[i].transform.position = new Vector3(diceAttackStartingPosition.transform.position.x, diceAttackStartingPosition.transform.position.y - i * diceSpacing);
            // Brief wait
            yield return new WaitForSeconds(0.05f);
        }
        saveRoll = damage;
        Vector3 attackDiceSumPosition = new Vector3(diceAttackStartingPosition.transform.position.x,
            diceAttackStartingPosition.transform.position.y - (turns) * diceSpacing);
        attackDiceSum = Instantiate(diceSumTextPrefab, canvas);
        attackDiceSum.transform.position = attackDiceSumPosition;
        attackDiceSum.GetComponentInChildren<Text>().text = damage.ToString();
        running = false;
        yield return new WaitForSeconds(1f);
        foreach (Image die in attackerDice)
        {
            Destroy(die);
        }

        if (attackDiceSum != null)
        {
            attackDiceSum.gameObject.SetActive(false);
            Destroy(attackDiceSum);
        }
    }
}
