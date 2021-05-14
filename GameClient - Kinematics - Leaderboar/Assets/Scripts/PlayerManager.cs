using DitzelGames.FastIK;
using System.Collections;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Threading;
using UnityEditor;
//using TMPro.EditorUtilities;
//using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public GameObject head;
    public Animator anim;
    public Animator gunanim;
    //public Animator gunanim;
    public GameObject FakeCam;
    //public GameObject gun;
    //public PlayerController controller;
    public bool Shot;
    //public GameObject FakeCam;
    //public GameObject gunaimpos;
    //public GameObject gunpos;
    public bool isWalking;
    public float health;
    public float maxHealth = 100f;
    public int itemCount = 0;
    //public TwoBoneIKConstraint righthand;
    //public TwoBoneIKConstraint lefthand;
    public FastIKFabric left;
    public FastIKFabric right;
    public MeshRenderer model;
    private Vector3 fromPos = Vector3.zero;
    private Vector3 toPos = Vector3.zero;
    private float lastTime;
    public int weapon = 0;
    public GameObject[] myObjects;
    public GameObject text;
    public GameObject playerdeath;
    public int kills;
    private int prevweapon;
    public int up;
    //public GameObject leaderboard;
    //public List<PlayerInfo> playerinfo = new List<PlayerInfo>();
    public int count;
    public int idk;
    public int timer;
    public GameObject playercard;
    public int update;
    public Info info;
    public Dictionary<int, PlayerInfo> playerinfo = new Dictionary<int, PlayerInfo>();

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

    public void Start()
    {
        playerinfo.Add(0, new PlayerInfo(0, "ServerBoi", 0));
        //GameObject.Find("Info").GetComponent<Info>().playerinfo.Clear();
    }

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;
    }

    public void SetPosition(Vector3 position)
    {
        fromPos = toPos;
        toPos = position;
        lastTime = Time.time;
    }

    public void UpdateDeaths(int _id)
    {
        //Debug.Log(playerinfo[0].Username);
        Transform leaderboard = GameObject.Find("Leaderboard").transform;
        idk = leaderboard.childCount;
        //for (int i = 0; i < playerinfo.Count; i++)
        //{
        //leaderboard.GetChild(_id).transform.GetChild(5).GetComponent<Text>().text = playerinfo[_id].Deaths.ToString();
            //playercard.SetActive(false);
            //newCard.transform.GetChild(1).gameObject.GetComponent<Text>().text = playerinfo[_i].Username;
            //newCard.layer = 5;
            //GameObject.Find("Username").GetComponent<Text>().text = playerinfo[i].Username;
            //ServerSend.LeaderboardUpdate(playerinfo[i].ID, playerinfo[i].Username, playerinfo[i].Deaths, playerinfo.Count);
            //leaderboard = GameObject.Find("Username");
            //leaderboard.GetComponent<Text>().text = playerinfo[i].Username;
            //leaderboard.transform.GetChild(1).gameObject.GetComponent<Text>().text = playerinfo[i].Username;
            //}
        }

    public void UpdatePlayers(int _id)
    {
        //Debug.Log(playerinfo[0].Username);
        Transform leaderboard = GameObject.Find("Leaderboard").transform;
        //idk = leaderboard.childCount;
        //playerinfo[_id].Username = "Gone";
        //playerinfo[_id].ID = 0;
        foreach (Transform child in leaderboard)
        {
            Destroy(child.gameObject);
        }
        Debug.Log("Updating Players");
        //Destroy(leaderboard.GetChild(_id+1).gameObject);
        //playercard.SetActive(false);
        //newCard.transform.GetChild(1).gameObject.GetComponent<Text>().text = playerinfo[_i].Username;
        //newCard.layer = 5;
        //GameObject.Find("Username").GetComponent<Text>().text = playerinfo[i].Username;
        //ServerSend.LeaderboardUpdate(playerinfo[i].ID, playerinfo[i].Username, playerinfo[i].Deaths, playerinfo.Count);
        //leaderboard = GameObject.Find("Username");
        //leaderboard.GetComponent<Text>().text = playerinfo[i].Username;
        //leaderboard.transform.GetChild(1).gameObject.GetComponent<Text>().text = playerinfo[i].Username;
        //UpdateLeaderboard(id);
    }

    public void OnDestroy()
    {
        //playerinfo.Remove(playerinfo[id]);
        //UpdatePlayers(id);
        //ClientSend.UpdateLeaderboard();
        //if (id > 0 && id < info.playerinfo.Count)
        //{
        //info.oldid += 1;
        //if (id < info.playerinfo.Count)
        //{
        int _id = id;
        //HERE HERE HERE
        playerinfo.Remove(_id);
        //info.oldid = id;
        //}
        //else
        //{
        //    Debug.Log("ID = " + id + "Whereas amount of people = " + info.playerinfo.Count);
        //}
        //}
        //else
        //{
        //    info.playerinfo.Remove(info.playerinfo[id-info.oldid]);
        //}
    }

    public void UpdateLeaderboard(int _i)
    {
        //Debug.Log(playerinfo[0].Username);
        Transform leaderboard = GameObject.Find("Leaderboard").transform;
            idk = leaderboard.childCount;
                //if (playerinfo[_i].ID != 0 && leaderboard.childCount < playerinfo.Count)
                //{
                //    if (leaderboard.childCount < playerinfo.Count)
                 //   {
                 //       //GameObject playercard = playercard;
                 //       GameObject newCard = Instantiate(playercard, leaderboard) as GameObject;
                //        //playercard.SetActive(false);
                //        newCard.transform.GetChild(1).gameObject.GetComponent<Text>().text = playerinfo[_i].Username;
                //    }
                //}
                //newCard.layer = 5;
                //GameObject.Find("Username").GetComponent<Text>().text = playerinfo[i].Username;
                //ServerSend.LeaderboardUpdate(playerinfo[i].ID, playerinfo[i].Username, playerinfo[i].Deaths, playerinfo.Count);
                //leaderboard = GameObject.Find("Username");
                //leaderboard.GetComponent<Text>().text = playerinfo[i].Username;
                //leaderboard.transform.GetChild(1).gameObject.GetComponent<Text>().text = playerinfo[i].Username;
    }


    private void Update()
    {
         info = GameObject.Find("Info").GetComponent<Info>();

        if (update != 0)
        {
            UpdatePlayers(update);
            update = 0;
        }
        //Transform leaderboard = GameObject.Find("Leaderboard").transform;
        //for (int i = 0; i < playerinfo.Count; i++)
        //{
        //    if (leaderboard.GetChild(i).transform.GetChild(1).gameObject.GetComponent<Text>().text == "Gone")
        //    {
        //        Destroy(leaderboard.GetChild(i).gameObject);
        //    }
        //    Debug.Log("Updated Players");
        //}

        if (timer > 0)
        {
            timer -= 1;
        }

        if (timer <= 0)
        {
            //var item = info.playerinfo[id];
            //info.playerinfo.RemoveAt(id);
            //info.playerinfo.Insert(id, item);
            //UpdatePlayers();
            //UpdateLeaderboard();
            timer = 60;
        }

        //int playercount = GameManager.players.Count;
        //count = playerinfo.Count;

        //if (count == playercount)
        //{
        //    playerinfo.Clear();
        //}
        //Debug.Log(playerinfo[0].Username);

        //text.GetComponent<Text>().text = "Eat Ass";
        if (up == 1)
        {
            if (weapon < 1)
            {
                prevweapon = weapon;
                weapon += 1;
                //myObjects[weapon].GetComponent<GunHand>().holding = true;
                gunanim = myObjects[weapon].GetComponent<Animator>();
                //myObjects[prevweapon].GetComponent<GunHand>().holding = false;
                up = 0;
            }
        }

        if (up == 2)
        {
            if (weapon > 0)
            {
                prevweapon = weapon;
                weapon -= 1;
                //myObjects[weapon].GetComponent<GunHand>().holding = true;
                gunanim = myObjects[weapon].GetComponent<Animator>();
                //myObjects[prevweapon].GetComponent<GunHand>().holding = false;
                up = 0;
            }
        }

        if (weapon == 0)
        {
            myObjects[0].GetComponent<GunHand>().holding = true;
            myObjects[1].GetComponent<GunHand>().holding = false;
            myObjects[1].SetActive(myObjects[1].GetComponent<GunHand>().holding);
            myObjects[0].SetActive(myObjects[0].GetComponent<GunHand>().holding);
        }

        if (weapon == 1)
        {
            myObjects[0].GetComponent<GunHand>().holding = false;
            myObjects[1].GetComponent<GunHand>().holding = true;
            myObjects[0].SetActive(myObjects[0].GetComponent<GunHand>().holding);
            myObjects[1].SetActive(myObjects[1].GetComponent<GunHand>().holding);
        }
        //myObjects[prevweapon].SetActive(false);
        //gun.transform.rotation = head.transform.rotation;
        //righthand.data.target = controller.myObjects[weapon].transform.position;
        //lefthand.data.target = controller.myObjects[weapon].GetComponent<GunHand>().lefthand.transform;
        left.Target = myObjects[weapon].GetComponent<GunHand>().leftwrist.transform;
        right.Target = myObjects[weapon].GetComponent<GunHand>().rightwrist.transform;
        anim.SetBool("isWalking", isWalking);
        this.transform.position = Vector3.Lerp(fromPos, toPos, (Time.time - lastTime) / (1.0f / Constants.TICKS_PER_SEC));
    }

    public void SetHealth(float _health)
    {
        health = _health;
        
        if (health <= 0f)
        {
            Die();
        }
    }

    public void SetKill(string _text)
    {
        GameObject.Find("KillText").GetComponent<Text>().text = GameObject.Find("KillText").GetComponent<Text>().text + _text;
        //GameObject.Find("KillDraw").GetComponent<KillDraw>().count += 1;
    }

    public void Die()
    {
        //playerdeath.Play();
        model.enabled = false;
    }

    public void Respawn()
    {
        model.enabled = true;
        SetHealth(maxHealth);
    }
}
