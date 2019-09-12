using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* Map. */
public class Map
{

    /* Default size of the board.
     *  0th element is the width and 1st element is the height of the board. */
    private int[] DEFAULTSIZE = { 18, 10 };

    /* Initializes the board. */
    public Map()
    {
        _sizeX = DEFAULTSIZE[0];
        _sizeY = DEFAULTSIZE[1];
        instantiateBoard();
    }

    /* Initializes the board. */
    public Map(int sizeX, int sizeY)
    {
        _sizeX = sizeX;
        _sizeY = sizeY;
        instantiateBoard();
    }

    /* Initializes the board. */
    public Map(string board)
    {

    }

    /* Creates a directed edge between all adjacent tiles with cost COST. */
    public void connectAllAdjacent(int cost)
    {
        for (int i = 0; i < sizeX() - 1; i++)
        {
            for (int j = 0; j < sizeY(); j++)
            {
                if (getTile(i, j) != null && getTile(i + 1, j) != null)
                {
                    setUndirectedPath(getTile(i, j), getTile(i + 1, j), cost);
                }
            }
        }
        for (int i = 0; i < sizeX(); i++)
        {
            for (int j = 0; j < sizeY() - 1; j++)
            {
                if (getTile(i, j) != null && getTile(i, j + 1) != null)
                {
                    setUndirectedPath(getTile(i, j), getTile(i, j + 1), cost);
                }
            }
        }
    }

    /* Fills up the board with empty tiles. */
    public void instantiateBoard()
    {
        _board = new Tile[_sizeX][];
        _tiles = new HashSet<Tile>();
        for (int i = 0; i < sizeX(); i++)
        {
            Tile[] t = new Tile[sizeY()];
            for (int j = 0; j < sizeY(); j++)
            {
                Tile tile = new Tile("test" + i + j, i, j);
                t[j] = tile;
                _tiles.Add(tile);
            }
            _board[i] = t;
        }
    }

    /* Returns the width of the board. */
    public int sizeX()
    {
        return _sizeX;
    }

    /* Returns the height of the board. */
    public int sizeY()
    {
        return _sizeY;
    }

    /* Returns the tile at (X, Y) position. */
    public Tile getTile(int x, int y)
    {
        if (x < _sizeX && y < _sizeY)
        {
            return _board[x][y];
        }
        else
        {
            return null;
        }
    }

    /* Sets the (X, Y) position to the Tile T. */
    public void setTile(int x, int y, Tile t)
    {
        if (x < _sizeX && y < _sizeY)
        {
            _board[x][y] = t;
        }
    }

    /* Creates a directed path from Tile T1 to Tile T2 with cost C if they are present in the board. */
    public void setDirectedPath(Tile t1, Tile t2, int c)
    {
        if (_tiles.Contains(t1) && _tiles.Contains(t2))
        {
            t1.addAdjacent(t2, c);
        }
    }

    /* Creates an undirected path between Tile T1 and Tile T2 with cost C if they are present in the board. */
    public void setUndirectedPath(Tile t1, Tile t2, int c)
    {
        if (_tiles.Contains(t1) && _tiles.Contains(t2))
        {
            t1.addAdjacent(t2, c);
            t2.addAdjacent(t1, c);
        }
    }

    /* Sets the cost from Tile at position (X1, Y1) to Tile at position (X2, Y2) to C.
     * Assumes that correponding Tile exist. */
    public void setCost(int x1, int y1, int x2, int y2, int c)
    {
        getTile(x1, y1).setCostTo(getTile(x2, y2), c);
    }

    /* Sets the cost from Tile T1 to Tile T2 to C.
     * Assumes that T1 and T2 exist.
     * Assumes that there is an adjacent path from T1 to T2. */
    public void setCost(Tile T1, Tile T2, int c)
    {
        T1.setCostTo(T2, c);
    }

    /* Highlights all Tile in HashSet TS.
     * Assumes that highlightSprite exists in all Tile.*/
    public void highlightAll(HashSet<Tile> tS)
    {
        foreach (Tile t in tS)
        {
            t.highlight();
        }
    }

    /* Removes highlight in all Tile in HashSet TS.
     * Assumes that sprite exists in all Tile.*/
    public void removeHighlightAll(HashSet<Tile> tS)
    {
        foreach (Tile t in tS)
        {
            t.removeHighlight();
        }
    }

    /* Unmarks all Tile in HashSet TS. */
    private void unmarkAll(HashSet<Tile> tS)
    {
        foreach (Tile t in tS)
        {
            t.unmark();
        }
    }

    /* Returns a HashSet of Tile containing all Tile that can be reached from Tile ORIGIN with NUM movements.
     * If IGNORECOST is True, then fix the cost to move to 1.
     * Takes movement as Character MYTYPE if specified.
     * IGNORECOST is False default and MYTYPE is null default.
     * Assumes that all Tile are unmarked.*/
    public HashSet<Tile> reachTo(Tile origin, int num, bool ignoreCost = false, Character myType = null)
    {
        HashSet<Tile> result = new HashSet<Tile>();
        traverse(origin, num, ignoreCost, result, myType);
        unmarkAll(result);
        return result;
    }

    /* Helper funcion for reachTo that recursively traverses adjacent tiles from CURRENT.
     * Takes in Tile CURRENT to begin from, NUM movements, IGNORECOST, and SAVES that keeps track of Tile reached. */
    private void traverse(Tile current, int num, bool ignoreCost, HashSet<Tile> saves, Character myType)
    {
        current.mark(num);
        saves.Add(current);
        foreach (Tile next in current.getAllAdjacent()) {
            int costTo = current.getCost(next, myType);
            if (costTo != -1)
            {
                int numAfter = num - ((ignoreCost) ? 1 : costTo);
                if (next.getMark() < numAfter)
                {
                    traverse(next, numAfter, ignoreCost, saves, myType);
                }
            }
        }
    }

    /* A 2D list of tiles. */
    private Tile[][] _board;
    /* Set of tiles associated with the board. */
    private HashSet<Tile> _tiles;
    /* Width of the game board. */
    private int _sizeX;
    /* Height of the game board. */
    private int _sizeY;
}
