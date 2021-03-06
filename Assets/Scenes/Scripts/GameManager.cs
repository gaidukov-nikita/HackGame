﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
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

        private int currentPulse;
        
        #region editor

        [SerializeField]
        protected ChatRect ChatterBox;
        [SerializeField]
        protected LampPuzzle LampPuzzle;
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

        [SerializeField] 
        protected TextMeshProUGUI pulseLabel;

        #endregion

        private void Awake()
        {
            Instance = this;
            LampPuzzle.OnMiniGameStart = MinigameLamp;
            LampPuzzle.OnMiniGameEnd = ConfirmMessage;
            cover.raycastTarget = false;
        }

        void Start()
        {
            Application.targetFrameRate = 60;
            currentPulse = 80;
            StartCoroutine(GameStartSeq());
        }

        void MinigameLamp()
        {
            showCover();
            LampPuzzle.gameObject.SetActive(true);
            LampPuzzle.CVGroup.alpha = 1;
        }

        protected void Update()
        {
            switch (GameState)
            {
                case STATES.WAITINGFORMESSAGE:
                case STATES.INMINIGAME:
              

                    MessageEvent = ChatterBox.GetNextMessage();

                    ChatterBox.ProcessMessage(MessageEvent);

                    break;

                case STATES.ANSWERMESSAGE:

                    ChatRect.MessageEvent me = new ChatRect.MessageEvent();
                    me.message = ChatterBox.GetAnswerString();
                    me.messagetype = ChatRect.messagetype.messageback;

                    ChatterBox.ProcessMessage(me);

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
            cover.raycastTarget = true;
            cover.DOFade(endValue, 0.5f);
            yield return new WaitForSeconds(0.5f);
            

        }
        
        public void showCover()
        {
            StartCoroutine(coverRutine(1.0f));
        }

        public void changePulse(int newDelta)
        {
            currentPulse += newDelta;
            pulseLabel.text = currentPulse.ToString();
            if (currentPulse > 160)
            {
                NotifRect.SetDeathLabel();
            }
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
            float batteryLife = 60*5.0f;
            
            
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

            GameState = STATES.WAITINGFORMESSAGE;
        }

        public void ConfirmMessage()
        {
            GameState = STATES.WAITINGFORMESSAGE;
        }


    }
}

