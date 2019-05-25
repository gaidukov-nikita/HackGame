using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        protected IEnumerator GameStartSeq()
        {
            yield return new WaitForEndOfFrame();

            NotifRect.SetNotifLabel();

            yield return new WaitForSeconds(NotifRect.NotifTime);

            GameState = STATES.WAITINGFORMESSAGE;

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

