using UnityEngine;

public enum ModeGame
{
    OFFLINE, ONLINE
}

public class GlobalVarS : MonoBehaviour
{
    public static GlobalVarS Instance;

    [Header("[ api ]")]
    public string urlApi;
    public string tokenStr;
    [Header("[ game ]")]
    public ModeGame modeGame;
    public string gameVersion;
    public string roomName;
    [Space]
    public int MaxPlayers;
    [Header("[ player ]")]
    public string playerName;
    public string playerId;
    public int playerCharId;
    public bool isPlayerStartChat = false;
    [Header("[ var ]")]
    public bool isClickLeftGame;
    public Vector2 posChara;
    public bool isBackFromGame;
    [Space]
    public bool IsMusicOn;
    public bool IsSoundOn;

    // Start is called before the first frame update
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
