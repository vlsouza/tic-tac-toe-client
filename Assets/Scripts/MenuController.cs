using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public Text menuStatusDescription;
    public GameObject playButton;
    public string gameScene;
    public float timeToLoadGame;

    private float timer = 0f;
    private float interval = 2f;
    private int tryCount = 0;

    private enum States
    {
        DEFAULT,
        FINDING_MATCH,
        CREATING_MATCH,
        JOINING_MATCH,
    }
    private States state = States.DEFAULT;

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        switch (state)
        {
            case States.DEFAULT:
                tryCount = 0;
                playButton.SetActive(true);
                menuStatusDescription.text = "";
                break;
            case States.FINDING_MATCH:
                playButton.SetActive(false);

                //create match after 3 tries
                if (tryCount >= 3)
                {
                    state = States.CREATING_MATCH;
                }

                menuStatusDescription.text = "Finding match...";
                timer += Time.fixedDeltaTime;
                if (timer >= interval && tryCount < 3)
                {
                    timer = 0f;
                    tryCount++;
                    StartCoroutine(ServerController.instance.GetMatchesByStatus(Match.StatusEnum.PENDINGPLAYER));
                }

                if (ServerController.instance.availableMatches.Count > 0)
                {
                    //if joining a existing match, set player 2
                    PlayerPrefs.SetString("player", "PLAYER2");
                    state = States.JOINING_MATCH;
                }
                break;
            case States.CREATING_MATCH:
                menuStatusDescription.text = "Creating match...";
                StartCoroutine(ServerController.instance.CreateMatch());
                //if creating, set player 1
                PlayerPrefs.SetString("player", "PLAYER1");
                state = States.JOINING_MATCH;
                break;
            case States.JOINING_MATCH:
                menuStatusDescription.text = "Joining...";
                Invoke("LoadGame", timeToLoadGame);
                break;
        }
    }

    public void SearchMatch()
    {
        state = States.FINDING_MATCH;
    }

    void LoadGame()
    {
        var match = ServerController.instance.availableMatches[0];
        PlayerPrefs.SetString("match_id", match.ID);
        PlayerPrefs.SetString("board", match.Board);
        PlayerPrefs.SetString("status", match.Status);
        PlayerPrefs.SetString("current_player_turn", match.CurrentPlayerTurn);
        PlayerPrefs.SetString("last_move_xy", match.LastMoveXY);
        PlayerPrefs.SetString("next_player_turn", match.NextPlayerTurn);
        PlayerPrefs.Save();
        SceneManager.LoadScene(gameScene);
    }
}
