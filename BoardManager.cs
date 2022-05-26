using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] GameObject currentCandy;
    [SerializeField] int xSize, ySize;

    private void Start()
    {
        Vector2 offset = currentCandy.GetComponent<BoxCollider2D>().size;
        CreateInitialBoard(offset);
    }

    void CreateInitialBoard(Vector2 offset)
    {
        float startX = transform.position.x;
        float startY = transform.position.y;

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
                newCandy.transform.parent = transform;
            }
        }
    }
}