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
    PlayerInput playerInput;

    // Start is called before the first frame update
    void Awake()
    { 
        playerInput = gameObject.GetComponent<PlayerInput>();
  
    }

    void OnEnable()
    {
        playerInput.actions["Movement"].performed += Movement;
        playerInput.actions["Swap"].performed += Swap;

    }

    void OnDisable()
    {
        playerInput.actions["Movement"].performed -= Movement;
        playerInputActions.Keyboard.Movement.Disable();
        
        playerInput.actions["Swap"].performed -= Swap;
        playerInputActions.Keyboard.Swap.Disable();
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

    void Movement(InputAction.CallbackContext context)
    {
        Vector2 movement = context.ReadValue<Vector2>();

        Vector2 left = new Vector2(selectorLeft.transform.position.x, selectorLeft.transform.position.y);
        Vector2 right = new Vector2(selectorRight.transform.position.x, selectorRight.transform.position.y);

        if(grid.GetComponent<GridManager>().ValidMovement(left + movement) && grid.GetComponent<GridManager>().ValidMovement(right + movement))
        {
            selectorLeft.GetComponent<PlayerTileSelector>().Position = ((int)(left.x + movement.x), (int)(left.y + movement.y));
            selectorRight.GetComponent<PlayerTileSelector>().Position = ((int)(right.x + movement.x), (int)(right.y + movement.y));
        }

    }

    void Swap(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            grid.GetComponent<GridManager>().SwapTiles(selectorLeft, selectorRight);
        }
    }
}
