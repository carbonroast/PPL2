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
        GameObject playerGO = new GameObject("Player");
        GameObject p1 = Instantiate(playerPrefab,Vector2.zero, quaternion.identity);
        GameObject p2 = Instantiate(playerPrefab, Vector2.right, quaternion.identity);
        p1.transform.SetParent(playerGO.transform);
        p2.transform.SetParent(playerGO.transform);
        p1.GetComponent<PlayerTileSelector>().SetPosition((0,0));
        p2.GetComponent<PlayerTileSelector>().SetPosition((1,0));

        playerGO.AddComponent<PlayerController>();

        GameObject gridGO = new GameObject("GridManager");
        gridGO.AddComponent<GridManager>();
        gridGO.GetComponent<GridManager>().tilePrefab = tilePrefab;
        gridGO.GetComponent<GridManager>().gridWidth = gridWidth;
        gridGO.GetComponent<GridManager>().gridHeight = gridHeight;
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
