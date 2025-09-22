using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    public int gridWidth;
    public int gridHeight;
    private GameObject [,] grid;

    public GameObject tilePrefab;

    // Start is called before the first frame update
    void Start()
    {
        grid = new GameObject[gridWidth, gridHeight];
        int i=0;
        for(int x=0; x < gridWidth; x++)
        {
            for(int y=0; y < gridHeight; y++)
            {
                 GameObject tile = Instantiate(tilePrefab,new Vector2(x,y),Quaternion.identity);
                 tile.transform.SetParent(this.transform);
                 tile.name = $"Tile: {i++}";
                 tile.GetComponent<Tile>().SetPosition((x,y));
                 grid[x,y] = tile;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool ValidMovement(Vector2 move)
    {
        if(move.x < 0 || move.x > gridWidth -1 || move.y < 0 || move.y > gridHeight -1)
            return false;
        return true;
    }

    public void SwapTiles(GameObject selectorLeft, GameObject selectorRight)
    {

        (int x, int y) leftSelectorPosition = selectorLeft.GetComponent<PlayerTileSelector>().Position;
        (int x, int y) rightSelectorPosition = selectorRight.GetComponent<PlayerTileSelector>().Position;
        
        GameObject leftTile = GetTile(leftSelectorPosition);
        GameObject rightTile = GetTile(rightSelectorPosition);

        SetTile(leftSelectorPosition, rightTile);
        SetTile(rightSelectorPosition, leftTile);
    }

    public GameObject GetTile((int x, int y) position)
    {
        return grid[position.x,position.y];
    }

    public void SetTile((int x, int y) position, GameObject tile)
    {
        grid[position.x,position.y] = tile;
        tile.GetComponent<Tile>().SetPosition(position);
    }
}
