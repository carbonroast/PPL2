using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    [SerializeField] private int x;
    [SerializeField] private int y;

    public int X => x;
    public int Y => y;

    public (int x, int y) Position => (X,Y);

    public void SetPosition((int x, int y) position)
    {
        y = position.y;
        x = position.x;
        this.transform.position = new Vector2(position.x,position.y);
    }

}
