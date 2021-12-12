using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AddressableSceneS : MonoBehaviour
{
    public static AddressableSceneS Instance;

    [SerializeField] GameObject panelDownload;
    [SerializeField] TMP_Text progressT;
    [SerializeField] Image progressImg;

    bool isReadyToLoad = true;
    SceneInstance tempScene;
    string tempSceneName;

    Canvas canvas;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            canvas = GetComponent<Canvas>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        UIEvents.OnAddressablesCheckE += CheckAddressables;


    }

    private void OnDisable()
    {
        UIEvents.OnAddressablesCheckE -= CheckAddressables;
    }

    #region Addressables
    void CheckAddressables(string _sceneName)
    {
        panelDownload.SetActive(true);

        canvas.worldCamera = Camera.main;
        tempSceneName = _sceneName;

        progressT.text = "checking addressable assets";
        StartCoroutine(LoadAddressableScene());

        progressT.color = new Color(progressT.color.r, progressT.color.g, progressT.color.b, 1f);
        progressT.DOColor(new Color(progressT.color.r, progressT.color.g, progressT.color.b, 0.25f), 0.35f).SetLoops(6, LoopType.Yoyo);
    }

    IEnumerator UnloadAddressableScene()
    {
        var downloadScene = Addressables.UnloadSceneAsync(tempScene, true);
        downloadScene.Completed += SceneUnloadComplete;
        Debug.Log("Starting scene unLoad");

        while (!downloadScene.IsDone)
        {
            yield return null;
        }

        StartCoroutine(LoadAddressableScene());
    }

    private void SceneUnloadComplete(AsyncOperationHandle<SceneInstance> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("success unload scene");
            isReadyToLoad = true;
            tempScene = new SceneInstance();
        }
        else
        {
            Debug.Log("Failed " + obj.DebugName);
        }
    }

    IEnumerator LoadAddressableScene()
    {
        var downloadScene = Addressables.LoadSceneAsync(tempSceneName, LoadSceneMode.Single);
        downloadScene.Completed += SceneDownloadComplete;
        Debug.Log("Starting scene download");

        while (!downloadScene.IsDone)
        {
            var status = downloadScene.GetDownloadStatus();
            float progress = status.Percent;
            progressImg.fillAmount = progress;
            //text
            progressT.text = "Loading..." + Mathf.Round(progress*100f) + "%";
            yield return null;
        }

        Debug.Log("download complete");
        progressImg.fillAmount = 1f;
        progressT.text = "Loading...100%";
    }

    private void SceneDownloadComplete(AsyncOperationHandle<SceneInstance> obj)
    {
        if (obj.Status == AsyncOperationStatus.Succeeded)
        {
            Debug.Log("success load scene");
            tempScene = obj.Result;
            isReadyToLoad = false;

           
            //hide
            panelDownload.SetActive(false);
        }
        else
        {
            Debug.Log("Failed " + obj.DebugName);
        }
    }
    #endregion

}
