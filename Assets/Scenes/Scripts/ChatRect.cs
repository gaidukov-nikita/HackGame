using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ASpyInHarmWay
{
    public class ChatRect : MonoBehaviour
    {

        public enum messagetype { message, answer, messageback, minigame}       
        public enum minigametype { none, game1, game2, game3}

        [System.Serializable]
        public class MessageEvent
        {
            public int id;
            [Multiline]
            public string message;
            public messagetype messagetype;
            public AnswerEvent[] answers;
            public minigametype minigame;
        }

        [System.Serializable]
        public class AnswerEvent
        {
            public int id;
            public string answer;
            public int pulseChange = 0;
        }

        protected RectTransform thisrect;
        protected RectTransform lastmsgRect;


        protected MessageEvent currMessage;
        protected ChatMessage currChatMsg;
        

        [SerializeField]
        protected GameObject[] MessagesPrefab;

        [SerializeField]
        protected MessageEvent[] Messages;
        [SerializeField]
        protected AnswersRect currAnswers;

        [SerializeField]
        protected float chatDeltaY;

        int currentmessageId = -1;
        Dictionary<int, MessageEvent> MessagesObj;

        private void Awake()
        {
            thisrect = GetComponent<RectTransform>();
            MessagesObj = new Dictionary<int, MessageEvent>();

            foreach (var item in Messages)
            {
                MessagesObj.Add(item.id, item);
            }
        }


        public MessageEvent GetNextMessage()
        {
            if (currentmessageId == -1)
            {
                currentmessageId = 0;
            }

            return MessagesObj[currentmessageId];
        }
         

        public void ProcessMessage(MessageEvent me)
        {
            currMessage = me;
            if (lastmsgRect != null)
            chatDeltaY = lastmsgRect.sizeDelta.y;

            switch (currMessage.messagetype)
            {
                case messagetype.message:

                    currChatMsg = Instantiate(MessagesPrefab[0], thisrect).GetComponent<ChatMessage>(); //spy message 

                    if (lastmsgRect == null)
                    {
                        lastmsgRect = currChatMsg.thisrect;
                        currChatMsg.PrepareMessage(me.message, lastmsgRect.localPosition);
                    }
                    else
                    {
                        lastmsgRect = currChatMsg.thisrect;
                        currChatMsg.PrepareMessage(me.message, 
                            
                            new Vector2(currChatMsg.thisrect.localPosition.x,
                            lastmsgRect.localPosition.y
                            +chatDeltaY));
                       
                    }
                    currChatMsg.onMessageEnd.Add(ConfirmMessageEnd);
                    break;

                case messagetype.answer:

                    currChatMsg = Instantiate(MessagesPrefab[0], thisrect).GetComponent<ChatMessage>(); //message with answers 

                    if (lastmsgRect == null)
                    {
                        // currChatMsg.thisrect = 
                    }
                    else
                    {
                        currChatMsg.PrepareMessage(me.message, new Vector2(currChatMsg.thisrect.localPosition.x,
                            lastmsgRect.localPosition.y
                            + chatDeltaY));
                    }

                    currAnswers.SetAnswers(me.answers[0].answer, me.answers[1].answer);
                    currAnswers.SetPulseChanges(me.answers[0].pulseChange, me.answers[1].pulseChange);
                    currAnswers.SetAnswersIds(me.answers[0].id, me.answers[1].id);
                    currChatMsg.onMessageEnd.Add(ConfirmMessageEndWithAnswers);

                    break;

                case messagetype.messageback:

                    currChatMsg = Instantiate(MessagesPrefab[1], thisrect).GetComponent<ChatMessage>(); //answering the message

                    if (lastmsgRect == null)
                    {
                        // currChatMsg.thisrect = 
                    }
                    else
                    {
                        currChatMsg.PrepareMessage(me.message, new Vector2(currChatMsg.thisrect.localPosition.x,
                            lastmsgRect.localPosition.y
                            + chatDeltaY));
                    }

                    break;

                case messagetype.minigame:

                    //
                    Debug.Log("Show minigame via other gameobject");

                    break;

  
            }
        }

        public void ConfirmAnswer()
        {
            currentmessageId = currAnswers.GetAnswerId();

            //todo call here the --- gamemanager pulse changing

            //currAnswers.GetPulseChoise();

            currAnswers.ShowAnswers(false);
        }

        private void ConfirmMessageEnd() {

            currentmessageId++;
            GameManager.Instance.ConfirmMessage();

        }

        private void ConfirmMessageEndWithAnswers()
        {
            currAnswers.ShowAnswers(true);
        }

    }
}

