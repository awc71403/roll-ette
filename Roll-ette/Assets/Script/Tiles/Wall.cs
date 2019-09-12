using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Wall : TileBehavior
{
    //different variables
    private int wallmovement = 99;
    private int moneybool = 0;


    // Start is called before the first frame update
    void Awake()
    {
        movementCost = wallmovement;
        tileType = "wall";
        isroulette = moneybool;
    }

    // Update is called once per frame
    void Update()
    {

    }

}