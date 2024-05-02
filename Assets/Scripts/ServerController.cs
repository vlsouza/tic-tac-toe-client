using Amazon.Runtime.Internal.Endpoints.StandardLibrary;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ServerController : MonoBehaviour
{
    public static ServerController instance;

    public List<Match> availableMatches = new List<Match>();

    private string serverAddress;
    private string serverURI;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        serverAddress = Environment.GetEnvironmentVariable("TTT_SERVER_ADDRESS", EnvironmentVariableTarget.Machine);
        serverURI = "http://" + serverAddress + ":8080";

        Debug.Log(serverURI);
    }

    public IEnumerator GetMatchesByStatus(Match.StatusEnum status)
    {
        string uri = serverURI + "/matches?status=" + status.ToString();
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log("Match found: " + request.downloadHandler.text);
                availableMatches = Match.ConvertJsonToList(request.downloadHandler.text);

                foreach (Match match in availableMatches)
                {
                    Debug.Log("Match ID: " + match.ID);
                }
            }
        }
    }

    public IEnumerator GetStateByID(string matchID)
    {
        string uri = serverURI + "/matches/" + matchID;
        using (UnityWebRequest request = UnityWebRequest.Get(uri))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(request.error);
            }
            else
            {
                Debug.Log("Match found: " + request.downloadHandler.text);
                GameController.instance.currentMatch = Match.ConvertJsonToMatch(request.downloadHandler.text);
            }
        }
    }

    public IEnumerator CreateMatch()
    {
        string uri = serverURI + "/matches";

        // Criar a requisição POST
        UnityWebRequest request = UnityWebRequest.PostWwwForm(uri, UnityWebRequest.kHttpVerbPOST);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Enviar a requisição e aguardar sua conclusão
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log("Match created: " + request.downloadHandler.text);
            availableMatches.Add(Match.ConvertJsonToMatch(request.downloadHandler.text));

            foreach (Match match in availableMatches)
            {
                Debug.Log("Match ID: " + match.ID);
            }
        }
    }

    public IEnumerator UpdateMatch(string matchID, int row, int col)
    {
        string uri = serverURI + "/matches/" + matchID + "/move";

        // Criar um objeto para enviar
        Move dataToSend = new Move { row = row, col = col };
        string json = JsonUtility.ToJson(dataToSend);

        // Converter o JSON em um formato compatível com byte
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);

        Debug.Log("Match: " + matchID);
        Debug.Log("Received JSON: " + json);

        // Criar a requisição POST
        UnityWebRequest request = new UnityWebRequest(uri, UnityWebRequest.kHttpVerbPUT);
        request.uploadHandler = new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Enviar a requisição e aguardar sua conclusão
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log("Match updated: " + request.downloadHandler.text);
            GameController.instance.currentMatch = Match.ConvertJsonToMatch(request.downloadHandler.text);
        }
    }

    public IEnumerator StartMatch(string matchID)
    {
        string uri = serverURI + "/matches/" + matchID + "/start";

        // Criar a requisição POST
        UnityWebRequest request = new UnityWebRequest(uri, UnityWebRequest.kHttpVerbPUT);
        request.downloadHandler = new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Enviar a requisição e aguardar sua conclusão
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError(request.error);
        }
        else
        {
            Debug.Log("Match updated: " + request.downloadHandler.text);
        }
    }
}
