using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    public int id;

    static Color selectedCandyColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    static Candy previousSelectedCandy = null;
    [SerializeField] bool isSelected = false;

    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void SelectCandy()
    {
        isSelected = true;
        spriteRenderer.color = selectedCandyColor;
        previousSelectedCandy = gameObject.GetComponent<Candy>();
    }

    void DeselectCandy()
    {
        isSelected = false;
        spriteRenderer.color = Color.white;
        previousSelectedCandy = null;
    }

    private void OnMouseDown()
    {
        if (spriteRenderer.sprite == null || 
            BoardManager.instance.IsShifting)
        {
            return;
        }

        if (isSelected)
        {
            DeselectCandy();
        }
        else
        {
            if (previousSelectedCandy == null)
            {
                SelectCandy();
            }
            else
            {
                previousSelectedCandy.DeselectCandy();
                SelectCandy();
            }
        }
    }
}