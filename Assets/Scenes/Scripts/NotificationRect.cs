using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

namespace ASpyInHarmWay
{
    public class NotificationRect : MonoBehaviour
    {
        [SerializeField]
        protected TextMeshProUGUI NotifLabel;
        [SerializeField]
        protected string NotifText = "connecting...";
        [SerializeField]
        public float NotifTime = 1.5f;

        [SerializeField]
        protected RectTransform loadingimg1;
        [SerializeField]
        protected RectTransform loadingimg2;

        private Tween _tween;
        private Tween _tween2;

        private void Awake()
        {
            NotifLabel.text = string.Empty;
        }

        public void SetNotifLabel()
        {
            NotifLabel.text = NotifText;

            NotifLabel.maxVisibleCharacters = 0;

            StartCoroutine(ShowLabel());
        }

        protected IEnumerator ShowLabel()
        {
            _tween = loadingimg2.DOScale(1.2f, .5f).SetLoops(-1).SetLoops(-1, LoopType.Yoyo);
            _tween2 = loadingimg1.DORotate(new Vector3(0f, 0f, -360f), 1.5f, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);

            do
            {
                NotifLabel.maxVisibleCharacters++;
                yield return new WaitForSeconds(Time.fixedDeltaTime*2f);

            } while (NotifLabel.maxVisibleCharacters != NotifLabel.text.Length);

            yield return new WaitForSeconds(Time.fixedDeltaTime * 10f);

            NotifLabel.DOColor(Color.clear, NotifTime);

  

            yield return new WaitForSeconds(NotifTime);

            _tween.Kill();
            _tween2.Kill();

            loadingimg1.gameObject.SetActive(false);
            loadingimg2.gameObject.SetActive(false);



        }
    }
}

