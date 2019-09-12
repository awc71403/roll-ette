using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile

{
    [SerializeField]
    private GameObject tile;
    private SpriteRenderer renderer;
    private Sprite sprite;
    private Sprite highlightSprite;

    public Tile()
    {
        _id = "test";
        _x = -1;
        _y = -1;
        _mark = -1;
        _adjacent = new Dictionary<Tile, int>();
        initializeGameObject();
    }

    public Tile(string id, int x, int y)
    {
        _id = id;
        _x = x;
        _y = y;
        _mark = -1;
        _adjacent = new Dictionary<Tile, int>();
        initializeGameObject();
    }
    
    /* Initializes the GameObject. */
    private void initializeGameObject()
    {
        tile = new GameObject(getID());
        renderer = tile.AddComponent<SpriteRenderer>();
    }

    /* Returns the GameObject associated with this tile. */
    public GameObject getTileObject()
    {
        return tile;
    }

    /* Returns the SpriteRenderer of this tile. */
    public SpriteRenderer getRenderer()
    {
        return renderer;
    }

    /* Returns the sprite of this tile.
     * null if this tile does not have a sprite. */
    public Sprite getSprite()
    {
        return sprite;
    }

    /* Sets Sprite SP to sprite of this tile. */
    public void addSprite(Sprite sp)
    {
        sprite = sp;
    }

    /* Sets Sprite SP to sprite of this tile and set it to the current sprite. */
    public void setSprite(Sprite sp)
    {
        sprite = sp;
        renderer.sprite = sp;
    }

    /* Sets sprite at LOCATION to sprite of this tile and set it to the current sprite.
     * The directory for the LOCATION is Resources folder.*/
    public void setSprite(string location)
    {
        sprite = Resources.Load<Sprite>(location);
        renderer.sprite = sprite;
    }

    /* Returns the sprite of this tile.
     * null if this tile does not have a sprite. */
    public Sprite getHighlightSprite()
    {
        return highlightSprite;
    }

    /* Sets Sprite SP to sprite of this tile. */
    public void addHighlightSprite(Sprite sp)
    {
        highlightSprite = sp;
    }

    /* Sets sprite at LOCATION to sprite of this tile.
     * The directory for the LOCATION is Resources folder.*/
    public void setHighlightSprite(string location)
    {
        highlightSprite = Resources.Load<Sprite>(location);
    }

    /* Highlights this tile.
     * Assumes that highlightSprite exists. */
    public void highlight()
    {
        renderer.sprite = highlightSprite;
    }

    /* Removes highlight on this tile.
     * Assumes that sprite exists. */
    public void removeHighlight()
    {
        renderer.sprite = sprite;
    }

    /* Sets the identifier of this tile to ID. */
    public void setID(string id)
    {
        _id = id;
    }

    /* Returns the identifier of this tile. */
    public string getID()
    {
        return _id;
    }

    /* Returns HashSet of Tile that are adjacent to this tile. */
    public HashSet<Tile> getAllAdjacent()
    {
        HashSet<Tile> tS = new HashSet<Tile>();
        foreach (Tile t in _adjacent.Keys)
        {
            tS.Add(t);
        }
        return tS;
    }
    
    /* Adds Tile T with cost C to the list of adjacent tiles.
     * If Tile t is already adjacent, set its cost to C.
     * C is 1 by default. */
    public void addAdjacent(Tile t, int c = 1)
    {
        if (hasAdjacentTile(t))
        {
            setCostTo(t, c);
        } else
        {
            _adjacent.Add(t, c);
        }
    }

    /* Deletes Tile T from the list of adjacent tiles.
     * Does nothing if T is not adjacent.*/
    public void deleteAdjacent(Tile t)
    {
        _adjacent.Remove(t);
    }

    /* Returns true if Tile T is adjacent. */
    public bool hasAdjacentTile(Tile t)
    {
        return _adjacent.ContainsKey(t);
    }

    /* Sets the cost of moving to Tile T as C.
     * Does not do anything if T is not adjacent. */
    public void setCostTo(Tile t, int c)
    {
        if (hasAdjacentTile(t))
        {
            _adjacent[t] = c;
        }
    }

    /* Get cost to move to Tile T with Character C.
     * Assumes that T is adjacent.
     * C is null by default. */
    public int getCost(Tile t, Character c = null)
    {
        return _adjacent[t];
    }

    /* Returns true if there is a character in this tile. */
    public bool hasCharacter()
    {
        return _occu != null;
    }

    /* Assigns character C to this tile. */
    public void setCharacter(Character c)
    {
        _occu = c;
    }

    /* Returns the character in this tile.
     * Null if there is no character in this tile.*/
    public Character getCharacter()
    {
        return _occu;
    }

    /* Deletes the character in this tile. */
    public void deleteCharacter()
    {
        _occu = null;
    }

    /* Sets x-position of this tile to X. */
    public void setX(int x)
    {
        _x = x;
    }

    /* Sets y-position of this tile to Y. */
    public void setY(int y)
    {
        _y = y;
    }

    /* Returns the x-position of this tile. */
    public int getX()
    {
        return _x;
    }

    /* Returns the y-position of this tile. */
    public int getY()
    {
        return _y;
    }

    /* Returns the mark on this tile. */
    public int getMark()
    {
        return _mark;
    }

    /* Marks this tile. IT HAS NOTHING TO DO WITH HIGHTLIGHTING. */
    public void mark(int n)
    {
        _mark = n;
    }

    /* Unmarks this tile. IT HAS NOTHING TO DO WITH HIGHTLIGHTING. */
    public void unmark()
    {
        _mark = -1;
    }

    /* Identifier assigned to this tile. */
    private string _id;
    /* x-position of this tile. */
    private int _x;
    /* y-position of this tile. */
    private int _y;
    /* A dictionary of adjacent tiles.
     * Keys are Tile and values are int. */
    private Dictionary<Tile, int> _adjacent;
    /* The character on this tile. */
    private Character _occu;
    /* Mark on this tile that is used for function traverse in Map.
     * -1 if unmarked.
     * IT HAS NOTHING TO DO WITH HIGHTLIGHTING. */
    private int _mark;
}
