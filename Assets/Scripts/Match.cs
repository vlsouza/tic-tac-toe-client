using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Match 
{
    [JsonProperty("match_id")]
    public string ID;

    [JsonProperty("board")]
    public string Board;

    [JsonProperty("status")]
    public string Status;

    [JsonProperty("current_player_turn")]
    public string CurrentPlayerTurn;

    [JsonProperty("last_move_xy")]
    public string LastMoveXY;

    [JsonProperty("next_player_turn")]
    public string NextPlayerTurn;

    //not available in the server
    public string StatusDescription;

    public enum StatusEnum
    {
        PENDINGPLAYER,
        RUNNING,
        PLAYER1WON,
        PLAYER2WON,
        DRAW
    }

    public static Match ConvertJsonToMatch(string jsonString)
    {
        return JsonConvert.DeserializeObject<Match>(jsonString);
    }

    public static List<Match> ConvertJsonToList(string jsonString)
    {
        var settings = new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore,
            NullValueHandling = NullValueHandling.Include
        };

        return JsonConvert.DeserializeObject<List<Match>>(jsonString, settings);
    }
}

[System.Serializable]
public class MatchList
{
    public List<Match> matches;
}

