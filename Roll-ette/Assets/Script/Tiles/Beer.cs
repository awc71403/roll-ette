using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Beer : TileBehavior
{
    //different variables
    private int beermovement = 2;
    private int moneybool = 0;


    // Start is called before the first frame update
    void Awake()
    {
        movementCost = beermovement;
        tileType = "beer";
        isroulette = moneybool;
    }

    // Update is called once per frame
    void Update()
    {

    }

}
