using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerHandle
{

    public static void WelcomeReceived(int _fromClient, Packet _packet)
    {
        int _clientIdCheck = _packet.ReadInt();
        string _username = _packet.ReadString();

        Debug.Log($"{Server.clients[_fromClient].tcp.socket.Client.RemoteEndPoint} connected successfully and is now player {_fromClient}.");
        if (_fromClient != _clientIdCheck)
        {
            Debug.Log($"Player \"{_username}\" (ID: {_fromClient}) has assumed the wrong client ID ({_clientIdCheck})!");
        }
        Server.clients[_fromClient].SendIntoGame(_username);
    }

    public static void PlayerMovement(int _fromClient, Packet _packet)
    {
        bool[] _inputs = new bool[_packet.ReadInt()];
        for (int i = 0; i < _inputs.Length; i++)
        {
            _inputs[i] = _packet.ReadBool();
        }
        Quaternion _rotation = _packet.ReadQuaternion();
        Quaternion _cameraRotation = _packet.ReadQuaternion();
        Boolean _walking = _packet.ReadBool();
        //Boolean _shot = _packet.ReadBool();
        //Quaternion _fakecam = _packet.ReadQuaternion();
        //Vector3 _fakecampos = _packet.ReadVector3();
        //Vector3 _gunposition = _packet.ReadVector3();
        int _weapon = _packet.ReadInt();
        Boolean _holding = _packet.ReadBool();
        //int _clientcount = _packet.ReadInt();
        //Quaternion _gunrotation = _packet.ReadQuaternion();

        Server.clients[_fromClient].player.SetInput(_inputs, _rotation);
        Server.clients[_fromClient].player.headAngle = _cameraRotation;
        Server.clients[_fromClient].player.walking = _walking;
        //Server.clients[_fromClient].player.shot = _shot;
        //Server.clients[_fromClient].player.fakeCam = _fakecam;
        //Server.clients[_fromClient].player.fakeCamPos = _fakecampos;
        //Server.clients[_fromClient].player.gunpos = _gunposition;
        Server.clients[_fromClient].player.weapon = _weapon;
        Server.clients[_fromClient].player.holding = _holding;
        //Server.clients[_fromClient].player.killman.ClientCount = _clientcount;
        //Server.clients[_fromClient].player.gunrot = _gunrotation;
    }

    //public static void leaderboardUpdate(int _fromClient, Packet _packet)
    //{
    //    int _count = _packet.ReadInt();

        //Server.clients[_fromClient].player.killman.UpdateKillTest();
    //    Debug.Log("Sent Leaderboard Update");
    //}

    public static void PlayerShoot(int _fromClient, Packet _packet)
    {
        Vector3 _shootDirection = _packet.ReadVector3();

        Server.clients[_fromClient].player.Shoot(_shootDirection);
    }

    public static void PlayerThrowItem(int _fromClient, Packet _packet)
    {
        Vector3 _throwDirection = _packet.ReadVector3();

        Server.clients[_fromClient].player.ThrowItem(_throwDirection);
    }
}
