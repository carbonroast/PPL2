using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class Tile : GridObject
{   
    //TODO  add color to tiles, add falling mechanic, add clearing mechanic
    private Element element;

    public void SetElement(Element element)
    {
        this.element = element;
        this.GetComponent<SpriteRenderer>().color = ElementalColorUtils.SetColor(element);
    }

    public void SetSprite(Element element)
    {
        // Future use a different sprite per element
    }

    public Element GetElement()
    {
        return element;
    }
    private void Fall()
    {

    }


}
