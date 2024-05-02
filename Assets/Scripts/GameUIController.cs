using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUIController : MonoBehaviour
{
    public static GameUIController instance;

    public Text playerDescription;
    public Text statusDescription;

    public GameObject canvasGroup;

    // Start is called before the first frame update
    void Start()
    {
        var player = PlayerPrefs.GetString("player");

        if (player == "PLAYER1")
        {
            playerDescription.text = "You: X (Player 1)";
        }
        else if (player == "PLAYER2")
        {
            playerDescription.text = "You: O (Player 2)";
        }
    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        statusDescription.text = GameController.instance.currentMatch.StatusDescription;
        UpdateBoard();
    }

    private void UpdateBoard()
    {
        Debug.Log("Current Board:" + GameController.instance.currentMatch.Board);
        string boardWithoutCommas = GameController.instance.currentMatch.Board.Replace(",", "");

        for (int i = 0; i < boardWithoutCommas.Length; i++)
        {
            if (boardWithoutCommas[i] == '1')
            {
                GameController.instance.crossGridSlots[i].SetActive(true);
            }
            else if (boardWithoutCommas[i] == '2')
            {
                GameController.instance.circleGridSlots[i].SetActive(true);
            }
        }
    }
}
