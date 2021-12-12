using UnityEngine;
using TMPro;
using Photon.Pun;
using System;
using DG.Tweening;
using System.Collections;

public class CharaS : MonoBehaviourPunCallbacks
{
    [SerializeField] TMP_Text nameT;
    [SerializeField] Transform camPoint;
    [SerializeField] Transform chatBubble;
    TMP_Text chatT;
    CanvasGroup chatCG;

    Animator anim;
    CharaMoveS charaMoveS;
    Coroutine timerWaitBaloonChat;

    // Start is called before the first frame update
    void Start()
    {
        //chat
        chatT = chatBubble.GetChild(0).GetComponent<TMP_Text>();
        chatCG = chatBubble.GetComponent<CanvasGroup>();
        chatBubble.gameObject.SetActive(false);

        if (!photonView.IsMine)
        {
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<Rigidbody2D>().isKinematic = true;
            GetComponent<CharaMoveS>().enabled = false;
        }
        else
        {
            GameM.Instance.vcam.m_Follow = camPoint;
            charaMoveS = GetComponent<CharaMoveS>();
            anim = GetComponent<Animator>();
            //ui
            UIS.Instance.playerNameT.text = GlobalVarS.Instance.playerName;
        }

        if (GlobalVarS.Instance.modeGame == ModeGame.OFFLINE)
        {
            nameT.text = GlobalVarS.Instance.playerName;//photonView.Owner.NickName
        }
        else
        {
            nameT.text = photonView.Owner.NickName;//photonView.Owner.NickName
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name + ", tag: " + other.gameObject.tag);
        if (other.gameObject.CompareTag("door"))
        {
            DoorS doorS = other.gameObject.GetComponent<DoorS>();
            transform.position = doorS.GetDoorOutPosition();
            //cam
            if (doorS.doorType == DoorType.IN)
            {
                GameM.Instance.vcam.gameObject.SetActive(false);
                doorS.GetVCam().gameObject.SetActive(true);
                doorS.GetVCam().m_Follow = camPoint;
            }
            else
            {
                GameM.Instance.vcam.gameObject.SetActive(true);
                doorS.GetVCam().gameObject.SetActive(false);
            }

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!photonView.IsMine)
        {
            return;
        }

        Animating();
    }

    private void Animating()
    {
        anim.SetFloat("h", charaMoveS.getAxis().x);
        anim.SetFloat("v", charaMoveS.getAxis().y);
        anim.SetFloat("speed", charaMoveS.getAxis().sqrMagnitude);
    }

    #region Chat
    public void ShowChat(string msg)
    {
        photonView.RPC("PopUpChatRPC", RpcTarget.All, new object[] { msg });
    }

    [PunRPC]
    void PopUpChatRPC(string msg)
    {
        chatBubble.gameObject.SetActive(true);
        chatCG.DOKill();
        chatCG.alpha = 0f;
        chatCG.DOFade(1f, 0.35f);
        chatT.text = msg;

        RectTransform chatRect = chatBubble.GetComponent<RectTransform>();
        chatRect.DOKill();
        chatRect.anchoredPosition = Vector2.zero;
        chatRect.DOAnchorPosY(14.5f, 0.35f).SetDelay(0.1f);

        timerWaitBaloonChat = StartCoroutine(TimerChat());
    }

    IEnumerator TimerChat()
    {
        yield return new WaitForSeconds(3f);
        chatBubble.gameObject.SetActive(false);
    }
    #endregion
}
