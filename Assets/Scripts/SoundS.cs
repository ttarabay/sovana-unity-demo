using UnityEngine;

public class SoundS : MonoBehaviour 
{
	public static SoundS Instance;
    //[SerializeField] int maxBgm = 2;
	GameObject audioBgm;
	GameObject audioSFX;

    public AudioClip m_siteBackSound;
    public AudioClip m_gameBackSound;
    /*
    int currProgress = 0;
    [SerializeField] int maxProgress = 0;
    [Header("[ ui ]")]
    [SerializeField] GameObject loadingCanvas;
    [SerializeField] Image progressBar;
    */
    // Use this for initialization
    void Awake () 
	{
		if (Instance == null) {
			Instance = this;
		} else {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (this);

		audioBgm = transform.GetChild (0).gameObject;
		audioSFX = transform.GetChild (1).gameObject;
	}

    /*
    private void Start()
    {
        currProgress = 0;
        maxProgress = maxBgm;
        //StartCoroutine(LoadAsset());
    }
    #region MyRegion

    IEnumerator LoadAsset()
    {
        //bgm
        for (int i = 0; i < maxBgm; i++)
        {
            currProgress++;
            progressBar.fillAmount = currProgress / maxProgress;

            var BgmClip = Addressables.LoadAssetAsync<AudioClip>("BGM" + i);
            yield return BgmClip;
            audioBgm.transform.GetChild(i).GetComponent<AudioSource>().clip = BgmClip.Result;
        }

        loadingCanvas.SetActive(false);
        MenuM.Instance.StarMenu();
    }
    #endregion
    */
    
    // Update is called once per frame
    public void PlayBgm (int ke) 
	{
        if (!GlobalVarS.Instance.IsMusicOn)
        {
            return;
        }
		audioBgm.transform.GetChild (ke).GetComponent<AudioSource> ().Play ();
	}
	// Update is called once per frame
	public void StopBgm ()
    {
       
        
        for ( int idx = 0; idx < audioBgm.transform.childCount; idx++)
        {
            audioBgm.transform.GetChild(idx).GetComponent<AudioSource>().Stop();
        }
        
	}

	// Update is called once per frame
	public void SetVolBgm (int ke, float vol)
    {
        if (!GlobalVarS.Instance.IsMusicOn)
        {
            return;
        }
        audioBgm.transform.GetChild (ke).GetComponent<AudioSource> ().volume = vol;
	}

	// Update is called once per frame
	public void PlaySfx (int ke)
    {
        if (!GlobalVarS.Instance.IsSoundOn)
        {
            return;
        }
        audioSFX.transform.GetChild (ke).GetComponent<AudioSource> ().Play ();
	}

    public void StopSfx(int ke)
    {
        audioSFX.transform.GetChild(ke).GetComponent<AudioSource>().Stop();
    }

    public bool IsPlayingSfx(int ke)
    {
        return audioSFX.transform.GetChild(ke).GetComponent<AudioSource>().isPlaying;
    }
}
