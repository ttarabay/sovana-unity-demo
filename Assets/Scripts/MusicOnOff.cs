using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicOnOff : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        UnityEngine.UI.Toggle ui_toggle = transform.GetComponent<UnityEngine.UI.Toggle>();
        ui_toggle.isOn = !GlobalVarS.Instance.IsMusicOn;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ValueChanged(bool isOnOff)
    {
        UnityEngine.UI.Toggle ui_toggle = transform.GetComponent<UnityEngine.UI.Toggle>();
        GlobalVarS.Instance.IsMusicOn = !ui_toggle.isOn;
        if(!ui_toggle.isOn)
        {
            SoundS.Instance.PlayBgm(0);
        }
        else
        {
            SoundS.Instance.StopBgm();
        }
    }
}
