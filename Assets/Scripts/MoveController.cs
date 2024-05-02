using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveController : MonoBehaviour
{
    public static MoveController instance;

    public GameObject circleMoveGameObeject;
    public GameObject crossMoveGameObeject;
    public int moveRow;
    public int moveCol;
    public int moveIndex;

    private Button moveButton;

    // Start is called before the first frame update
    void Start()
    {
        moveButton = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        if (crossMoveGameObeject.activeInHierarchy || circleMoveGameObeject.activeInHierarchy)
        {
            moveButton.interactable = false;
        }

        if (GameController.instance.currentMatch.CurrentPlayerTurn == GameController.instance.player && GameController.instance.currentMatch.Status == "RUNNING")
        {
            moveButton.interactable = true;
        } else
        {
            moveButton.interactable = false;
        }
    }

    public void Move()
    {
        StartCoroutine(ServerController.instance.UpdateMatch(GameController.instance.currentMatch.ID, moveRow, moveCol));

        if (GameController.instance.player == "PLAYER1")
        {
            crossMoveGameObeject.SetActive(true);
        }
        else if (GameController.instance.player == "PLAYER2")
        {
            circleMoveGameObeject.SetActive(true);
        }

        moveButton.interactable = false;
    }
}
