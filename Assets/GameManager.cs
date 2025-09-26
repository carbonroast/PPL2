using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject tilePrefab;
    [SerializeField] private int gridWidth, gridHeight;
    // Start is called before the first frame update
    void Start()
    {
        CreatePlayer("Player");
        CreateGrid();

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CreatePlayer(string name)
    {
        Instantiate(playerPrefab,Vector2.zero, Quaternion.identity);
    }

    void CreateGrid()
    {
        GameObject gridGO = new GameObject("GridManager");
        gridGO.AddComponent<GridManager>();
        gridGO.GetComponent<GridManager>().tilePrefab = tilePrefab;
        gridGO.GetComponent<GridManager>().gridWidth = gridWidth;
        gridGO.GetComponent<GridManager>().gridHeight = gridHeight;
    }
}
