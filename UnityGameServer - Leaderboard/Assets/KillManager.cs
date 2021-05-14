using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class KillManager : MonoBehaviour
{
    //public List<PlayerInfo> playerinfo = new List<PlayerInfo>();
    public Dictionary<int, PlayerInfo> playerinfo = new Dictionary<int, PlayerInfo>();
    //public PlayerInfo[,] playerinfo;
    // Start is called before the first frame update
    //public int deathscheck;
    //public int deathscheck2;
    //public int IDCheck;
    //public int Count;
    //public int ClientCount;
    public int oldid = 0;
    public class PlayerInfo
    {
        public int ID;
        public string Username;
        public int Deaths;

        public PlayerInfo(int _id, string _username, int _deaths)
        {
            ID = _id;
            Username = _username;
            Deaths = _deaths;
        }
    }

    void Start()
    {
        playerinfo.Add(0,new PlayerInfo(0, "Server", 0));
    }

    // Update is called once per frame
    void Update()
    {
        //if (playerinfo.Count > 0)
        //{
        //    Count = playerinfo.Count;
        //}
        //Debug.Log(playerinfo.Count);
    }

    //public void UpdateDeaths(Player _player1)
    //{
    //    for (int i = 0; i < playerinfo.Count; i++)
    //    {
    //        ServerSend.UpdateDeaths(_player1, playerinfo[i].ID, playerinfo[i].Username, playerinfo[i].Deaths);
    //    }
    //}

    //public void UpdatePlayers(Player _player1, int id)
    //{
    //        if (ClientCount < playerinfo.Count + oldid)
    //        {
    //            ServerSend.UpdatePlayers(_player1, playerinfo[id].ID);
    //            Debug.Log("Sent UpdatePlayers");
    //        }
    //}

    public void DestroyInfo()
    {
        //ServerSend.DestroyInfo();
    }

    public void AddInfo()
    {
        for (int i = 1; i < playerinfo.Count; i++)
        {
            ServerSend.AddInfo(i, playerinfo[i].Username, playerinfo[i].Deaths);
            Debug.Log("Sent Leaderboard");
        }
    }
}
