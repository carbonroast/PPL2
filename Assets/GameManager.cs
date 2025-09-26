using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject tilePrefab;
    [SerializeField] private int gridWidth, gridHeight;
    private PlayerInputManager manager;
    private List<GameObject> players = new List<GameObject>();
    private int playerCount;

    void Awake()
    {
        manager = GetComponent<PlayerInputManager>();
        manager.onPlayerJoined += OnPlayerJoined;
        manager.onPlayerLeft += OnPlayerLeft;
    }
    // Start is called before the first frame update
    void Start()
    {
        CreateGrid();

        
    }

    void Update()
    {

        if(Input.GetKeyDown(KeyCode.Y))
        {
            KickPlayer();
        }
    }

    void AddPlayer()
    {
        PlayerInput newPlayer = manager.JoinPlayer();
        if (newPlayer != null)
        {
            players.Add(newPlayer.gameObject);
        }
        else
        {
            Debug.LogError("JoinPlayer failed — check PlayerInputManager Player Prefab!");
        }
    }

    void KickPlayer()
    {
        if(players.Count > 0)
        {
            Destroy(players[players.Count-1].gameObject);
            players.RemoveAt(players.Count -1);
        }

    }
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        playerCount++;
        Debug.Log($"Player Count - {playerCount}."); 
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        playerCount--;
        Debug.Log($"Player Count - {playerCount}."); 

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
