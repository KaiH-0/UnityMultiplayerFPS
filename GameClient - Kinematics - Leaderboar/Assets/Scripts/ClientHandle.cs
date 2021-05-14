using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        // Now that we have the client's id, connect UDP
        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
        Debug.Log("I'm spawning players I promise");
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        bool _walking = _packet.ReadBool();
        //Quaternion _gunrotation = _packet.ReadQuaternion();
        //Quaternion _fakecam = _packet.ReadQuaternion();
        //Vector3 _gunposition = _packet.ReadVector3();

        GameManager.players[_id].SetPosition(_position);
        GameManager.players[_id].isWalking = _walking;
        //GameManager.players[_id].gun.transform.position = _gunposition;
        //GameManager.players[_id].gun.transform.rotation = _gunrotation;
    }

    public static void PlayerKill(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _kill = _packet.ReadString();

        GameManager.players[_id].SetKill(_kill);
        Debug.Log(_kill);
    }

    public static void PlayerRotation(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();
        Quaternion _cameraRotation = _packet.ReadQuaternion();
        //Boolean _shot = _packet.ReadBool();
        //Quaternion _fakecam = _packet.ReadQuaternion();
        //Vector3 _fakecampos = _packet.ReadVector3();
        //Vector3 _gunposition = _packet.ReadVector3();
        int _weapon = _packet.ReadInt();
        bool _holding = _packet.ReadBool();

        GameManager.players[_id].transform.rotation = _rotation;
        GameManager.players[_id].head.transform.rotation = _cameraRotation;
        //GameManager.players[_id].myObjects[GameManager.players[_id].weapon].transform.position = _gunposition;
        GameManager.players[_id].weapon = _weapon;
        GameManager.players[_id].myObjects[GameManager.players[_id].weapon].GetComponent<GunHand>().holding = _holding;
        //GameManager.players[_id].Shot = _shot;
        //GameManager.players[_id].FakeCam.transform.rotation = _fakecam;
        //GameManager.players[_id].FakeCam.transform.position = _fakecampos;
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int _id = _packet.ReadInt();

        Destroy(GameManager.players[_id].gameObject);
        GameManager.players.Remove(_id);
    }

    public static void PlayerHealth(Packet _packet)
    {
        int _id = _packet.ReadInt();
        float _health = _packet.ReadFloat();

        GameManager.players[_id].SetHealth(_health);
    }

    public static void PlayerRespawned(Packet _packet)
    {
        int _id = _packet.ReadInt();

        GameManager.players[_id].Respawn();
    }

    public static void CreateItemSpawner(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        Vector3 _spawnerPosition = _packet.ReadVector3();
        bool _hasItem = _packet.ReadBool();

        GameManager.instance.CreateItemSpawner(_spawnerId, _spawnerPosition, _hasItem);
    }

    public static void ItemSpawned(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();

        GameManager.itemSpawners[_spawnerId].ItemSpawned();
    }

    public static void ItemPickedUp(Packet _packet)
    {
        int _spawnerId = _packet.ReadInt();
        int _byPlayer = _packet.ReadInt();

        GameManager.itemSpawners[_spawnerId].ItemPickedUp();
        GameManager.players[_byPlayer].itemCount++;
    }

    public static void SpawnProjectile(Packet _packet)
    {
        int _projectileId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();
        int _thrownByPlayer = _packet.ReadInt();

        GameManager.instance.SpawnProjectile(_projectileId, _position);
        GameManager.players[_thrownByPlayer].itemCount--;
    }

    public static void ProjectilePosition(Packet _packet)
    {
        int _projectileId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        GameManager.projectiles[_projectileId].transform.position = _position;
    }

    public static void ProjectileExploded(Packet _packet)
    {
        int _projectileId = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        GameManager.projectiles[_projectileId].Explode(_position);
    }

    public static void DestroyInfo(Packet _packet)
    {
        //int _id = _packet.ReadInt();
        //int _id1 = _packet.ReadInt();
        //string _username1 = _packet.ReadString();
        //int _deaths1 = _packet.ReadInt();
        int _id1 = _packet.ReadInt();
        //GameObject.Find("Info").GetComponent<Info>().playerinfo.Clear();


        //GameManager.players[_id1].gameObject.transform.Find("Info").GetComponent<Info>().playerinfo.Add(_id1, new Info.PlayerInfo(_id1, username, deaths));
        //GameManager.players[_id].UpdateLeaderboard(_id);

        //GameManager.players[_id].UpdateLeaderboard();
        //GameManager.players[_id].playerinfo.Add(new PlayerManager.PlayerInfo(_id1, _username1, _deaths1));
    }

    public static void AddInfo(Packet _packet)
    {
        //int _id = _packet.ReadInt();
        //int _id1 = _packet.ReadInt();
        //string _username1 = _packet.ReadString();
        //for (int i = 1; i < GameObject.Find("Info").GetComponent<Info>().playerinfo.Count; i++)
        //{
        //    GameObject.Find("Info").GetComponent<Info>().playerinfo.Remove(i);
        //}
        //int _deaths1 = _packet.ReadInt();
        int _id1 = _packet.ReadInt();
        string username = _packet.ReadString();
        int deaths = _packet.ReadInt();
        //HERE HERE HERE
        int _id2 = _id1;
        //GameManager.players[_id1].playerinfo.Add(_id2,new PlayerManager.PlayerInfo(_id2, username, deaths));
        
        
        //GameManager.players[_id1].gameObject.transform.Find("Info").GetComponent<Info>().playerinfo.Add(_id1, new Info.PlayerInfo(_id1, username, deaths));
        //GameManager.players[_id].UpdateLeaderboard(_id);

        //GameManager.players[_id].UpdateLeaderboard();
        //GameManager.players[_id].playerinfo.Add(new PlayerManager.PlayerInfo(_id1, _username1, _deaths1));
    }

    public static void UpdateDeaths(Packet _packet)
    {
        int _id = _packet.ReadInt();
        int _id1 = _packet.ReadInt();
        string _username1 = _packet.ReadString();
        int _deaths1 = _packet.ReadInt();

        //GameManager.players[_id].playerinfo[_id1].Deaths = _deaths1;
        GameManager.players[_id].UpdateDeaths(_id);
        //GameManager.players[_id].UpdateLeaderboard(_id1);
        //GameManager.players[_id].UpdateLeaderboard();
        //GameManager.players[_id].playerinfo.Add(new PlayerManager.PlayerInfo(_id1, _username1, _deaths1));
    }

    //public static void UpdatePlayers(Packet _packet)
    //{
    //    int _id12 = _packet.ReadInt();
    //    int _id123 = _packet.ReadInt();

    //    GameManager.players[_id12].update = _id123;
        //GameManager.players[_id].UpdateLeaderboard(_id1);
        //GameManager.players[_id].UpdateLeaderboard();
        //GameManager.players[_id].playerinfo.Add(new PlayerManager.PlayerInfo(_id1, _username1, _deaths1));
    //}
}
