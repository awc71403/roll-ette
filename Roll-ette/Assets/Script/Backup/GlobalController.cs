using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        _map = new Map(5, 5);
        Sprite sprite = Resources.Load<Sprite>("AssetsSprites/grass_2");
        Sprite sprite1 = Resources.Load<Sprite>("Sprites/grass_2_highlight");
        float tileSizeX = sprite.bounds.size.x;
        float tileSizeY = sprite.bounds.size.y;
        for (int i = 0; i < 5; i++)
        {
            for (int j = 0; j < 5; j++)
            {
                GameObject go = _map.getTile(i, j).getTileObject();
                Vector3 worldStart = Camera.main.ScreenToWorldPoint(new Vector3(0, 0));
                _map.getTile(i, j).setSprite(sprite);
                _map.getTile(i, j).addHighlightSprite(sprite1);
                go.transform.position = new Vector3(worldStart.x + tileSizeX * i, worldStart.y + tileSizeY * j, 0);
            }
        }
        _map.connectAllAdjacent(2);
        _map.setCost(2,2,2,3,3);
        _map.highlightAll(_map.reachTo(_map.getTile(2, 2), 4));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private Map _map;
}
