using UnityEngine;
using UnityEngine.UI;

public class SelectAvatarS : MonoBehaviour
{
    [SerializeField] Transform avatarCont;
    public GameObject m_uiPlayerName;
    public GameObject m_uiAvatarView;
    public TMPro.TMP_InputField m_uiInputplayerName;
    // Start is called before the first frame update
    void Start()
    {
        
        for (int i = 0; i < avatarCont.childCount; i++)
        {
            Button btn = avatarCont.GetChild(i).GetComponent<Button>();
            int id = i;
            btn.onClick.AddListener(()=>ClickAvatar(id));
        }
    }
    public void OnEnable()
    {
        m_uiAvatarView.SetActive(true);
        m_uiPlayerName.SetActive(false);
    }
    public void OnStart()
    {
        GlobalVarS.Instance.playerName = m_uiInputplayerName.text;
        MenuM.Instance.canvasPlay.SetActive(true);
        gameObject.SetActive(false);
        
    }
    private void ClickAvatar(int id)
    {
        GlobalVarS.Instance.playerCharId = id;

        m_uiAvatarView.SetActive(false);
        m_uiPlayerName.SetActive(true);

    }
}
