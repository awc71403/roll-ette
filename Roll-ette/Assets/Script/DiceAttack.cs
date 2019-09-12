using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DiceAttack : MonoBehaviour
{
    #region audio_variables
    public AudioClip shootSound;
    public AudioClip[] diceRollSounds;
    public AudioSource source;
    #endregion

    #region dice_variables
    private int[] attackDice = { 1, 2, 3, 4, 5, 6 };
    private int[] defenseDice = { 1, 1, 1, 2, 2, 3 };
    #endregion

    #region dice_face_variables
    private Image[] diceFaces;
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

    [SerializeField]
    private Image diceSumTextPrefab;

    [SerializeField]
    private Text attacker;
    public Text attackerImage;

    [SerializeField]
    private Text defender;
    public Text defenderImage;
    public Image attackDiceSum;
    public Image defenseDiceSum;
    #endregion

    #region dice_position_variables
    [SerializeField]
    private GameObject diceAttackStartingPosition;

    [SerializeField]
    private GameObject diceDefenseStartingPosition;

    public float diceSpacing = 40;
    #endregion

    #region meta_variables
    [SerializeField]
    private Transform canvas;

    [SerializeField]
    public GameObject levelManager;

    [SerializeField]
    public Text damageText;
    #endregion

    #region attacking_variables
    bool running;
    private Image[] attackerDice;
    private Image[] defenderDice;
    public int attackerTurns;
    public int defenderTurns;
    public GameObject attackingUnit;
    public GameObject defendingUnit;
    public int damage;
    public int defense;
    public int netDamage;
    public float sumDisplayTime = 1f;
    #endregion

    #region sheriff_ability_variables
    bool sheriffCanRerollAttackingDie;
    bool sheriffCanRerollDefendingDie;

    [SerializeField]
    Button RerollAttackButtonPrefab;
    public Button RerollAttackButton;

    [SerializeField]
    Button RerollDefenseButtonPrefab;
    public Button RerollDefenseButton;

    [SerializeField]
    Button ConfirmAttackButtonPrefab;
    public Button ConfirmAttackButton;
    #endregion

    #region misc_functions
    /*
     * Runs when instantiated.
     */
    public void Awake()
    {
        /// Setting up die face images
        diceFaces = new Image[7];
        diceFaces[0] = zeroRoll;
        diceFaces[1] = oneRoll;
        diceFaces[2] = twoRoll;
        diceFaces[3] = threeRoll;
        diceFaces[4] = fourRoll;
        diceFaces[5] = fiveRoll;
        diceFaces[6] = sixRoll;

        // Initializing damage and efense
        damage = 0;
        defense = 0;
    }

    /*
     * Returns whether attacking is currently occuring.
     */
    public bool IsRunning()
    {
        return running;
    }
    #endregion

    #region rolling_functions
    /*
     * Starts the attacking coroutine.
     */
    public void StartRolling(GameObject attackingUnit, GameObject targetUnit)
    {
        StartCoroutine(ShowDiceRolls(attackingUnit, targetUnit));
    }

    /*
     * Return the result of a random roll of the given die.
     */
    public int DiceRoll(int[] die)
    {
        int index = Random.Range(0, die.Length);
        return die[index];
    }

    /*
     * Unshows all dice, and all dice UI elements.
     */
    public void UnshowDice()
    {
        UnshowAttackerDice();
        UnshowDefenderDice();

        if (RerollAttackButton != null)
        {
            RerollAttackButton.gameObject.SetActive(false);
            Destroy(RerollAttackButton);
        }

        if (ConfirmAttackButton != null)
        {
            ConfirmAttackButton.gameObject.SetActive(false);
            Destroy(ConfirmAttackButton);
        }

        if (RerollDefenseButton != null)
        {
            RerollDefenseButton.gameObject.SetActive(false);
            Destroy(RerollDefenseButton);
        }
    }

    /*
     * Unshows attacker dice.
     */
    private void UnshowAttackerDice()
    {
        foreach (Image die in attackerDice)
        {
            Destroy(die);
        }
        Destroy(attackerImage);

        if (attackDiceSum != null)
        {
            attackDiceSum.gameObject.SetActive(false);
            Destroy(attackDiceSum);
        }
    }

    /*
     * Unshows defender dice.
     */
    private void UnshowDefenderDice()
    {
        foreach (Image die in defenderDice)
        {
            Destroy(die);
        }
        Destroy(defenderImage);

        if (defenseDiceSum != null)
        {
            defenseDiceSum.gameObject.SetActive(false);
            Destroy(defenseDiceSum);
        }
    }

    private void DisplayAttackDiceSumText(Vector3 position, string text)
    {
        attackDiceSum = Instantiate(diceSumTextPrefab, canvas);
        attackDiceSum.transform.position = position;
        attackDiceSum.GetComponentInChildren<Text>().text = text;
    }

    private void DisplayDefenseDiceSumText(Vector3 position, string text)
    {
        defenseDiceSum = Instantiate(diceSumTextPrefab, canvas);
        defenseDiceSum.transform.position = position;
        defenseDiceSum.GetComponentInChildren<Text>().text = text;
    }
    #endregion

    #region show_dice_rolls_function
    /* 
     * Coroutine for attacking.
     */
    IEnumerator ShowDiceRolls(GameObject attackingU, GameObject targetU)
    {
        // Set running to true; can't do anything else while attacking.
        running = true;

        // Set up who's attacking and defending.
        attackingUnit = attackingU;
        defendingUnit = targetU;

        // Play dice roll sound effect
        PlayDiceRollSound();

        // Calculate the number of rolls.
        if (attackingUnit.GetComponent<Character>().GetName() == "Thief")
        {
            attackerTurns = attackingUnit.GetComponent<Character>().GetSpeed();
            defenderTurns = defendingUnit.GetComponent<Character>().GetSpeed();
        }
        else
        {
            attackerTurns = attackingUnit.GetComponent<Character>().GetMight();
            defenderTurns = defendingUnit.GetComponent<Character>().GetMight();
        }

        // Lower defense of the Roulette Wheel
        if (defendingUnit.GetComponent<Character>().GetOccupiedTile().GetComponent<TileBehavior>().isroulette == 1)
        {
            defenderTurns -= 1;
        }

        int totalTurns = Mathf.Max(attackerTurns, defenderTurns);

        attackerDice = new Image[attackerTurns];
        defenderDice = new Image[defenderTurns];
        if (attackingU.GetComponent<Character>().GetName() == "Mage" && attackingU.GetComponent<Mage>().GetUseRoll()
    && attackingU.GetComponent<Mage>().GetPrevRoll() > 0)
        {
            print(attackingU.GetComponent<Mage>().GetPrevRoll());
            damage = attackingU.GetComponent<Mage>().GetPrevRoll();
        }

        // Set up damage and defense.
        if (damage != 0)
        {
            for (int i = 0; i < defenderTurns; i++)
            {
                int defenseRoll = DiceRoll(defenseDice);
                defenderDice[i] = Instantiate(diceFaces[defenseRoll], canvas);
                defenderDice[i].transform.position = new Vector3(diceDefenseStartingPosition.transform.position.x,
                    diceDefenseStartingPosition.transform.position.y - i * diceSpacing);
                defense += defenseRoll;

                // Brief wait.
                yield return new WaitForSeconds(0.05f);
            }
        }
        else
        {
            // Run through dice rolls.
            for (int i = 0; i < totalTurns; i++)
            {
                // If there are still turns left for the attacker:
                if (i < attackerTurns)
                {
                    int attackRoll = DiceRoll(attackDice);
                    attackerDice[i] = Instantiate(diceFaces[attackRoll], canvas);
                    attackerDice[i].transform.position = new Vector3(diceAttackStartingPosition.transform.position.x,
                        diceAttackStartingPosition.transform.position.y - i * diceSpacing);
                    damage += attackRoll;
                }

                // If there are still turns left for the defender:
                if (i < defenderTurns)
                {
                    int defenseRoll = DiceRoll(defenseDice);
                    defenderDice[i] = Instantiate(diceFaces[defenseRoll], canvas);
                    defenderDice[i].transform.position = new Vector3(diceDefenseStartingPosition.transform.position.x,
                        diceDefenseStartingPosition.transform.position.y - i * diceSpacing);
                    defense += defenseRoll;
                }

                // Brief wait.
                yield return new WaitForSeconds(0.05f);
            }
        }

        //Display dice sum totals.
        Vector3 attackDiceSumPosition = new Vector3(diceAttackStartingPosition.transform.position.x,
            diceAttackStartingPosition.transform.position.y - (attackerTurns) * diceSpacing);
        DisplayAttackDiceSumText(attackDiceSumPosition, damage.ToString());

        Vector3 defenseDiceSumPosition = new Vector3(diceDefenseStartingPosition.transform.position.x,
            diceAttackStartingPosition.transform.position.y - (defenderTurns) * diceSpacing);
        DisplayDefenseDiceSumText(defenseDiceSumPosition, defense.ToString());

        attackerImage = Instantiate(attacker, canvas);
        attackerImage.transform.position = new Vector3(diceAttackStartingPosition.transform.position.x + 50,
                    diceDefenseStartingPosition.transform.position.y);

        defenderImage = Instantiate(defender, canvas);
        defenderImage.transform.position = new Vector3(diceDefenseStartingPosition.transform.position.x - 50,
                    diceDefenseStartingPosition.transform.position.y);

        if (attackingUnit.GetComponent<Character>().GetName() == "Mage" &&
            attackingU.GetComponent<Character>().GetName() == "Mage" && attackingU.GetComponent<Mage>().GetUseRoll())
        {
            attackingUnit.GetComponent<Mage>().ResetUsePrev();
            levelManager.GetComponent<LevelManager>().ResetUIAfterAbil(attackingUnit);
            attackingUnit.GetComponent<Mage>().SetTurnDone(true);
        }

        // Sheriff special ability.
        if (attackingUnit.GetComponent<Character>().GetName().Equals("Sheriff"))
        {
            sheriffCanRerollAttackingDie = true;
            sheriffCanRerollDefendingDie = true;

            RerollAttackButton = Instantiate(RerollAttackButtonPrefab, canvas);
            RerollAttackButton.transform.position = new Vector3(diceAttackStartingPosition.transform.position.x,
                diceAttackStartingPosition.transform.position.y - (attackerTurns + 1) * diceSpacing);
            RerollAttackButton.onClick.AddListener(PressRerollAttackButton);

            RerollDefenseButton = Instantiate(RerollDefenseButtonPrefab, canvas);
            RerollDefenseButton.transform.position = new Vector3(diceDefenseStartingPosition.transform.position.x,
                diceDefenseStartingPosition.transform.position.y - (defenderTurns + 1) * diceSpacing);
            RerollDefenseButton.onClick.AddListener(PressRerollDefenseButton);

            ConfirmAttackButton = Instantiate(ConfirmAttackButtonPrefab, canvas);
            ConfirmAttackButton.transform.position = new Vector3(diceAttackStartingPosition.transform.position.x,
                diceAttackStartingPosition.transform.position.y - (attackerTurns + 2) * diceSpacing);
            ConfirmAttackButton.onClick.AddListener(PressConfirmAttackButton);
        }
        else
        {
            // Clear dice and start allocation.
            yield return new WaitForSeconds(sumDisplayTime);
            EndAttack();
        }
    }
    #endregion

    #region sheriff_ability_functions
    public void PressRerollAttackButton()
    {
        if (sheriffCanRerollAttackingDie)
        {
            StartCoroutine("RerollAttackButtonCoroutine");
        }
        sheriffCanRerollAttackingDie = false;
    }

    IEnumerator RerollAttackButtonCoroutine()
    {
        // Delete previous roll
        UnshowAttackerDice();

        // Play dice roll sound effect
        PlayDiceRollSound();

        // Run through dice rolls
        damage = 0;
        attackerDice = new Image[attackerTurns];
        for (int i = 0; i < attackerTurns; i++)
        {
            // If there are still turns left for the attacker:
            if (i < attackerTurns)
            {
                int attackRoll = DiceRoll(attackDice);
                attackerDice[i] = Instantiate(diceFaces[attackRoll], canvas);
                attackerDice[i].transform.position = new Vector3(diceAttackStartingPosition.transform.position.x,
                    diceAttackStartingPosition.transform.position.y - i * diceSpacing);
                damage += attackRoll;
            }

            // Brief wait
            yield return new WaitForSeconds(0.05f);
        }

        //Display dice sum totals.
        Vector3 attackDiceSumPosition = new Vector3(diceAttackStartingPosition.transform.position.x,
            diceAttackStartingPosition.transform.position.y - (attackerTurns) * diceSpacing);
        DisplayAttackDiceSumText(attackDiceSumPosition, damage.ToString());
    }

    public void PressRerollDefenseButton()
    {
        if (sheriffCanRerollDefendingDie)
        {
            StartCoroutine("RerollDefenseButtonCoroutine");
        }
        sheriffCanRerollDefendingDie = false;
    }

    IEnumerator RerollDefenseButtonCoroutine()
    {
        // Delete previous roll
        UnshowDefenderDice();

        // Play dice roll sound effect
        PlayDiceRollSound();

        // Run through dice rolls
        defense = 0;
        defenderDice = new Image[defenderTurns];
        for (int i = 0; i < defenderTurns; i++)
        {
            // If there are still turns left for the defender:
            if (i < defenderTurns)
            {
                int defenseRoll = DiceRoll(defenseDice);
                defenderDice[i] = Instantiate(diceFaces[defenseRoll], canvas);
                defenderDice[i].transform.position = new Vector3(diceDefenseStartingPosition.transform.position.x,
                    diceDefenseStartingPosition.transform.position.y - i * diceSpacing);
                defense += defenseRoll;
            }

            // Brief wait
            yield return new WaitForSeconds(0.05f);
        }

        // Display dice sum totals.
        Vector3 defenseDiceSumPosition = new Vector3(diceDefenseStartingPosition.transform.position.x,
            diceAttackStartingPosition.transform.position.y - (defenderTurns) * diceSpacing);
        DisplayDefenseDiceSumText(defenseDiceSumPosition, defense.ToString());
    }

    public void PressConfirmAttackButton()
    {
        EndAttack();
    }
    #endregion

    #region button_functions
    public void EndAttack()
    {
        UnshowDice();
        // Calculate net damage.
        netDamage = Mathf.Max(damage - defense, 0);
        print("Damage: " + damage);
        print("Defense: " + defense);
        print("Damage done: " + netDamage);
        levelManager.GetComponent<LevelManager>().OpenAllocation(attackingUnit, defendingUnit, netDamage);
        attackingUnit.GetComponent<Character>().SetCanAttack(false);
        attackingUnit.GetComponent<Character>().SetCanMove(false);
        PlayShootSound();
        damage = 0;
        defense = 0;
        running = false;
    }

    public void PlayShootSound()
    {
        source.clip = shootSound;
        source.Play();
    }

    public void PlayDiceRollSound()
    {
        System.Random r = new System.Random();
        int diceRollNum = r.Next(0, diceRollSounds.Length);
        source.clip = diceRollSounds[diceRollNum];
        source.Play();
    }
    #endregion

    public Transform GetCanvas()
    {
        return canvas;
    }

    public Text GetDamageText()
    {
        return damageText;
    }
}

