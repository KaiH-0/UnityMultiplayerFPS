using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info : MonoBehaviour
{

    //public List<PlayerInfo> playerinfo = new List<PlayerInfo>();

    //HERE HERE HERE
    //public Dictionary<int, PlayerInfo> playerinfo = new Dictionary<int, PlayerInfo>();
    //public Dictionary<int, string, int> playerinfo = new Dictionary<int, string, int>();
    public int oldid = 0;
    // Start is called before the first frame update
    void Start()
    {
        //HERE HERE HERE
        //playerinfo.Add(0,new PlayerInfo(0, "ServerBoi", 0));
    }

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

    // Update is called once per frame
    void Update()
    {
        
    }
}
