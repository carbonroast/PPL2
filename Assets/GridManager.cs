using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
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

    public async Task SwapTilesAsync((int x, int y) left, (int x, int y) right)
    {
        Tile leftTile = GetTile(left);
        Tile rightTile = GetTile(right);

        SetNull(left);
        SetNull(right);

        SetTile(left, rightTile);
        SetTile(right, leftTile);

        HashSet<(Tile, (int x, int y))> tilesToMove = new HashSet<(Tile, (int x, int y))>();

        if(leftTile != null) tilesToMove.Add((leftTile, right));
        if(rightTile != null) tilesToMove.Add((rightTile, left));

        await MoveTilesAsync(tilesToMove);

        await Task.Delay(500);

        HashSet<(int x, int y)> tilesPositionToDestroy = new HashSet<(int x, int y)>();
        if (leftTile == null || rightTile == null)
        {
            HashSet<(Tile, (int x, int y))> fallTiles = Fall();
            await MoveTilesAsync(fallTiles);
            await Task.Delay(500);
            foreach ((Tile, (int x, int y) position) tile in fallTiles)
            {
                tilesPositionToDestroy.UnionWith(GetAllMatches(tile.position));
            }
        }
        else
        {

            tilesPositionToDestroy.UnionWith(GetAllMatches(left));
            tilesPositionToDestroy.UnionWith(GetAllMatches(right));
        }



        while (tilesPositionToDestroy.Count > 0)
        {
            await DestroyTilesAsync(tilesPositionToDestroy);
            tilesPositionToDestroy.Clear();

            await Task.Delay(500);

            HashSet<(Tile, (int x, int y))> fallTiles = Fall();
            await MoveTilesAsync(fallTiles);

            await Task.Delay(500);

            foreach ((Tile,(int x, int y)position) tile in fallTiles)
            {
                tilesPositionToDestroy.UnionWith(GetAllMatches(tile.position));
            }
        }
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
        }
    }
    private async Task MoveTilesAsync(HashSet<(Tile tile, (int x, int y) position)> tilesToMove)
    {
        List<Task> tasks = new List<Task>();

        foreach((Tile tile, (int x, int y) position) movement in tilesToMove)
        {
            tasks.Add(movement.tile.MoveMeAsync(movement.position));

        }

        await Task.WhenAll(tasks);
    }

    private async Task DestroyTilesAsync(HashSet<(int x, int y)> tilesPosition)
    {
        List<Task> tasks = new List<Task>();
        foreach ((int x, int y) position in tilesPosition)
        {
            Tile tile = GetTile(position);
            if (tile != null)
            {
                tasks.Add(tile.DestroyMeAsync());
                SetNull(position);
            }

        }
        await Task.WhenAll(tasks);
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


#endregion
#region TileLogic

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

    private HashSet<(Tile, (int x, int y))> Fall()
    {
        HashSet<(Tile ,(int x, int y))> fallTiles = new HashSet<(Tile,(int x, int y))>();
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
                    fallTiles.Add((tile, lowestPosition));
                    SetNull((x,y+1));
                    SetTile(lowestPosition, tile);
                } 
            }
        }
    return fallTiles;
    }


#endregion
}
