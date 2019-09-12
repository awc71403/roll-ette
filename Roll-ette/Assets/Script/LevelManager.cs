using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LevelManager : MonoBehaviour
{
    public Color saveColor;
    public static LevelManager singleton;
    public bool testSingleton;

    #region audio_variables
    public AudioClip coinSound;
    public AudioSource audioSource;
    #endregion

    #region unit_variables
    [SerializeField]
    GameObject testCharacter;

    [SerializeField]
    GameObject SheriffPrefab;

    [SerializeField]
    GameObject CowboyPrefab;

    [SerializeField]
    GameObject GamblerPrefab;

    [SerializeField]
    GameObject PreacherPrefab;

    [SerializeField]
    GameObject NativePrefab;

    List<GameObject> player1Units = new List<GameObject>();
    List<GameObject> player2Units = new List<GameObject>();
    public int unitsCount;
    public static GameObject tempUnit;
    public static int tempX;
    public static int tempY;
    public bool changed;
    #endregion

    #region tile_variables
    [SerializeField]
    private GameObject[] tilePrefabs;
    [SerializeField]
    public int gridHeight;
    [SerializeField]
    public int gridWidth;
    public static int statGridWidth;
    GameObject[,] mapArray;
    float tileSize;
    #endregion

    #region UI_variables
    public Text Time;

    [SerializeField]
    private RectTransform winUIPrefab;
    public RectTransform winUI;

    public Text winText;

    [SerializeField]
    private RectTransform allocationUIPrefab;
    public RectTransform allocationUI;

    [SerializeField]
    private Button attackButtonPrefab;
    public Button attackButton;

    [SerializeField]
    private Button abilityButtonPrefab;
    public Button abilityButton;

    [SerializeField]
    private Button useRollButtonPrefab;
    public Button useRollButton;

    [SerializeField]
    private Button healButtonPrefab;
    public Button healButton;

    [SerializeField]
    private Button respawnButtonPrefab;
    public Button respawnButton;

    [SerializeField]
    private Text SpeedStatTextPrefab;
    public Text SpeedStatText;

    [SerializeField]
    private Text MightStatTextPrefab;
    public Text MightStatText;

    [SerializeField]
    private Text KnowledgeStatTextPrefab;
    public Text KnowledgeStatText;

    [SerializeField]
    Text WillStatTextPrefab;
    public Text WillStatText;

    [SerializeField]
    private Text HPStatTextPrefab;
    public Text HPStatText;

    [SerializeField]
    private Slider HPBarPrefab;
    public Slider HPBar;

    [SerializeField]
    private Text SavedRollTextPrefab;
    public Text SavedRollText;

    [SerializeField]
    private Button EndTurnButtonPrefab;
    public Button EndTurnButton;

    //public Text CurrentPlayerText;

    [SerializeField]
    private GameObject DiceAttack;

    public Transform contentPanel;

    [SerializeField]
    private Image Player1MoneyUI;
    int player1money = 0;

    [SerializeField]
    private Image Player2MoneyUI;
    int player2money = 0;

    [SerializeField]
    private Button MoneyButtonPrefab;
    public Button MoneyButton;
    #endregion

    #region other_variables
    public static int currentPlayer = 1;
    public static bool actionInProcess;
    public int gameTime = 0;
    int winAmount = 2000;
    #endregion

    #region Spawn variables
    public int turnCount;
    public int savePlayerTurn;
    #endregion

    public static bool usingRoll;

    #region unity_functions
    void Start()
    {
        if (singleton == null)
        {
            singleton = this;
        }
        else if (singleton != this)
        {
            Destroy(gameObject);
        }

        changed = false;
        usingRoll = false;
        tileSize = tilePrefabs[0].GetComponent<SpriteRenderer>().sprite.bounds.size.x;
        CreateTiles();
        SetUpUI();
        TileBehavior.diceAttack = DiceAttack;
        gameTime = 0;
        turnCount = 2;
        statGridWidth = gridWidth;
        saveColor = mapArray[0, 0].GetComponent<SpriteRenderer>().color;
        audioSource = GetComponent<AudioSource>();


        // FOR TESTING PURPOSES
        PlaceCharacterOnTile(PreacherPrefab, 0, 1, 1);
        PlaceCharacterOnTile(GamblerPrefab, 0, 2, 1);
        PlaceCharacterOnTile(CowboyPrefab, 0, 3, 1);
        PlaceCharacterOnTile(NativePrefab, 0, 4, 1);
        PlaceCharacterOnTile(SheriffPrefab, 0, 5, 1);

        PlaceCharacterOnTile(PreacherPrefab, 10, 5, 2);
        PlaceCharacterOnTile(GamblerPrefab, 10, 4, 2);
        PlaceCharacterOnTile(CowboyPrefab, 10, 3, 2);
        PlaceCharacterOnTile(NativePrefab, 10, 2, 2);
        PlaceCharacterOnTile(SheriffPrefab, 10, 1, 2);

        unitsCount = 5;
    }

    public void Update()
    {

    }
    #endregion

    #region setting_up

    public void CreateTiles()
    {

        string[] mapData = ReadLevelText();

        int mapXSize = mapData[0].ToCharArray().Length;
        int mapYSize = mapData.Length;

        // Fill mapArray, which should be empty at first.
        mapArray = new GameObject[mapXSize, mapYSize];

        // Calculate the size of the map.
        float mapWidth = mapXSize * tileSize;
        float mapHeight = mapYSize * tileSize;

        // Finds the top left corner.
        Vector3 worldStart = new Vector3(-mapWidth / 2.0f + (0.5f * tileSize), mapHeight / 2.0f - (0.5f * tileSize));

        // Nested for loop that creates mapYSize * mapXSize tiles.
        for (int y = 0; y < mapYSize; y++)
        {
            char[] newTiles = mapData[y].ToCharArray();
            for (int x = 0; x < mapXSize; x++)
            {
                PlaceTile(newTiles[x].ToString(), x, y, worldStart);
            }
        }

        // Player side roulettes
        mapArray[3, 1].GetComponent<Roulette>().SetPlayer(1);
        mapArray[7, 5].GetComponent<Roulette>().SetPlayer(2);
    }

    // Places a tile at position (x, y).
    private void PlaceTile(string tileType, int x, int y, Vector3 worldStart)
    {
        int tileIndex = int.Parse(tileType);

        // Creates a new tile instance.
        GameObject newTile = Instantiate(tilePrefabs[tileIndex]);

        // Calculates where it should go.
        float newX = worldStart.x + (tileSize * x);
        float newY = worldStart.y - (tileSize * y);

        // Puts it there.
        newTile.transform.position = new Vector3(newX, newY, 0);
        newTile.GetComponent<TileBehavior>().xPosition = x;
        newTile.GetComponent<TileBehavior>().yPosition = y;

        // Adds it to mapArray so we can keep track of it later.
        mapArray[x, y] = newTile;
    }

    private string[] ReadLevelText()
    {
        TextAsset bindData = Resources.Load("Level") as TextAsset;
        string data = bindData.text.Replace("\r\n", string.Empty);
        return data.Split('-');
    }

    private void SetUpUI()
    {
        // Win Condition
        winUI = Instantiate(winUIPrefab);
        winUI.transform.SetParent(contentPanel, false);
        winUI.gameObject.SetActive(false);

        // Attack button
        attackButton = Instantiate(attackButtonPrefab);
        attackButton.transform.SetParent(contentPanel, false);
        attackButton.onClick.AddListener(PressAttackButton);
        TileBehavior.attackButton = attackButton;
        attackButton.gameObject.SetActive(false);

        // Ability button
        abilityButton = Instantiate(abilityButtonPrefab);
        abilityButton.transform.SetParent(contentPanel, false);
        abilityButton.onClick.AddListener(PressAbilityButton);
        TileBehavior.abilityButton = abilityButton;
        abilityButton.gameObject.SetActive(false);

        // Use Roll button
        useRollButton = Instantiate(useRollButtonPrefab);
        useRollButton.transform.SetParent(contentPanel, false);
        useRollButton.onClick.AddListener(PressUseRollButton);
        TileBehavior.useRollButton = useRollButton;
        useRollButton.gameObject.SetActive(false);

        // Respawn button
        respawnButton = Instantiate(respawnButtonPrefab);
        respawnButton.transform.SetParent(contentPanel, false);
        respawnButton.onClick.AddListener(PressRespawnButton);
        respawnButton.gameObject.SetActive(false);

        // Heal button
        healButton = Instantiate(healButtonPrefab);
        healButton.transform.SetParent(contentPanel, false);
        healButton.onClick.AddListener(PressHealButton);
        //TileBehavior.healButton = healButton;
        healButton.gameObject.SetActive(false);

        // Might Stat Text
        MightStatText = Instantiate(MightStatTextPrefab);
        MightStatText.transform.SetParent(contentPanel, false);
        MightStatText.gameObject.SetActive(false);

        // Speed Stat Text
        SpeedStatText = Instantiate(SpeedStatTextPrefab);
        SpeedStatText.transform.SetParent(contentPanel, false);
        SpeedStatText.gameObject.SetActive(false);

        // Knowledge Stat Text
        KnowledgeStatText = Instantiate(KnowledgeStatTextPrefab);
        KnowledgeStatText.transform.SetParent(contentPanel, false);
        KnowledgeStatText.gameObject.SetActive(false);

        // HP Bar
        HPBar = Instantiate(HPBarPrefab);
        HPBar.transform.SetParent(contentPanel, false);
        HPBar.transform.gameObject.SetActive(false);

        // Will Stat Text
        WillStatText = Instantiate(WillStatTextPrefab);
        WillStatText.transform.SetParent(contentPanel, false);
        WillStatText.gameObject.SetActive(false);

        // Mage Saved Roll Text
        SavedRollText = Instantiate(SavedRollTextPrefab);
        SavedRollText.transform.SetParent(contentPanel, false);
        SavedRollText.gameObject.SetActive(false);

        // End Turn Button
        EndTurnButton = Instantiate(EndTurnButtonPrefab);
        EndTurnButton.transform.SetParent(contentPanel, false);
        EndTurnButton.onClick.AddListener(PressEndTurnButton);
        EndTurnButton.gameObject.SetActive(true);
        EndTurnButton.GetComponent<Image>().color = Color.red;

        // Player Money UI
        UpdateMoneyUI();

        // Money Button
        MoneyButton = Instantiate(MoneyButtonPrefab);
        MoneyButton.transform.SetParent(contentPanel, false);
        MoneyButton.onClick.AddListener(PressMoneyButton);
        MoneyButton.gameObject.SetActive(false);

        //HP Stat
        HPStatText = Instantiate(HPStatTextPrefab);
        HPStatText.transform.SetParent(contentPanel, false);
        HPStatText.gameObject.SetActive(false);

    }

    void PlaceCharacterOnTile(GameObject unit, int x, int y, int player)
    {
        // Instantiate an instance of the unit and place it on the given tile.
        GameObject newUnit = Instantiate(unit);
        newUnit.GetComponent<Character>().SetPlayer(player);
        newUnit.GetComponent<Character>().SetHPFull();
        mapArray[x, y].transform.GetComponent<TileBehavior>().PlaceUnit(newUnit);

        // Put the unit in the right player's array.
        if (player == 1)
        {
            player1Units.Add(newUnit);

        }
        else if (player == 2)
        {
            player2Units.Add(newUnit);
        }
    }
    #endregion

    #region Get grid height and width
    public int GetGridHeight()
    {
        return gridHeight;
    }

    public static int GetGridWidth()
    {
        return statGridWidth;
    }
    #endregion

    #region UI_functions
    public void ShowCharacterUI(GameObject selectedUnit)
    {
        attackButton.gameObject.SetActive(true);
        if (selectedUnit.GetComponent<Character>().GetCanAttack())
        {
            attackButton.GetComponent<ButtonSpriteHandler>().setDisabled(false);
        }
        else
        {
            attackButton.GetComponent<ButtonSpriteHandler>().setDisabled(true);
        }
        SpeedStatText.gameObject.SetActive(true);
        SpeedStatText.text = "Speed: " + selectedUnit.GetComponent<Character>().GetSpeed();

        MightStatText.gameObject.SetActive(true);
        MightStatText.text = "Might: " + selectedUnit.GetComponent<Character>().GetMight();

        /*
        KnowledgeStatText.gameObject.SetActive(true);
        KnowledgeStatText.text = "Know.: " + selectedUnit.GetComponent<Character>().GetKnowledge();

        WillStatText.gameObject.SetActive(true);
        WillStatText.text = "Will: " + selectedUnit.GetComponent<Character>().GetWill();

        HPStatText.gameObject.SetActive(true);
        HPStatText.text = "HP: " + selectedUnit.GetComponent<Character>().GetHP();
        */

        // HP Bar
        HPBar.gameObject.SetActive(true);
        int totalHP = selectedUnit.GetComponent<Character>().totalHealth;
        int currHP = selectedUnit.GetComponent<Character>().GetHP();
        HPBar.GetComponentInChildren<Text>().text = currHP + "/" + totalHP;
        HPBar.value = (float)currHP / totalHP;

        if (selectedUnit.GetComponent<Character>().GetName() == "Mage")
        {
            SavedRollText.gameObject.SetActive(true);
            SavedRollText.text = "Saved roll: " + selectedUnit.GetComponent<Mage>().GetPrevRoll();
            if (TileBehavior.selectedUnit.GetComponent<Mage>().CanUseAbil()
                && !TileBehavior.selectedUnit.GetComponent<Mage>().GetTurnDone())
            {
                abilityButton.gameObject.SetActive(true);
            }
            else if (TileBehavior.selectedUnit.GetComponent<Mage>().GetTurnDone())
            {
                SavedRollText.gameObject.SetActive(true);
                abilityButton.gameObject.SetActive(false);
            }
            else if (!TileBehavior.selectedUnit.GetComponent<Mage>().CanUseAbil()
                && !TileBehavior.selectedUnit.GetComponent<Mage>().GetTurnDone())
            {
                abilityButton.gameObject.SetActive(true);
                useRollButton.gameObject.SetActive(true);
            }

            if (selectedUnit.GetComponent<Character>().GetPlayer() != currentPlayer)
            {
                abilityButton.gameObject.SetActive(false);
                SavedRollText.gameObject.SetActive(true);
            }
        }
        else
        {
            abilityButton.gameObject.SetActive(false);
            SavedRollText.gameObject.SetActive(false);
            useRollButton.gameObject.SetActive(false);
        }

    }

    public void ShowMoneyButton()
    {
        MoneyButton.gameObject.SetActive(true);
    }

    public void ClearUI()
    {
        attackButton.gameObject.SetActive(false);
        MoneyButton.gameObject.SetActive(false);
        SpeedStatText.gameObject.SetActive(false);
        MightStatText.gameObject.SetActive(false);
        WillStatText.gameObject.SetActive(false);
        KnowledgeStatText.gameObject.SetActive(false);
        HPStatText.gameObject.SetActive(false);
        HPBar.gameObject.SetActive(false);
        abilityButton.gameObject.SetActive(false);
        SavedRollText.gameObject.SetActive(false);
        useRollButton.gameObject.SetActive(false);
    }

    public void PressAttackButton()
    {
        attackButton.enabled = false;
        attackButton.enabled = true;
        if (testSingleton == true)
        {
            return;
        }
        if (TileBehavior.selectedUnit != null && TileBehavior.selectedUnit.GetComponent<Character>().GetCanAttack())
        {
            TileBehavior.AttackSelection();
        }
    }

    public void PressAbilityButton()
    {
        if (testSingleton == true)
        {
            return;
        }
        if (TileBehavior.selectedUnit != null)
        {
            TileBehavior.selectedUnit.GetComponent<Mage>().Ability();
            TileBehavior.selectedUnit.GetComponent<Mage>().SetTurnDone(true);
            TileBehavior.selectedTile.GetComponent<TileBehavior>().SelectionStateToNull();
        }
    }

    public void PressUseRollButton()
    {
        if (TileBehavior.selectedUnit != null && !TileBehavior.selectedUnit.GetComponent<Mage>().CanUseAbil()
            && TileBehavior.selectedUnit.GetComponent<Character>().GetCanAttack())
        {
            TileBehavior.selectedUnit.GetComponent<Mage>().UsePrevRoll();
            TileBehavior.SetSelectionState("move");
            TileBehavior.selectedTile.GetComponent<TileBehavior>().SelectionStateToAttack();
        }
    }

    public void ChangeAbilBack()
    {
        abilityButton.GetComponent<Button>().GetComponentInChildren<Text>().text = "Save roll";
        SavedRollText.text = "Saved roll: " + TileBehavior.selectedUnit.GetComponent<Mage>().GetPrevRoll();
        abilityButton.gameObject.SetActive(false);
        attackButton.gameObject.SetActive(false);
    }

    public void PressHealButton()
    {
        if (testSingleton == true)
        {
            return;
        }
        if (TileBehavior.selectedUnit != null && TileBehavior.selectedUnit.GetComponent<Character>().GetCanAttack())
        {
            TileBehavior.selectedUnit.GetComponent<Character>().SelfHeal();
        }
    }

    public void PressEndTurnButton()
    {
        if (testSingleton == true || DiceAttack.GetComponent<DiceAttack>().IsRunning())
        {
            return;
        }

        if (currentPlayer == 1)
        {
            currentPlayer = 2;
            EndTurnButton.GetComponent<Image>().color = Color.blue;
            foreach (GameObject unit in player1Units)
            {
                unit.GetComponent<Character>().SetCanMove(true);
                unit.GetComponent<Character>().SetCanAttack(true);
            }
            foreach (GameObject unit in player2Units)
            {
                unit.GetComponent<Character>().SetCanMove(true);
                unit.GetComponent<Character>().SetCanAttack(true);
            }
        }
        else if (currentPlayer == 2)
        {
            currentPlayer = 1;
            EndTurnButton.GetComponent<Image>().color = Color.red;
            foreach (GameObject unit in player1Units)
            {
                unit.GetComponent<Character>().SetCanMove(true);
                unit.GetComponent<Character>().SetCanAttack(true);
            }
            foreach (GameObject unit in player2Units)
            {
                unit.GetComponent<Character>().SetCanMove(true);
                unit.GetComponent<Character>().SetCanAttack(true);
            }
        }

        if (TileBehavior.GetSelectionState() != null)
        {
            TileBehavior.selectedTile.GetComponent<TileBehavior>().SelectionStateToNull();
        }
        else
        {
            if (currentPlayer == 1)
            {
                foreach (GameObject unit in player1Units)
                {
                    if (unit.GetComponent<Character>().GetTurnsToRespawn() > 0)
                    {
                        unit.GetComponent<Character>().DecreaseTurnsToRespawn();
                        if (unit.GetComponent<Character>().GetTurnsToRespawn() == 0)
                        {

                            unit.GetComponent<Character>().canRespawn = true;
                            tempUnit = unit;
                        }
                    }
                    if (unit.GetComponent<Character>().GetName() == "Mage")
                    {
                        unit.GetComponent<Mage>().SetTurnDone(false);
                    }
                }
                mapArray[gridWidth - 1, 0].GetComponent<SpriteRenderer>().color = saveColor;
            }
            else if (currentPlayer == 2)
            {
                foreach (GameObject unit in player2Units)
                {
                    if (unit.GetComponent<Character>().GetTurnsToRespawn() > 0)
                    {
                        unit.GetComponent<Character>().DecreaseTurnsToRespawn();
                        if (unit.GetComponent<Character>().GetTurnsToRespawn() == 0)
                        {
                            unit.GetComponent<Character>().canRespawn = true;
                            tempUnit = unit;
                        }
                    }
                    if (unit.GetComponent<Character>().GetName() == "Mage")
                    {
                        unit.GetComponent<Mage>().SetTurnDone(false);
                    }
                }
                mapArray[0, gridHeight].GetComponent<SpriteRenderer>().color = saveColor;
            }
        }

        //CurrentPlayerText.text = "Player " + currentPlayer;
        gameTime += 1;
        Time.text = "Turn " + gameTime;
        Respawn();

        /*
        if (gameTime == 11)
        {
            mapArray[2, 1].GetComponent<TileBehavior>().SetMoneyTile(true);
            mapArray[10, 5].GetComponent<TileBehavior>().SetMoneyTile(true);
            mapArray[2, 1].GetComponent<Roulette>().SetPlayer(1);
            mapArray[10, 5].GetComponent<Roulette>().SetPlayer(2);
        }
        if (gameTime == 15)
        {
            mapArray[6, 3].GetComponent<TileBehavior>().SetMoneyTile(false);
        }
        */
    }

    public void OpenAllocation(GameObject attackingUnit, GameObject targetUnit, int damage)
    {
        targetUnit.GetComponent<Character>().HPDamage(damage);
        if (targetUnit.GetComponent<Character>().GetHP() <= 0)
        {
            turnCount = 1;
            Despawn(targetUnit);
        }
        //allocationUI.gameObject.SetActive(true);
        //allocationUI.GetComponent<Allocation>().startAllocation(targetUnit, damage, "physical");
    }

    public void UpdateMoneyUI()
    {
        Player1MoneyUI.GetComponentInChildren<Text>().text = "" + player1money;
        Player2MoneyUI.GetComponentInChildren<Text>().text = "" + player2money;
    }

    public void PressRespawnButton()
    {
        float tileSize = TileBehavior.tileDim;
        int count = 0;
        if (currentPlayer == 1)
        {
            if (mapArray[0, gridHeight].transform.GetComponent<TileBehavior>().HasUnit())
            {
            }
            else
            {
                foreach (GameObject unit in player1Units)
                {
                    if (unit.GetComponent<Character>().GetTurnsToRespawn() == 0)
                    {
                        count++;
                        if (!changed)
                        {
                            respawnButton.GetComponentInChildren<Text>().text = "Respawn " + unit.GetComponent<Character>().GetThemeName();
                            changed = true;
                            return;
                        }
                        else
                        {
                            unit.GetComponent<Character>().ResetStats();
                            unit.SetActive(true);
                            mapArray[0, gridHeight].transform.GetComponent<TileBehavior>().PlaceUnit(unit);
                            unit.GetComponent<Character>().DecreaseTurnsToRespawn();
                            changed = false;
                        }
                    }
                }
                mapArray[0, gridHeight].GetComponent<SpriteRenderer>().color = saveColor;
            }
        }
        else if (currentPlayer == 2)
        {
            if (mapArray[gridWidth - 1, 0].transform.GetComponent<TileBehavior>().HasUnit())
            {
            }
            else
            {
                foreach (GameObject unit in player2Units)
                {
                    count++;
                    if (unit.GetComponent<Character>().GetTurnsToRespawn() == 0)
                    {
                        if (!changed)
                        {
                            respawnButton.GetComponentInChildren<Text>().text = "Respawn " + unit.GetComponent<Character>().GetThemeName();
                            changed = true;
                            return;
                        }
                        else
                        {
                            unit.GetComponent<Character>().ResetStats();
                            unit.SetActive(true);
                            mapArray[gridWidth - 1, 0].transform.GetComponent<TileBehavior>().PlaceUnit(unit);
                            unit.GetComponent<Character>().DecreaseTurnsToRespawn();
                            changed = false;
                        }
                    }
                }
                mapArray[gridWidth - 1, 0].GetComponent<SpriteRenderer>().color = saveColor;
            }
        }
        if (count > 0)
        {
            respawnButton.gameObject.SetActive(false);
        }
    }

    public void PressMoneyButton()
    {
        if (TileBehavior.selectedUnit != null)
        {
            GameObject unit = TileBehavior.selectedUnit;
            bool canAttack = unit.GetComponent<Character>().GetCanAttack();
            //bool canMove = unit.GetComponent<Character>().GetCanMove();
            int unitPlayer = unit.GetComponent<Character>().GetPlayer();
            if (canAttack && currentPlayer == unitPlayer)
            {
                if (currentPlayer == 1)
                {
                    if (unit.GetComponent<Character>().GetOccupiedTile().GetComponent<TileBehavior>().playerside == 2)
                    {
                        player1money += 200;
                        audioSource.clip = coinSound;
                        audioSource.Play();
                    }
                    else if (unit.GetComponent<Character>().GetOccupiedTile().GetComponent<TileBehavior>().playerside == 1)
                    {
                        return;
                    }
                    else
                    {
                        player1money += 100;
                        audioSource.clip = coinSound;
                        audioSource.Play();

                    }
                    if (player1money >= winAmount)
                    {
                        winUI.gameObject.SetActive(true);
                    }
                    // Gambler ability
                    if (unit.GetComponent<Character>().GetName() == "Gambler")
                    {
                        player1money += 100;
                    }
                }
                else if (currentPlayer == 2)
                {
                    if (unit.GetComponent<Character>().GetOccupiedTile().GetComponent<TileBehavior>().playerside == 1)
                    {
                        player2money += 200;
                        audioSource.clip = coinSound;
                        audioSource.Play();
                    }
                    else if (unit.GetComponent<Character>().GetOccupiedTile().GetComponent<TileBehavior>().playerside == 2)
                    {
                        return;
                    }
                    else
                    {
                        player2money += 100;
                        audioSource.clip = coinSound;
                        audioSource.Play();
                    }
                    // Gambler ability
                    if (unit.GetComponent<Character>().GetName() == "Gambler")
                    {
                        player2money += 100;
                    }
                    if (player2money >= winAmount)
                    {
                        winUI.transform.GetChild(0).GetComponent<Text>().text = "Player 2 wins!";
                        winUI.gameObject.SetActive(true);
                    }
                }
                UpdateMoneyUI();
                unit.GetComponent<Character>().SetCanAttack(false);
                unit.GetComponent<Character>().SetCanMove(false);
                TileBehavior.selectedTile.GetComponent<TileBehavior>().SelectionStateToMove();
            }
        }
    }
    #endregion

    #region Spawning
    public void Despawn(GameObject character)
    {
        //TileBehavior.selectedTile.GetComponent<TileBehavior>().SetSelectedTile(character);
        character.GetComponent<Character>().SetTurnsToRespawn();
        character.GetComponent<Character>().GetOccupiedTile().GetComponent<TileBehavior>().ClearUnit();
        //character.SetActive(false);
    }

    public void Respawn()
    {
        if (currentPlayer == 1)
        {
            foreach (GameObject unit in player1Units)
            {
                if (unit.GetComponent<Character>().GetTurnsToRespawn() == 0)
                {
                    respawnButton.gameObject.SetActive(true);
                    respawnButton.GetComponentInChildren<Text>().text = "Respawn " + unit.GetComponent<Character>().GetThemeName();
                    saveColor = mapArray[0, gridHeight].GetComponent<SpriteRenderer>().color;
                    mapArray[0, gridHeight].GetComponent<SpriteRenderer>().color = Color.red;
                    changed = true;
                    break;
                }
                else
                {
                    respawnButton.gameObject.SetActive(false);
                }
            }
        }
        else if (currentPlayer == 2)
        {
            foreach (GameObject unit in player2Units)
            {
                if (unit.GetComponent<Character>().GetTurnsToRespawn() == 0)
                {
                    respawnButton.gameObject.SetActive(true);
                    respawnButton.GetComponentInChildren<Text>().text = "Respawn " + unit.GetComponent<Character>().GetThemeName();
                    saveColor = mapArray[gridWidth - 1, 0].GetComponent<SpriteRenderer>().color;
                    mapArray[gridWidth - 1, 0].GetComponent<SpriteRenderer>().color = Color.blue;
                    changed = true;
                    break;
                }
                else
                {
                    respawnButton.gameObject.SetActive(false);
                }
            }
        }

    }
    #endregion

    public void ResetUIAfterAbil(GameObject unit)
    {
        unit.GetComponent<Mage>().SetAbilRoll(0);
        SavedRollText.text = "Saved roll: " + unit.GetComponent<Mage>().GetPrevRoll();
        abilityButton.GetComponent<Button>().GetComponentInChildren<Text>().text = "Save roll";
        abilityButton.gameObject.SetActive(false);
        attackButton.gameObject.SetActive(false);
    }
}
