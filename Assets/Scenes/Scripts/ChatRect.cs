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
            public int nextid = -1;
        }

        [System.Serializable]
        public class AnswerEvent
        {
            public int id;
            public string answer;
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
        private float _chatDeltaY = 0;

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
                _chatDeltaY = chatDeltaY - lastmsgRect.sizeDelta.y;
            else
                _chatDeltaY = chatDeltaY;

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
                        currChatMsg.PrepareMessage(me.message, 
                            
                            new Vector2(currChatMsg.thisrect.localPosition.x,
                            lastmsgRect.localPosition.y
                            +_chatDeltaY));

                        lastmsgRect = currChatMsg.thisrect;

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
                            + _chatDeltaY));

                        lastmsgRect = currChatMsg.thisrect;
                    }

                    currAnswers.SetAnswers(me.answers[0].answer, me.answers[1].answer);
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
                            + _chatDeltaY));

                        lastmsgRect = currChatMsg.thisrect;
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
            currAnswers.ShowAnswers(false);
        }

        private void ConfirmMessageEnd() {

            if (currMessage.nextid == -1)
                currentmessageId++;
            else
                currentmessageId = currMessage.nextid;
            GameManager.Instance.ConfirmMessage();

        }

        private void ConfirmMessageEndWithAnswers()
        {
            currAnswers.ShowAnswers(true);
        }

    }
}

