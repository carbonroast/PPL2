using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    [SerializeField] private int x;
    [SerializeField] private int y;

    public int X => x;
    public int Y => y;

    public (int x, int y) Position
    {
        get =>(X,Y);
        set
        {
            this.y = value.y;
            this.x = value.x;
            this.transform.position = new Vector2(value.x,value.y);
        }
    } 

}
