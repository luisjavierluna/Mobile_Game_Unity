using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Candy : MonoBehaviour
{
    public int id;

    static Color selectedCandyColor = new Color(0.5f, 0.5f, 0.5f, 1.0f);
    static Candy previousSelectedCandy = null;
    [SerializeField] bool isSelected = false;

    Vector2[] adjacentDirections = new Vector2[]
    {
        Vector2.up,
        Vector2.down,
        Vector2.right,
        Vector2.left
    };

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

        if (!isSelected)
        {
            if (previousSelectedCandy == null)
            {
                SelectCandy();
            }
            else
            {
                if (CanSwipe())
                {
                    SwapCandies(previousSelectedCandy);
                    previousSelectedCandy.FindAllMatches();
                    previousSelectedCandy.DeselectCandy();
                    FindAllMatches();
                    //SelectCandy();
                    
                }
                else
                {
                    previousSelectedCandy.DeselectCandy();
                    //SelectCandy();
                }
            }
        }
        else
        {
            DeselectCandy();
        }
    }

    void SwapCandies(Candy newCandy)
    {
        if (spriteRenderer.sprite == newCandy.spriteRenderer.sprite)
        {
            return;
        }

        Sprite oldCandy = newCandy.spriteRenderer.sprite;
        newCandy.spriteRenderer.sprite = spriteRenderer.sprite;
        spriteRenderer.sprite = oldCandy;

        int tempId = newCandy.id;
        newCandy.id = id;
        id  = tempId;
    }

    GameObject GetNeighbor(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);
        if (hit.collider != null)
        {
            return hit.collider.gameObject;
        }
        else
        {
            return null;
        }
    }

    List<GameObject> GetAllNeighbors()
    {
        List<GameObject> neighbors = new List<GameObject>();
        foreach (Vector2 direction in adjacentDirections)
        {
            neighbors.Add(GetNeighbor(direction));
        }
        return neighbors;
    }

    bool CanSwipe()
    {
        return GetAllNeighbors().Contains(previousSelectedCandy.gameObject);
    }

    List<GameObject> FindMatches(Vector2 direction)
    {
        List<GameObject> foundMatches = new List<GameObject>();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction);
        while (hit.collider != null && 
               hit.collider.GetComponent<SpriteRenderer>().sprite == spriteRenderer.sprite)
        {
            foundMatches.Add(hit.collider.gameObject);
            hit = Physics2D.Raycast(hit.collider.transform.position, direction);
        }

        return foundMatches;
    }

    bool ClearMatches(Vector2[] directions)
    {
        List<GameObject> clearedMatches = new List<GameObject>();
        foreach (Vector2 direction in directions)
        {
            clearedMatches.AddRange(FindMatches(direction));
        }

        if (clearedMatches.Count >= BoardManager.MIN_CANDIES_TO_MATCH)
        {
            foreach (GameObject candy in clearedMatches)
            {
                candy.GetComponent<SpriteRenderer>().sprite = null;
            }
            return true;
        }
        else
        {
            return false;
        }
    }

    public void FindAllMatches()
    {
        if (spriteRenderer.sprite == null)
        {
            return;
        }

        bool vMatch = ClearMatches(new Vector2[2] { Vector2.up, Vector2.down });
        bool hMatch = ClearMatches(new Vector2[2] { Vector2.right, Vector2.left });

        if (vMatch || hMatch)
        {
            spriteRenderer.sprite = null;
            StopCoroutine(BoardManager.instance.FindNullCandies());
            StartCoroutine(BoardManager.instance.FindNullCandies());
        }
    }
}