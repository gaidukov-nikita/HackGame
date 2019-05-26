using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using DG.Tweening.Plugins.Options;
using UnityEngine.UI;

namespace ASpyInHarmWay
{
    public class NotificationRect : MonoBehaviour
    {
        [SerializeField]
        protected TextMeshProUGUI NotifLabel;
        [SerializeField]
        protected string NotifText = "connecting...";
        [SerializeField]
        public float notifTime = 1.5f;

        [SerializeField]
        protected RectTransform loadingimg2;

        [SerializeField]
        protected RectTransform loadingimg1;
        
        private Image loadingimg1Image;
        private Image loadingimg2Image;

        private Tween _tween;
        private Tween _tween2;
        private Color defaultTextColor;  
            
        private void Awake()
        {
            NotifLabel.text = string.Empty;
            defaultTextColor = new Color(0.0f, 0.99f, 0.0f, 1.0f);
        }

        public void SetNotifLabel()
        {
            NotifLabel.text = NotifText;

            NotifLabel.maxVisibleCharacters = 0;
            
            loadingimg1Image = loadingimg1.GetComponent<Image>();
            loadingimg2Image = loadingimg2.GetComponent<Image>();
            
            StartCoroutine(ShowLabel());
        }
        
        public void SetDeathLabel()
        {
            StartCoroutine(deathSeq());
        }

        protected IEnumerator ShowLabel()
        {
            _tween = loadingimg2.DOScale(1.2f, .5f).SetLoops(-1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.OutSine);
            _tween2 = loadingimg1Image.DOFade(0.5f, 1.0f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

            
            // НАЙДЕНО УСТРОЙСТВО
            
            NotifLabel.text = "[НАЙДЕНО УСТРОЙСТВО]";
            NotifLabel.maxVisibleCharacters = 0;
            
            do
            {
                NotifLabel.maxVisibleCharacters++;
                yield return new WaitForSeconds(Time.fixedDeltaTime*2f);

            } while (NotifLabel.maxVisibleCharacters != NotifLabel.text.Length);

            yield return new WaitForSeconds(2.0f);

            
            // ПОДКЛЮЧЕНИЕ....
            
            NotifLabel.text = "[ПОДКЛЮЧЕНИЕ...]";
            NotifLabel.maxVisibleCharacters = 0;
            NotifLabel.DOColor(defaultTextColor, 0.0f);
            
            do
            {
                NotifLabel.maxVisibleCharacters++;
                yield return new WaitForSeconds(Time.fixedDeltaTime*2f);

            } while (NotifLabel.maxVisibleCharacters != NotifLabel.text.Length);
            
            yield return new WaitForSeconds(2.0f);

            
            // AGENT ПРИСОЕДИНЯЕТСЯ К ЧАТУ.....
            
            NotifLabel.text = "[ПРИСОЕДИНЯЕТСЯ AGENT]";
            NotifLabel.maxVisibleCharacters = 0;
            NotifLabel.DOColor(defaultTextColor, 0.0f);
            
            do
            {
                NotifLabel.maxVisibleCharacters++;
                yield return new WaitForSeconds(Time.fixedDeltaTime*2f);

            } while (NotifLabel.maxVisibleCharacters != NotifLabel.text.Length);
            
            yield return new WaitForSeconds(2.0f);
            NotifLabel.DOColor(Color.clear, 1.0f);

            _tween2.Kill();
            loadingimg1Image.DOFade(0.0f, 1.0f);
            loadingimg2Image.DOFade(0.0f, 1.0f);
            
            yield return new WaitForSeconds(1.0f);

            _tween.Kill();

            loadingimg1.gameObject.SetActive(false);
            loadingimg2.gameObject.SetActive(false);
            
        }

        protected IEnumerator deathSeq()
        {
            loadingimg1.gameObject.SetActive(true);
            loadingimg2.gameObject.SetActive(true);
            
            loadingimg1Image.DOFade(1.0f, 0.0f);
            loadingimg2Image.DOFade(1.0f, 0.0f);
            NotifLabel.DOColor(defaultTextColor, 0.0f);
            
            _tween = loadingimg2.DOScale(1.2f, .5f).SetLoops(-1).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.OutSine);
            _tween2 = loadingimg1Image.DOFade(0.5f, 1.0f).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.Linear);

            
            // СВЯЗЬ С УСТРОЙСТВОМ ПОТЕРЯНА
            
            NotifLabel.text = "[СВЯЗЬ С УСТРОЙСТВОМ ПОТЕРЯНА]";
            NotifLabel.maxVisibleCharacters = 0;
            
            do
            {
                NotifLabel.maxVisibleCharacters++;
                yield return new WaitForSeconds(Time.fixedDeltaTime*2f);

            } while (NotifLabel.maxVisibleCharacters != NotifLabel.text.Length);

            yield return new WaitForSeconds(2.0f);

        }
    }
}

