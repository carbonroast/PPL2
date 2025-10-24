using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Tilemaps;

public class Tile : GridObject
{
    private SpriteRenderer sr;
    //TODO  add color to tiles, add falling mechanic, add clearing mechanic
    private Element element;

    public void SetElement(Element element)
    {
        this.element = element;
        sr = GetComponent<SpriteRenderer>();
        sr.color = ElementalColorUtils.SetColor(element);
    }

    public void SetSprite(Element element)
    {
        // Future use a different sprite per element
    }


    public Element GetElement()
    {
        return element;
    }

    public async Task DestroyMeAsync()
    {
        float fadeDuration = 1f;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            Color tempColor = sr.color;
            tempColor.a = alpha;
            sr.color = tempColor;
            await Task.Yield();
        }

        Destroy(this.gameObject);
    }

    public async Task MoveMeAsync((int x, int y) position)
    {
        Vector2 startPosition = this.transform.localPosition;
        Vector2 endPosition = new Vector2(position.x, position.y);
        Debug.Log($"{startPosition},{endPosition}");
        float elasped = 0f;
        float duration = 0.3f;

        while (elasped < duration)
        {
            elasped += Time.deltaTime;
            this.transform.localPosition = Vector2.Lerp(startPosition, endPosition, elasped / duration);
            await Task.Yield();
        }
        this.Position = position;
    }
}
