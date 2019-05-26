using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FantomLib;
using TMPro;

public class LampPuzzle : MonoBehaviour
{
    #region Editor only
    [Header("To make this work change the AndroidManifest for the Fantom Plugin")]
    [SerializeField]
    protected CanvasGroup CVGroup;
    [SerializeField]
    protected Image lampLight;
    [SerializeField]
    protected Image lampBgCode;

    [SerializeField]
    protected float lampReqLuxValue;
    [SerializeField]
    protected float lampHoldTime = 5f;
    [SerializeField]
    protected AudioClip lampClickSFX;
    [SerializeField]
    protected AudioClip lampDoneSFX;

    [SerializeField]
    protected TextMeshProUGUI lampdebug;

    #endregion

    #region Class Variable

    private float luxVal;
    private float holdTime;
    private Color curColor;
    #endregion

    #region Event for Minigame

    public delegate void OnLampPuzzleEvent();
    public static OnLampPuzzleEvent OnMiniGameStart;
    public static OnLampPuzzleEvent OnMiniGameEnd;

    #endregion

    private void Awake()
    {
        luxVal = 100;
        holdTime = 0;
        curColor = Color.white;
    }

    private void Start()
    {
        StartCoroutine(WaitForLuxChange());
    }

    public void OnLightChangedEvent(int type, float[] values)
    {
        Debug.Log("OnLightChangedEvent = " + type);
        foreach (var item in values)
        {
            Debug.Log("-------------");
            Debug.Log(item); 
        }
    }

    public void OnLightSensorChanged(float val)
    {
        //Debug.Log("OnLightSensorChanged = " + val);
       
        luxVal = val;
    }

    public void OnLightError(string s)
    {
        Debug.Log("OnLightError = " + s);
    }

    private void PlayClick()
    {
        AudioSource.PlayClipAtPoint(lampClickSFX, Vector3.zero);
    }

    private void PlayDone()
    {
        AudioSource.PlayClipAtPoint(lampDoneSFX, Vector3.zero);
    }

    protected IEnumerator WaitForLuxChange()
    {
        do
        {

        yield return new WaitForSeconds(Time.fixedDeltaTime * 10f);

        lampdebug.text = luxVal.ToString();
        
        if (luxVal <= lampReqLuxValue)
        {
            PlayClick();
            holdTime += (Time.fixedDeltaTime *10f);

            curColor = new Color(1f, 1f, 1f, (lampHoldTime - holdTime) / lampHoldTime);
            lampLight.color = lampBgCode.color = curColor;

            if (holdTime >= lampHoldTime)
            {
                    PlayDone();

                    yield return new WaitForSeconds(3f);

                    CVGroup.alpha = 0f; //todo fade

                    yield return new WaitForSeconds(2f);

                    OnMiniGameEnd?.Invoke();

                    yield break;
            }
        }

        yield return new WaitForEndOfFrame();

        } while (true);
    }

}
