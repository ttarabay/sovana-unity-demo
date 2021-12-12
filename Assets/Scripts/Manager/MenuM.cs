using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuM : MonoBehaviour
{
    public static MenuM Instance;

    [Header("[ canvas ]")]
    [SerializeField] GameObject canvasSelectAvatar;
    public GameObject canvasPlay;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;

        GlobalVarS.Instance.playerName = "SovanaUser" + Random.Range(0, 999);

        StarMenu();
    }

    #region menu Event
    public void StarMenu()
    {
        //audio
        SoundS.Instance.PlayBgm(0);
        //SoundS.Instance.StopBgm(1);
        
        canvasSelectAvatar.SetActive(true);
    }
    public void GotoGame()

    {
        SoundS.Instance.PlayBgm(0);
        UIEvents.OnAddressablesCheck("GameS");
    }
    #endregion
}
