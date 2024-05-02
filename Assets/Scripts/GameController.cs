using Amazon.EC2.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController instance;

    public List<GameObject> crossGridSlots;
    public List<GameObject> circleGridSlots;
    public Match currentMatch = new Match();

    public string player;
    public string backScene;

    private float timer = 0f;
    private float interval = 3f;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        NewMatch();

        for (int i = 0; i < crossGridSlots.Count; i++)
        {
            crossGridSlots[i].SetActive(false);
            circleGridSlots[i].SetActive(false);
        }

        if (currentMatch.CurrentPlayerTurn == player)
        {
            currentMatch.StatusDescription = "Your turn!";
        }
        else
        {
            currentMatch.StatusDescription = "Waiting for the other player...";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        if (!Enum.TryParse(currentMatch.Status, true, out Match.StatusEnum currentMatchStatus))
        {
            currentMatchStatus = Match.StatusEnum.RUNNING;
        }

        switch (currentMatchStatus)
        {
            case Match.StatusEnum.RUNNING:
                timer += Time.fixedDeltaTime;
                if (timer >= interval)
                {
                    timer = 0f;
                    Debug.Log("Getting state by ID: " + currentMatch.ID);
                    StartCoroutine(ServerController.instance.GetStateByID(currentMatch.ID));
                }


                if (currentMatch.CurrentPlayerTurn == player)
                {
                    currentMatch.StatusDescription = "Your turn!";
                }
                else
                {
                    currentMatch.StatusDescription = "Waiting for the other player...";
                }

                break;
            case Match.StatusEnum.PENDINGPLAYER:
                currentMatch.StatusDescription = "Finding player...";

                timer += Time.fixedDeltaTime;
                if (timer >= interval)
                {
                    timer = 0f;
                    Debug.Log("Getting state by ID: " + currentMatch.ID);
                    StartCoroutine(ServerController.instance.GetStateByID(currentMatch.ID));
                }

                break;
            case Match.StatusEnum.PLAYER1WON:
                currentMatch.StatusDescription = "Player 1 won!";
                break;
            case Match.StatusEnum.PLAYER2WON:
                currentMatch.StatusDescription = "Player 2 won!";
                break;
            case Match.StatusEnum.DRAW:
                currentMatch.StatusDescription = "Draw :(";
                break;
            default: break;
        }
    }

    private void NewMatch()
    {
        currentMatch.ID = PlayerPrefs.GetString("match_id");
        currentMatch.Board = PlayerPrefs.GetString("board");
        currentMatch.Status = PlayerPrefs.GetString("status");
        currentMatch.CurrentPlayerTurn = PlayerPrefs.GetString("current_player_turn");
        currentMatch.LastMoveXY = PlayerPrefs.GetString("last_move_xy");
        currentMatch.NextPlayerTurn = PlayerPrefs.GetString("next_player_turn");


        player = PlayerPrefs.GetString("player");

        if (player == "PLAYER2")
        {
            //start the match if player 2 enters
            StartCoroutine(ServerController.instance.StartMatch(currentMatch.ID));
            currentMatch.Status = Match.StatusEnum.RUNNING.ToString();
        }
    }

    public void BackToScene()
    {
        SceneManager.LoadScene(backScene);
    }
}
