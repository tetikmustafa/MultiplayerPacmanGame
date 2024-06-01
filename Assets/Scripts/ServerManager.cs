using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class ServerManager : MonoBehaviourPunCallbacks
{
    public Text waitingText;
    PhotonView pw;
    void Start()
    {
        this.pw = GetComponent<PhotonView>();
        PhotonNetwork.ConnectUsingSettings();
    }
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Server.");
        PhotonNetwork.JoinLobby();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined to Lobby.");
        PhotonNetwork.JoinOrCreateRoom("Room", new RoomOptions { MaxPlayers = 4, IsOpen = true, IsVisible = true }, TypedLobby.Default);
    }
    public override void OnLeftLobby()
    {
        Debug.Log("Left the Lobby.");
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Created the Room.");
        GameObject pacman = PhotonNetwork.Instantiate("Pacman", new Vector3(0f, -9.5f, -5f), Quaternion.identity, 0, null);
        pacman.GetComponent<PhotonView>().Owner.NickName = "PlayerPacman";
        //GameObject gameManager = PhotonNetwork.Instantiate("GameManager", Vector3.zero, Quaternion.identity, 0, null);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined to Room.");
        /*if (PhotonNetwork.CurrentRoom.PlayerCount <= 1)
        {
            GameObject pacman = PhotonNetwork.Instantiate("Pacman", new Vector3(0f, -9.5f, -5f), Quaternion.identity, 0, null);
            pacman.GetComponent<PhotonView>().Owner.NickName = "PlayerPacman";
            //GameObject.Find("GameManager").GetComponent<PhotonView>().RPC("AssignPacman", RpcTarget.All, pacman);
        }
        else */
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            GameObject blinky = PhotonNetwork.Instantiate("Ghost_Blinky", new Vector3(0f, -0.5f, -1f), Quaternion.identity, 0, null);
            blinky.GetComponent<PhotonView>().Owner.NickName = "Blinky";
            //GameObject.Find("GameManager").GetComponent<PhotonView>().RPC("AddGhost", RpcTarget.All, blinky);
        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount == 3)
        {
            GameObject inky = PhotonNetwork.Instantiate("Ghost_Inky", new Vector3(2f, -0.5f, -1f), Quaternion.identity, 0, null);
            inky.GetComponent<PhotonView>().Owner.NickName = "Inky";
            //GameObject.Find("GameManager").GetComponent<PhotonView>().RPC("AddGhost", RpcTarget.All, inky);

        }
        else if (PhotonNetwork.CurrentRoom.PlayerCount >= 4)
        {
            GameObject clyde = PhotonNetwork.Instantiate("Ghost_Clyde", new Vector3(-2f, -0.5f, -1f), Quaternion.identity, 0, null);
            clyde.GetComponent<PhotonView>().Owner.NickName = "Clyde";
            //GameObject.Find("GameManager").GetComponent<PhotonView>().RPC("AddGhost", RpcTarget.All, clyde
        }
        PlayerControlStart();
    }

    public override void OnLeftRoom()
    {
        Debug.Log("Left the Room.");

    }
    public override void OnJoinRoomFailed(short returnCode,string message)
    {
        Debug.Log("ERROR: Join Room Failed.");
    }
    /*void PlayerControl()
    {
        if (GameObject.Find("Pacman(Clone)") != null)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                this.waitingText.enabled = true;
                GameObject.Find("Pacman(Clone)").GetComponent<PhotonView>().RPC("Deactive", RpcTarget.All, null);
            }
            else if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
            {
                this.waitingText.enabled = false;
                GameObject.Find("Pacman(Clone)").GetComponent<PhotonView>().RPC("Active", RpcTarget.All, null);
            }
        }
    }*/
    /*void PlayerControlStart()
    {

        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {

            Debug.Log("More than two Players");
            this.waitingText.enabled = false;
            //Debug.Log(GameObject.Find("Pacman(Clone)").GetInstanceID());
            Debug.Log("sea:"+GameObject.Find("GameManager").GetInstanceID());
            Debug.Log("Starting New Game");
            GameObject.Find("GameManager").GetComponent<PhotonView>().RPC("StartNewGame", RpcTarget.All, null);
            //CancelInvoke();
            //InvokeRepeating("PlayerControlWait", 0f, 0.5f);
        }
    }*/
    void PlayerControlStart()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            waitingText.enabled = false;
            GameObject gameManager = GameObject.Find("GameManager");
            if (gameManager != null)
            {
                PhotonView photonView = gameManager.GetComponent<PhotonView>();
                if (photonView != null)
                {
                    Debug.Log("More than two Players");
                    Debug.Log("Starting New Game");
                    photonView.RPC("StartNewGame", RpcTarget.All, null);
                }
                else
                {
                    Debug.LogError("PhotonView component not found on GameManager GameObject.");
                }
            }
            else
            {
                Debug.LogError("GameManager GameObject not found in the scene.");
            }
        }
    }
    void PlayerControlWait()
    {
        if(PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            this.waitingText.enabled = true;
            GameObject.Find("GameManager(Clone)").GetComponent<PhotonView>().RPC("WaitforPlayers", RpcTarget.All, null);
        }
    }
    void Update()
    {
        
    }
}
