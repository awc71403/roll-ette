using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WoodenPlank : TileBehavior
{
    //different variables
    private int plankmovement = 1;
    private int moneybool = 0;


    // Start is called before the first frame update
    void Awake()
    {
        movementCost = plankmovement;
        tileType = "wood";
        isroulette = moneybool;
    }

    public void SetPlayer(int x)
    {
        playerside = x;
    }

    // Update is called once per frame
    void Update()
    {

    }

}
