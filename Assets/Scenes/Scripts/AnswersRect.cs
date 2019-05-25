using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

namespace ASpyInHarmWay
{
    public class AnswersRect : MonoBehaviour
    {

        [SerializeField]
        protected Button Answer1Btn;
        [SerializeField]
        protected Button Answer2Btn;

        [SerializeField]
        protected TextMeshProUGUI Answer1;
        [SerializeField]
        protected TextMeshProUGUI Answer2;

        [SerializeField]
        protected CanvasGroup CvGroup;


        private int answ1id, answs2id;
        private int choiseid;

        protected void Awake()
        {
            CvGroup.alpha = 0;
        }

        public void SetAnswers(string answ1, string answ2)
        {
            Answer1.text = answ1;
            Answer2.text = answ2;
        }


        public void SetAnswersIds(int answ1id, int answ2id)
        {
            this.answ1id = answ1id;
            this.answs2id = answ2id;
        }

        public void ShowAnswers(bool state)
        {
            CvGroup.DOFade((state)?1f:0f, 1.75f);
        }

        public void ChooseAnswer(int answerid)
        {
            switch (answerid)
            {
                case 1:
                    choiseid = answ1id;
                    break;
                case 2:
                    choiseid = answs2id;
                    break;
            }
        }

        public int GetAnswerId()
        {
            return choiseid;
        } 
    }
}

