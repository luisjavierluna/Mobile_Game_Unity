using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public static BoardManager instance;

    [SerializeField] GameObject currentCandy;
    [SerializeField] int xSize, ySize;
    public List<Sprite> sprites = new List<Sprite>();
    GameObject[,] candies;

    public bool IsShifting { get; set; }

    public const int MIN_CANDIES_TO_MATCH = 2;

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        Vector2 offset = currentCandy.GetComponent<BoxCollider2D>().size;
        CreateInitialBoard(offset);
    }

    void CreateInitialBoard(Vector2 offset)
    {
        candies = new GameObject[xSize, ySize];

        float startX = transform.position.x;
        float startY = transform.position.y;

        int idx = -1;

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                GameObject newCandy = Instantiate(currentCandy, 
                    new Vector3(
                        startX + (offset.x * x), 
                        startY + (offset.y * y), 
                        0), 
                    currentCandy.transform.rotation);
                newCandy.name = string.Format("Candy [{0}][{1}]", x, y);
                candies[x, y] = newCandy;
                newCandy.transform.parent = transform;

                do{ idx = Random.Range(0, sprites.Count); } 
                while (x > 0 && idx == candies[x - 1, y].GetComponent<Candy>().id ||
                       y > 0 && idx == candies[x, y - 1].GetComponent<Candy>().id);

                Sprite sprite = sprites[idx];
                newCandy.GetComponent<SpriteRenderer>().sprite = sprite;
                newCandy.GetComponent<Candy>().id = idx;
            }
        }
    }
}