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
    [SerializeField] Camera cam;

    private PlayerInputManager manager;
    private List<GameObject> players = new List<GameObject>();
    private int playerCount;

    void Awake()
    {
        manager = GetComponent<PlayerInputManager>();
        manager.onPlayerJoined += OnPlayerJoined;
        manager.onPlayerLeft += OnPlayerLeft;
        AddPlayer();
    }
    // Start is called before the first frame update
    void Start()
    {
        Vector2 screen = GetScreenSize();
        float tile_offset = .5f;
        float screen_left = -screen.x / 2f;
        float middle_section = 5f;

        float edge_offset = (screen.x - 2*gridWidth - middle_section)/2 ;

        float bottom = (-screen.y / 2f) + .5f;

        CreateGrid(screen_left + edge_offset + tile_offset, bottom);
        CreateGrid(middle_section/2 + tile_offset, bottom);

        
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

    Vector2 GetScreenSize()
    {
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;
        return new Vector2(width, height);
    }

    void CreateGrid(float xOffset, float yOffset)
    {

        GameObject gridGO = new GameObject("GridManager");
        gridGO.transform.position = new Vector2(xOffset, yOffset);

        GridManager gm = gridGO.AddComponent<GridManager>();
        gm.tilePrefab = tilePrefab;
        gm.gridWidth = gridWidth;
        gm.gridHeight = gridHeight;
    }


}
