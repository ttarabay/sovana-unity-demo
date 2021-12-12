using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.Collections;
using UnityEngine.AddressableAssets;
using DG.Tweening;
using UnityEngine.EventSystems;

public class UIS : MonoBehaviourPunCallbacks
{
    public static UIS Instance;

    [SerializeField] Button leaveBtn;
    [Header("[ player ]")]
    public TMP_Text playerNameT;
    [SerializeField] Transform playerPictCont;
    public Image playerPictImg;
    [Header("[ chat ]")]
    [SerializeField] RectTransform chatCont;
    [SerializeField] TMP_InputField chatIF;
    [SerializeField] TMP_Text chatFillT;
    [SerializeField] Button minimizeBtn;
    [SerializeField] Button sendChatBtn;

    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        sendChatBtn.onClick.AddListener(ClickSendChat);
        minimizeBtn.onClick.AddListener(ClickMinimizeChat);
        leaveBtn.onClick.AddListener(ClickLeave);

        //load pict
        StartCoroutine(LoadAddressableUserPict());

        chatCont.DOAnchorPosY(-232f, 0f);
    }

    private void ClickLeave()
    {
        GameM.Instance.LeftRoom();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!GlobalVarS.Instance.isPlayerStartChat)//chatIF.isFocused && 
            {
                chatIF.ActivateInputField();

                GlobalVarS.Instance.isPlayerStartChat = true;
                chatCont.DOAnchorPosY(-15f, 0.35f);
            }
            else if (GlobalVarS.Instance.isPlayerStartChat)
            {
                //baloonchat
                GameM.Instance.playerLocal.ShowChat(chatIF.text);
                //chatbox
                ClickSendChat();
                EventSystem.current.SetSelectedGameObject(null, null);
            }
        }
    }


    #region load pict user
    IEnumerator LoadAddressableUserPict()
    {
        var loadPict = Addressables.InstantiateAsync("CharaPict"+GlobalVarS.Instance.playerCharId, playerPictCont);
        Debug.Log("Starting load pict");

        while (!loadPict.IsDone)
        {
            yield return null;
        }
        playerPictImg = loadPict.Result.GetComponent<Image>();
    }
    #endregion

    #region chat
    public void ClickSelectInputField()
    {
        if (!GlobalVarS.Instance.isPlayerStartChat)//chatIF.isFocused && 
        {
            chatIF.ActivateInputField();

            GlobalVarS.Instance.isPlayerStartChat = true;
            chatCont.DOAnchorPosY(-15f, 0.35f);
        }
    }
    private void ClickMinimizeChat()
    {
        chatCont.DOAnchorPosY(-232f, 0.35f);
        EventSystem.current.SetSelectedGameObject(null, null);
    }

    private void ClickSendChat()
    {
        if (!string.IsNullOrEmpty(chatIF.text))
        {
            photonView.RPC("SendChatRPC", RpcTarget.All, new object[] { playerNameT.text, chatIF.text });
        }

        

        chatIF.text = "";
        GlobalVarS.Instance.isPlayerStartChat = false;
        chatIF.DeactivateInputField();
    }

    [PunRPC]
    void SendChatRPC(string senderName, string msg)
    {
        chatFillT.text += "-<color=white>"+ senderName + "</color>-\n" +
                         msg+"\n";
    }
    #endregion
}
