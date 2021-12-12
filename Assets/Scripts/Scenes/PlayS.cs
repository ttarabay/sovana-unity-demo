using System.Collections;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class PlayS : MonoBehaviourPunCallbacks
{ 
    [Header("[ var ]")]
    [SerializeField] int roomIdAdded = 0;
    [Space]
    [SerializeField] TMP_Text infoT;

    // Start is called before the first frame update
    void Start()
    {

        ClickJoinRandom();
    }

    #region Click
    private void ClickJoinRandom()
    {
        GlobalVarS.Instance.roomName = "newWorld_" + roomIdAdded;

        //MenuM.Instance.panelLoading.gameObject.SetActive(true);

        if (GlobalVarS.Instance.modeGame == ModeGame.OFFLINE)
        {
            MenuM.Instance.GotoGame();
        }
        else
        {
            Connect();
        }
    }

    private void ClickCreateRoom()
    {/*
        if ((GlobalVarS.Instance.IsMobile && String.IsNullOrEmpty(roomNameIF_mobile.text)) ||
            (!GlobalVarS.Instance.IsMobile && String.IsNullOrEmpty(roomNameIF.text)))
        {
            InfoS.Instance.Show("Nama ruang tidak boleh kosong!");
            return;
        }

        MenuM.Instance.panelLoading.gameObject.SetActive(true);
        //panelGroup.SetActive(false);

        GlobalVarS.Instance.roomName = roomNameIF.text;
        CreateRoom();*/
    }
    private void ClickJoinRoom()
    {
        //MenuM.Instance.panelLoading.gameObject.SetActive(true);
        //panelGroup.SetActive(false);

        //GlobalVarS.Instance.roomName = roomNameIF.text;
        JoinRoom();
    }
    #endregion

    #region RoomFunction
    private void Connect()
    {
        Debug.Log("isConnected: " + PhotonNetwork.IsConnected + ", isInLobby: " + PhotonNetwork.InLobby);

        if (!PhotonNetwork.IsConnected)
        {
            /*#if !UNITY_WEBGL && !UNITY_EDITOR
                        Debug.Log("IsInternetAvailable: " + GlobalVarS.Instance.IsInternetAvailable());
                        if (!GlobalVarS.Instance.IsInternetAvailable())
                        {
                            infoT.text = "No Connection detected!, try to connect again..";
                            StartCoroutine(JoinAgain());
                            return;
                        }
            #endif*/
            Debug.Log("ConnectUsingSettings: " + GlobalVarS.Instance.gameVersion);
            try
            {
            bool connect_result = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = GlobalVarS.Instance.gameVersion;
            PhotonNetwork.LocalPlayer.NickName = GlobalVarS.Instance.playerName;
            }
            catch (System.Exception err)
            {
               Debug.Log(err.Message);
                infoT.text = err.Message;
               StartCoroutine(JoinAgain());
            }
        }
        else
        {
            //text
            infoT.text = "joining to lobby..";

            if (!PhotonNetwork.InLobby)
            {
                Debug.Log("joining to lobby..");
                PhotonNetwork.JoinLobby();
            }
            else
            {
                Debug.Log("Join Lobby");
                //if (GlobalVarS.Instance.JoinType == JoinType.GROUP)
                //{
                //    panelGroup.SetActive(true);
                //}
                //else
               // {
                    //PhotonNetwork.JoinRandomRoom();
                    JoinOrCreateRoom();
               // }
            }
        }

        //text
        infoT.text = "Connecting..";
        //infoT.text = "";
    }

    IEnumerator JoinAgain()
    {
        yield return new WaitForSeconds(2f);
        Connect();
    }

    void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = true;
        roomOptions.MaxPlayers = (byte)GlobalVarS.Instance.MaxPlayers;
        PhotonNetwork.CreateRoom(GlobalVarS.Instance.roomName, roomOptions, null);
    }
    void JoinRoom()
    {
        PhotonNetwork.JoinRoom(GlobalVarS.Instance.roomName, null);
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

        //text
        infoT.text = "Connecting to lobby..";

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
        //infoT.text = "";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        infoT.text = returnCode + ": " + message;
        //show
        /*if (GlobalVarS.Instance.JoinType == JoinType.GROUP)
        {
            //panelGroup.SetActive(true);
            MenuM.Instance.panelLoading.SetActive(false);
        }*/
        if (returnCode == 32766) //A game with the specified id already exist.
        {
            Debug.Log("join random again");
            //if (GlobalVarS.Instance.JoinType == JoinType.RANDOM)
           // {
                roomIdAdded++;
                ClickJoinRandom();
            //}
        }

    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        infoT.text = returnCode + ": " + message;
        //show
        /*if (GlobalVarS.Instance.JoinType == JoinType.GROUP)
        {
            //panelGroup.SetActive(true);
            MenuM.Instance.panelLoading.SetActive(false);
        }*/
        if (returnCode == 32765) //A game with the specified id already full.
        {
            Debug.Log("join random again");
            //if (GlobalVarS.Instance.JoinType == JoinType.RANDOM)
            //{
                roomIdAdded++;
                ClickJoinRandom();
            //}
        }
        //CreateRoom();
    }

    public override void OnJoinedLobby()
    {
        //text
        infoT.text = "On lobby..";

        Debug.Log("Join Lobby");
        //PhotonNetwork.JoinRandomRoom();
        if (GlobalVarS.Instance.isBackFromGame)
        {
            //panelSelectCharacter.SetActive(true);
            //exitBtn.gameObject.SetActive(true);
        }
        else
        {
            JoinOrCreateRoom();
        }
        //infoT.text = "";
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        infoT.text = returnCode + ": " + message;
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.IsMessageQueueRunning = false;

        Debug.Log(PhotonNetwork.CurrentRoom + ", cp: " + PhotonNetwork.CurrentRoom.CustomProperties);

        infoT.text = "Room processing..";
        GlobalVarS.Instance.isBackFromGame = true;
        MenuM.Instance.GotoGame();
        //infoT.text = "Select Your Avatar";
        //panelGroup.SetActive(false);
        //loadingImg.gameObject.SetActive(false);
        //panelSelectCharacter.SetActive(true);
    }

    public override void OnLeftRoom()
    {
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
    }

    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
        {

        }
    }

    #endregion
}
