using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public static GUIManager instance;

    [SerializeField] Text movesText, scoreText;

    int moveCounter;
    public int MoveCounter
    {
        get => moveCounter;
        set
        {
            moveCounter = value;
            movesText.text = "Moves: " + moveCounter;
        }
    }

    int score;
    public int Score
    {
        get => score;
        set
        {
            score = value;
            scoreText.text = "Score: " + score;
        }
    }

    private void Start()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        score = 0;
        moveCounter = 30;

        movesText.text = "Moves: " + moveCounter;
        scoreText.text = "Score: " + score;
    }
}