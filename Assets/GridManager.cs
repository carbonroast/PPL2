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
        for(int x=0; x < gridWidth; x++)
        {
            for(int y=0; y < gridHeight; y++)
            {
                CreateTile(grid, x,y);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void CreateTile(GameObject[,] grid, int x, int y)
    {
        GameObject tile = Instantiate(tilePrefab,new Vector2(x,y),Quaternion.identity);
        tile.transform.SetParent(this.transform);
        tile.name = $"Tile: {x} - {y}";
        tile.GetComponent<Tile>().SetElement(RNG.RandomElement());
        tile.GetComponent<Tile>().Position = ((x,y));
        grid[x,y] = tile;
    }


    public bool ValidMovement((int x, int y) pos)
    {
        return pos.x >= 0 && pos.x < gridWidth && pos.y >= 0 && pos.y < gridHeight;
    }

    public void SwapTiles((int x, int y) left, (int x, int y) right)
    {
        GameObject leftTile = GetTile(left);
        GameObject rightTile = GetTile(right);

        SetTile(left, rightTile);
        SetTile(right, leftTile);
    }

    public GameObject GetTile((int x, int y) position)
    {
        return grid[position.x,position.y];
    }

    public void SetTile((int x, int y) position, GameObject tile)
    {
        grid[position.x,position.y] = tile;
        tile.GetComponent<Tile>().Position = position;
    }
}
