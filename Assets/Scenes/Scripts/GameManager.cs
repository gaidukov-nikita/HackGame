using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace ASpyInHarmWay
{
    public class GameManager : MonoBehaviour
    {
        public enum STATES { NONE, WAITINGFORMESSAGE, ANSWERMESSAGE, INMINIGAME }

        public static GameManager Instance;

        protected STATES GameState { get; set; }

        protected ChatRect.MessageEvent MessageEvent;
        

        #region editor

        [SerializeField]
        protected ChatRect ChatterBox;
        [SerializeField]
        protected NotificationRect NotifRect;

        [SerializeField]
        protected Image battery1;
        [SerializeField]
        protected Image battery2;
        [SerializeField]
        protected Image battery3;

        [SerializeField]
        protected Image cover;

        #endregion

        private void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            Application.targetFrameRate = 60;

            StartCoroutine(GameStartSeq());
            
        }

        protected void Update()
        {
            switch (GameState)
            {
                case STATES.WAITINGFORMESSAGE:

                 
                    MessageEvent = ChatterBox.GetNextMessage();

                    ChatterBox.ProcessMessage(MessageEvent);

                    break;

                case STATES.ANSWERMESSAGE:

                    MessageEvent = ChatterBox.GetNextMessage();

                    ChatterBox.ProcessMessage(MessageEvent);

                    break;

                case STATES.INMINIGAME:

                    break;

                default:
                    break;
            }

            GameState = STATES.NONE;
        }

        public void hideCover()
        {
            StartCoroutine(coverRutine(0.0f));
        }

        protected IEnumerator coverRutine(float endValue)
        {
            cover.DOFade(endValue, 0.5f);
            yield return new WaitForSeconds(0.5f);
        }
        
        public void showCover()
        {
            StartCoroutine(coverRutine(1.0f));

        }

        
        protected IEnumerator GameStartSeq()
        {
            yield return new WaitForEndOfFrame();

            NotifRect.SetNotifLabel();

            yield return new WaitForSeconds(10.5f);

            GameState = STATES.WAITINGFORMESSAGE;

            StartCoroutine(BetterySeq());
        }
        
        protected IEnumerator BetterySeq()
        {
            float batteryLife = 6.0f;
            
            
            yield return new WaitForSeconds(batteryLife/3.0f);

            battery3.DOFade(0.0f, 0.5f);
            
            yield return new WaitForSeconds(batteryLife/3.0f);
            
            battery2.DOFade(0.0f, 0.5f);
            
            yield return new WaitForSeconds(batteryLife/3.0f);
            
            battery1.DOFade(0.0f, 0.5f);

            yield return new WaitForSeconds(1.0f);

            ChatterBox.gameObject.SetActive(false);
            NotifRect.SetDeathLabel();
        }
        
        protected IEnumerator GameStateAnswerMessage()
        {
            do
            {
                yield return new WaitForEndOfFrame();

            } while (true);

        }

        public void ConfirmAnswer()
        {
            ChatterBox.ConfirmAnswer();

            GameState = STATES.ANSWERMESSAGE;
        }

        public void ConfirmMessage()
        {
            GameState = STATES.WAITINGFORMESSAGE;
        }


    }
}

