using System.Collections;
using System.Collections.Generic;
//using System.Diagnostics;
using UnityEngine;

public class ClientSend : MonoBehaviour
{
    /// <summary>Sends a packet to the server via TCP.</summary>
    /// <param name="_packet">The packet to send to the sever.</param>
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    /// <summary>Sends a packet to the server via UDP.</summary>
    /// <param name="_packet">The packet to send to the sever.</param>
    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    /// <summary>Lets the server know that the welcome message was received.</summary>
    public static void WelcomeReceived()
    {
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.usernameField.text);
            Debug.Log("Sent Head Rotation");
            SendTCPData(_packet);
        }
    }

    /// <summary>Sends player input to the server.</summary>
    /// <param name="_inputs"></param>
    public static void PlayerMovement(bool[] _inputs)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerMovement))
        {
            _packet.Write(_inputs.Length);
            foreach (bool _input in _inputs)
            {
                _packet.Write(_input);
            }
            _packet.Write(GameManager.players[Client.instance.myId].transform.rotation);
            _packet.Write(GameManager.players[Client.instance.myId].head.transform.rotation);
            _packet.Write(GameManager.players[Client.instance.myId].isWalking);
            //_packet.Write(GameManager.players[Client.instance.myId].myObjects[GameManager.players[Client.instance.myId].weapon].transform.position);
            _packet.Write(GameManager.players[Client.instance.myId].weapon);
            _packet.Write(GameManager.players[Client.instance.myId].myObjects[GameManager.players[Client.instance.myId].weapon].GetComponent<GunHand>().holding);
            //_packet.Write(GameManager.players[Client.instance.myId].playerinfo.Count);
            //_packet.Write(GameManager.players[Client.instance.myId].);
            //_packet.Write(GameManager.players[Client.instance.myId].Shot);
            //_packet.Write(GameManager.players[Client.instance.myId].gun.transform.rotation);
            //_packet.Write(GameManager.players[Client.instance.myId].FakeCam.transform.rotation);
            //_packet.Write(GameManager.players[Client.instance.myId].FakeCam.transform.position);

            SendUDPData(_packet);
        }
    }

    public static void UpdateLeaderboard()
    {
        using (Packet _packet = new Packet((int)ClientPackets.leaderboardUpdate))
        {
            //_packet.Write(GameManager.players[Client.instance.myId].playerinfo.Count);
        }
    }

public static void PlayerShoot(Vector3 _facing)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerShoot))
        {
            _packet.Write(_facing);

            SendTCPData(_packet);
        }
    }

    public static void PlayerThrowItem(Vector3 _facing)
    {
        using (Packet _packet = new Packet((int)ClientPackets.playerThrowItem))
        {
            _packet.Write(_facing);

            SendTCPData(_packet);
        }
    }
    #endregion
}
