using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;

namespace ASpyInHarmWay
{
    public class ChatMessage : MonoBehaviour
    {
        [SerializeField]
        protected float _Messagetime;
        [SerializeField]
        protected float _RefHeight;
        [SerializeField]
        protected int _RefTextLineLength = 7;

        [SerializeField]
        protected TextMeshProUGUI messageText;
        [SerializeField]
        protected TextMeshProUGUI messageLoading;
        [SerializeField]
        protected GameObject messageHead;
        [SerializeField]
        protected Image messageBubble;

        public RectTransform thisrect { get; protected set; }
        

        private string currMessage;
        private int textLength;

        public FastAction onMessageEnd;

        private void Awake()
        {
            thisrect = GetComponent<RectTransform>();
            messageText = GetComponentInChildren<TextMeshProUGUI>();
            onMessageEnd = new FastAction();
        }

        private void Start()
        {
            //PrepareMessage("ololo ya voditel");
        }

        public void PrepareMessage(string text, Vector2 pos)
        {
            Debug.Log(text.Length);
             
            textLength = text.Length;
            currMessage = text;
            messageText.text = string.Empty;
            messageHead?.SetActive(false);
            messageBubble.color = Color.clear;

            thisrect.localPosition = pos;

            StartCoroutine(PrepareMessageSeq());

        }

        private IEnumerator PrepareMessageSeq()
        {
            float disttime = ((Time.fixedDeltaTime * textLength * _Messagetime));
            float currtime = 0f;
            int dots = 0;

            messageLoading.enabled = true;
            messageHead?.SetActive(true);
            do
            {
                if (dots == 3)
                {
                    messageLoading.text = string.Empty;
                    dots = 0;
                }   
                else
                {
                    messageLoading.text += ".";
                    dots++;
                }

                currtime += Time.fixedDeltaTime * _Messagetime;
                yield return new WaitForSeconds(Time.fixedDeltaTime * _Messagetime);


                //Debug.Log(currtime);
                //Debug.Log(disttime);
            }
            while (currtime < disttime);


            messageLoading.enabled = false;
            yield return new WaitForEndOfFrame();
            SetChatMessage(currMessage);
        }

        public void SetChatMessage (string text)
        {
            messageText.text = currMessage;
            OnSetChatMessage();
        }

        protected IEnumerator ShowLabel()
        {
            do
            {
                messageText.maxVisibleCharacters++;
                yield return new WaitForSeconds(Time.fixedDeltaTime * 2f);

            } while (messageText.maxVisibleCharacters != messageText.text.Length);

           
            yield return new WaitForSeconds(1f);

          

        }

        private void OnSetChatMessage()
        {
            //messageText.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, thisrect.sizeDelta.x)
            //Debug.Log(messageText.li);

            int lines = textLength / _RefTextLineLength;
            float newheight = lines * _RefHeight;

            thisrect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newheight);

            Debug.Log(lines);

            messageBubble.color = Color.white;

            StartCoroutine(ShowLabel());

            onMessageEnd?.Call();
        }

    }
}

