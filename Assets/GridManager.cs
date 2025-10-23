using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using System.Linq;

public class GridManager : MonoBehaviour
{
    public int gridWidth;
    public int gridHeight;
    private Tile [,] grid;

    public GameObject tilePrefab;


    // Start is called before the first frame update
    void Start()
    {
        grid = new Tile[gridWidth, gridHeight];
        for(int x=0; x < gridWidth; x++)
        {
            for(int y=0; y < gridHeight; y++)
            {
                grid[x,y] = null;
            }
        }
        for(int x=0; x < 4; x++)
        {
            for(int y=0; y < gridHeight; y++)
            {
                CreateTile(grid, x,y);
            }
        }
    }
    public void CreateTile(Tile[,] grid, int x, int y)
    {
        GameObject tileGO = Instantiate(tilePrefab,new Vector2(x,y),Quaternion.identity);
        tileGO.transform.SetParent(this.transform);
        tileGO.name = $"Tile";

        Tile tile = tileGO.GetComponent<Tile>();
        tile.SetElement(RNG.RandomElement());
        tile.Position = ((x,y));
        grid[x,y] = tile;
    }


    public bool ValidMovement((int x, int y) pos)
    {
        return pos.x >= 0 && pos.x < gridWidth && pos.y >= 0 && pos.y < gridHeight;
    }

    public void SwapTiles((int x, int y) left, (int x, int y) right)
    {
        Tile leftTile = GetTile(left);
        Tile rightTile = GetTile(right);

        SetNull(left);
        SetNull(right);

        SetTile(left, rightTile);
        SetTile(right, leftTile);

        //match NEED TO JUST SEARCH EVERY SPOT IN GRID TO FIND COMPLETE MATCHES
        HashSet<(int x, int y)> leftFallTiles = CompleteMatches(left);
        HashSet<(int x, int y)> rightFallTiles = CompleteMatches(right);

        //fall
        Fall();
        // Fall(leftFallTiles);
        // Fall(rightFallTiles);



    }

#region BasicTileFunctions


    public Tile GetTile((int x, int y) position)
    {   
        if (position.x < 0 || position.x >= gridWidth || position.y < 0 || position.y >= gridHeight)
            return null;

        return grid[position.x, position.y];
    }

    public void SetNull((int x, int y) position)
    {
        grid[position.x, position.y] = null;
    }

    public void SetTile((int x, int y) position, Tile tile)
    {
        if(tile != null)
        {
            grid[position.x,position.y] = tile;
            tile.Position = position;
        }
    }

    public void DestroyTile((int x, int y) position)
    {
        Tile tile = GetTile(position);

        if(tile != null) tile.DestroyMe();
        grid[position.x,position.y] = null;
    }
#endregion
#region MatchLogic
    HashSet<(int x, int y)> GetAllMatches((int x, int y) position)
    {
        if(GetTile(position) == null)
          return new HashSet<(int x, int y)>();

        HashSet<(int x, int y)> allMatches = new HashSet<(int x, int y)>();
        HashSet<(int x, int y)> verticalMatches = new HashSet<(int x, int y)>();

        GetMatchesInDirection(position, Vector2.up, verticalMatches);
        GetMatchesInDirection(position, Vector2.down, verticalMatches);

        if(verticalMatches.Count >= 2)
        {
            allMatches.UnionWith(verticalMatches);
        }

        HashSet<(int x, int y)> horizontalMatches = new HashSet<(int x, int y)>();
        GetMatchesInDirection(position, Vector2.right, horizontalMatches);
        GetMatchesInDirection(position, Vector2.left, horizontalMatches);

        if(horizontalMatches.Count >= 2)
        {
            allMatches.UnionWith(horizontalMatches);
        }

        if(allMatches.Count >= 2)
        {
            allMatches.Add(position);
        }
        return allMatches;
    }

    void GetMatchesInDirection((int x, int y) prevPosition, Vector2 direction, HashSet<(int x, int y)> matches)
    {

        (int x, int y) position = ((prevPosition.x + (int)direction.x), (prevPosition.y + (int)direction.y));
        if(position.x < 0 || position.y < 0 || position.x >= gridWidth || position.y >= gridHeight || GetTile(position) == null || GetTile(position).GetElement() != GetTile(prevPosition).GetElement())
            return;

        matches.Add(position);
        GetMatchesInDirection(position, direction, matches);
    }

    HashSet<(int x, int y)> CompleteMatches((int x, int y) position)
    {
        HashSet<(int x, int y)> tiles = GetAllMatches(position);
       
        HashSet<(int x, int y)> fallTiles = new HashSet<(int x, int y)>();

        foreach((int x, int y) pos in tiles)
        {
            (int x, int y) newPos = (pos.x, pos.y + 1);
            if(newPos.y < gridHeight && !tiles.Contains(newPos))
            {
                fallTiles.Add(newPos);
            }
        }

        foreach((int x, int y) tilePosition in tiles)
        {
            DestroyTile(tilePosition);
        }
        
        return fallTiles;
    }
#endregion
#region TileLogic
    //If no tile exist, fall tiles above
    //If tile exist, fall tile
    // void TileFallLogic((int x, int y) position)
    // {
    //     if(GetTile(position) == null)
    //     {
    //         Fall((position.x,position.y+1));
    //     }
    //     else
    //     {
    //         if(WillFall(position))
    //         {
    //             Fall(position);
    //         }
    //     }
    // }
    //Get All the tiles above that will need to fall
    List<(int x, int y)> GetTilesAbove((int x, int y) position)
    {
        List<(int x, int y)> tiles = new List<(int x, int y)>();
        (int x, int y) above = (position.x, position.y + 1);
        while(above.y < gridHeight && GetTile(above) != null)
        {
            tiles.Add(above);
            above.y += 1;
        }
        return tiles;
    }
    
    // //Determine if when swapped, a piece will fall
    // bool WillFall((int x, int y) position)
    // {
    //     return GetValidPositionBelow(position) == position ? false : true;
    // }

    //Return the lowest not occupied position
    (int x, int y) GetValidPositionBelow((int x, int y) position)
    {
        (int x, int y) below = (position.x, position.y - 1);
        if(below.y < 0 || GetTile(below) != null)
        {
            return position;
        }
        return GetValidPositionBelow(below);
    } 

    void Fall()
    {
        //Check every spot in the grid for an empty tile
        //Check above to see if any tiles need to fall
        for(int x=0; x < gridWidth; x++)
        {
            for(int y=0; y < gridHeight; y++)
            {
                if(GetTile((x,y)) == null && GetTile((x,y+1)) != null)
                {
                    (int x, int y) lowestPosition = GetValidPositionBelow((x,y+1));
                    Tile tile = GetTile((x,y+1));
                    SetNull((x,y+1));
                    SetTile(lowestPosition, tile);
                } 
            }
        }
    }
    // void Fall(HashSet<(int x, int y)> fallTiles)
    // {   
    //     foreach((int x, int y) ftPosition in fallTiles)
    //     {
    //         List<(int x, int y)> tilesAbove = new List<(int x, int y)>(GetTilesAbove(ftPosition));

    //         foreach((int x, int y) taPosition in tilesAbove)
    //         {
    //             (int x, int y) lowestPosition = GetValidPositionBelow(taPosition);
    //             Tile tile = GetTile(taPosition);
    //             SetNull(taPosition);
    //             SetTile(lowestPosition, tile);
    //         }
    //     }
    // }
#endregion
}
