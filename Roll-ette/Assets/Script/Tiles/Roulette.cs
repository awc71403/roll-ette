using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Roulette : TileBehavior
{
    [SerializeField]
    Sprite player1Roulette;

    [SerializeField]
    Sprite player2Roulette;

    //different variables
    private int roulettemovement = 1;


    // Start is called before the first frame update
    void Awake()
    {
        movementCost = roulettemovement;
        tileType = "roulette";
        SetMoneyTile(true);
        isroulette = 1;
    }

    // 0 for center, 1 for player1, 2 for player2
    public void SetPlayer(int x)
    {
        playerside = x;
        if (x == 1)
        {
            moneyTileMarker.GetComponent<SpriteRenderer>().sprite = player1Roulette;
        }
        else if (x == 2)
        {
            moneyTileMarker.GetComponent<SpriteRenderer>().sprite = player2Roulette;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

}
