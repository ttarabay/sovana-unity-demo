using System.Collections;
using UnityEngine;
using Cinemachine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using System;
using Photon.Realtime;

public class GameM : MonoBehaviourPunCallbacks, IPunObservable
{
    public static GameM Instance;

    [Header("[ canvas ] ")]
    [SerializeField] GameObject reconnectingUI;
    [Header("obj")]
    public CinemachineVirtualCamera vcam;
    [Header("[ player ]")]
    public CharaS playerLocal;

    bool isMinimize = false;
    DateTime pauseTimeStart;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.IsMessageQueueRunning = true;

        if (GlobalVarS.Instance.modeGame == ModeGame.OFFLINE)
        {
            PhotonNetwork.OfflineMode = true;
            GameObject playerObj = Instantiate(Resources.Load("Chara/Chara" + GlobalVarS.Instance.playerCharId) as GameObject, new Vector3(UnityEngine.Random.Range(-3f, 0.73f), -1.09f, 0f), Quaternion.identity);
            playerLocal = playerObj.GetComponent<CharaS>();
        }
        else
        {
            //online
            GameObject playerObj = PhotonNetwork.Instantiate("Chara/Chara" + GlobalVarS.Instance.playerCharId, new Vector3(UnityEngine.Random.Range(-3f, 0.73f), -1.09f, 0f), Quaternion.identity, 0);
            playerLocal = playerObj.GetComponent<CharaS>();
        }

    }

    #region reconnect
    public void OnApplicationFocus(bool haveFocus)
    {
        Debug.Log("haveFocus: " + haveFocus + ", pos: " + GlobalVarS.Instance.posChara);
        if (!haveFocus)
        {
            pauseTimeStart = DateTime.Now;
            isMinimize = true;
            if (playerLocal != null)
            {
                GlobalVarS.Instance.posChara = playerLocal.transform.position;
            }
            else
            {
                Debug.Log("playerlocal is null");
            }
        }
        else
        {
            //Debug.Log("isConn: "+PhotonNetwork.IsConnected+", playerLocal: "+ playerLocal+", pos: "+playerLocal.transform.position+", graph:" +", isMinimize: " + isMinimize);
            Debug.Log("cState: " + PhotonNetwork.NetworkingClient.State);
            //Debug.Log("graph: " + playerLocal.GetComponent<CharaS>().graphObj.activeSelf);
            Debug.Log(pauseTimeStart.ToLongTimeString() + " = " + DateTime.Now.ToLongTimeString());
            TimeSpan ts = DateTime.Now - pauseTimeStart;
            Debug.Log("ts: " + ts.TotalSeconds);
            StartCoroutine(DelayCheck());
        }
    }

    IEnumerator DelayCheck()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("after 2s isConn: " + PhotonNetwork.IsConnected + ", isMinimize: " + isMinimize);
        Debug.Log("cState: " + PhotonNetwork.NetworkingClient.State);
        //Debug.Log("graph: " + playerLocal.GetComponent<CharaS>().graphObj.activeSelf);

        if (isMinimize && !PhotonNetwork.IsConnected)
        {
            Debug.Log("join again");
            //showReconnectingImg
            reconnectingUI.SetActive(true);
            ReJoin();
        }
    }
    #endregion

    #region PUN
    void ReJoin()
    {
        /*if (!GlobalVarS.Instance.IsInternetAvailable())
        {
            Debug.Log("reconnecting");
            StartCoroutine(JoinAgain());
            return;
        }*/

        //try
        //{
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.GameVersion = GlobalVarS.Instance.gameVersion;
        PhotonNetwork.LocalPlayer.NickName = GlobalVarS.Instance.playerName;
        /*}
        catch (Exception err)
        {
            Debug.Log(err.Message);
            StartCoroutine(JoinAgain());
        }*/
    }
    IEnumerator JoinAgain()
    {
        yield return new WaitForSeconds(2f);
        ReJoin();
    }
    public void LeftRoom()
    {
        GlobalVarS.Instance.isClickLeftGame = true;

        PhotonNetwork.LeaveRoom();
    }

    void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = (byte)GlobalVarS.Instance.MaxPlayers;
        PhotonNetwork.CreateRoom(GlobalVarS.Instance.roomName, roomOptions, null);
    }
    void JoinOrCreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = (byte)GlobalVarS.Instance.MaxPlayers;
        PhotonNetwork.JoinOrCreateRoom(GlobalVarS.Instance.roomName, roomOptions, TypedLobby.Default);// (roomName, options, null);
    }
    #endregion

    #region PUN CALLBACK
    public override void OnConnectedToMaster()
    {
        Debug.Log("Connect to Master");

        if (!PhotonNetwork.InLobby)
        {
            if (PhotonNetwork.NetworkingClient.State == ClientState.ConnectedToMasterServer)
            {
                PhotonNetwork.JoinLobby();
            }
        }
        else
        {
            Debug.Log("Join Lobby");
            //PhotonNetwork.JoinRandomRoom();
            JoinOrCreateRoom();
        }
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log(returnCode + ": " + message);
        if (returnCode == 32766) //A game with the specified id already exist.
        {
            Debug.Log("join random again");
        }

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        CreateRoom();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Join Lobby");
        //PhotonNetwork.JoinRandomRoom();
        JoinOrCreateRoom();
    }

    public override void OnJoinedRoom()
    {
        Debug.Log(PhotonNetwork.CurrentRoom + ", cp: " + PhotonNetwork.CurrentRoom.CustomProperties);
        //hidereconnection ui
        reconnectingUI.SetActive(false);
        //instantiate

        GameObject playerObj = PhotonNetwork.Instantiate("Chara/Chara" + GlobalVarS.Instance.playerCharId, GlobalVarS.Instance.posChara, Quaternion.identity, 0);
        playerLocal = playerObj.GetComponent<CharaS>();
    }

    private void OnPlayerDisconnected(Player player)
    {
        Debug.Log(player.NickName + " has disconnected");
    }

    public override void OnLeftRoom()
    {
        if (GlobalVarS.Instance.isClickLeftGame)
        {
            GlobalVarS.Instance.isClickLeftGame = false;

            UIEvents.OnAddressablesCheck("MenuS");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " has entered room");
        //CanvasUIS.Instance.AddPlayerChatInfo(newPlayer);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + " has left room");

        //CanvasUIS.Instance.RemovePlayerChatInfo(otherPlayer);
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {

        }
    }

    #endregion

    #region Serialized
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            //pos
            //stream.SendNext(randomPosMaskot);
        }
        else
        {
            //randomPosMaskot = (int)stream.ReceiveNext();
            //if (randomPosMaskot > -1 && maskotNPC == null)
            //{
            //    Debug.Log("cc add maskot, pos: "+ randomPosMaskot);
            //    maskotNPC = Instantiate(maskotPref, maskotPosCont.GetChild(randomPosMaskot).transform.position, maskotPosCont.GetChild(randomPosMaskot).transform.rotation);
            //}
        }
    }
    #endregion
}
