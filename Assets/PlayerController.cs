using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    GameObject grid;
    GameObject selectorLeft;
    GameObject selectorRight;
    PlayerInputActions playerInputActions;

    // Start is called before the first frame update
    void Awake()
    {
        playerInputActions = new PlayerInputActions();        
    }

    void OnEnable()
    {
        playerInputActions.Player.Move.performed += Movement;
        playerInputActions.Player.Move.Enable();

        playerInputActions.Player.Swap.performed += Swap;
        playerInputActions.Player.Swap.Enable();
    }

    void OnDisable()
    {
        playerInputActions.Player.Move.Disable();
        playerInputActions.Player.Swap.Disable();
    }

    void Start()
    {
        grid = GameObject.Find("GridManager"); 
        selectorLeft = this.transform.GetChild(0).gameObject;
        selectorRight = this.transform.GetChild(1).gameObject;
    }

    // Update is called once per frame
    void Update()
    {
    }

    void Movement(InputAction.CallbackContext obj)
    {
        Debug.Log(obj);
        // Vector2 movement = Vector2.zero;
        // if (Input.GetKeyDown(KeyCode.W))
        //     movement.y += 1;
        // if (Input.GetKeyDown(KeyCode.S))
        //     movement.y -= 1;
        // if (Input.GetKeyDown(KeyCode.A))
        //     movement.x -= 1;
        // if (Input.GetKeyDown(KeyCode.D))
        //     movement.x += 1;

        // Vector2 left = new Vector2(selectorLeft.transform.position.x, selectorLeft.transform.position.y);
        // Vector2 right = new Vector2(selectorRight.transform.position.x, selectorRight.transform.position.y);

        // if(grid.GetComponent<GridManager>().ValidMovement(left + movement) && grid.GetComponent<GridManager>().ValidMovement(right + movement))
        // {
        //     selectorLeft.GetComponent<PlayerTileSelector>().Position = ((int)(left.x + movement.x), (int)(left.y + movement.y));
        //     selectorRight.GetComponent<PlayerTileSelector>().Position = ((int)(right.x + movement.x), (int)(right.y + movement.y));
        // }

    }

    void Swap(InputAction.CallbackContext obj)
    {
        Debug.Log(obj);
        // if(Input.GetKeyDown(KeyCode.Space))
        // {
        //     grid.GetComponent<GridManager>().SwapTiles(selectorLeft, selectorRight);
        // }
    }
}
