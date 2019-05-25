using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

public class PulseAnimation : MonoBehaviour
{
    private Tween _tween;
    private Image img;
    
    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        StartCoroutine(Animate());
    }
    
    protected IEnumerator Animate()
    {
        _tween = img.DOFade( 0.5f,1.0f).SetLoops(-1).SetEase(Ease.OutBack);
        
        yield return new WaitForSeconds(0.0f);
    }

}
