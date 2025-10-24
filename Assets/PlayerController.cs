using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using System.Threading.Tasks;

public class PlayerController : MonoBehaviour
{
    GridManager gm;
    PlayerTileSelector leftSelector;
    PlayerTileSelector rightSelector;
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
        Init();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void Movement(InputAction.CallbackContext context)
    {
        Vector2 rawInput = context.ReadValue<Vector2>();
        int dx = (int)Mathf.Round(rawInput.x);
        int dy = (int)Mathf.Round(rawInput.y);

        (int x, int y) leftPos = leftSelector.Position;
        (int x, int y) rightPos = rightSelector.Position;

        (int x, int y) leftTarget = (leftPos.x + dx, leftPos.y + dy);
        (int x, int y) rightTarget = (rightPos.x + dx, rightPos.y + dy);

        if (gm.ValidMovement(leftTarget) && gm.ValidMovement(rightTarget))
        {
            leftSelector.Position = leftTarget;
            rightSelector.Position = rightTarget;
        }

    }

    async void Swap(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            await gm.SwapTilesAsync(leftSelector.Position, rightSelector.Position);
        }
    }
    public void SetGridManager(GridManager gm)
    {
        this.gm = gm;
    }
    void Init()
    {
        GameObject leftSelectorGO = this.transform.GetChild(0).gameObject;
        GameObject rightSelectorGO = this.transform.GetChild(1).gameObject;
        leftSelector = leftSelectorGO.GetComponent<PlayerTileSelector>();
        rightSelector = rightSelectorGO.GetComponent<PlayerTileSelector>();
        leftSelector.Position = ((0,0));
        rightSelector.Position = ((1,0));
    }
}
