using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tutorial
{
    public class TutorialPresenter : MonoBehaviour
    {
        [SerializeField] private List<RectTransform> _tutorialList = new List<RectTransform>();
        [SerializeField] private RectTransform _rt;
        [SerializeField] private Button _prevButton;
        [SerializeField] private Button _nextButton;
        private int _tapCount;

        private void Start()
        {
            _tapCount = 0;
            var count = 0;
            foreach (var tutorial in _tutorialList)
            {
                if (count == 0)
                {
                    tutorial.gameObject.SetActive(true);
                }
                else
                {
                    tutorial.gameObject.SetActive(false);
                }

                count++;
            }

            _prevButton.OnClickAsObservable().Subscribe(_ =>
           _rt.DOAnchorPosX(800f, 0.2f).SetRelative().OnComplete(() => SetPrevTutorialList()));

            _nextButton.OnClickAsObservable().Subscribe(_ =>
            _rt.DOAnchorPosX(800f, 0.2f).SetRelative().OnComplete(() => SetNextTutorialList()));
        }

        private void SetPrevTutorialList()
        {
            _tapCount -= 1;
            if (_tapCount < 0)
            {
                _tapCount = _tutorialList.Count - 1;
            }

            foreach (var tutorial in _tutorialList)
            {
                tutorial.gameObject.SetActive(false);
            }

            _tutorialList[_tapCount].gameObject.SetActive(true);

            this._rt.anchoredPosition = Vector2.zero;
        }

        private void SetNextTutorialList()
        {
            _tapCount += 1;
            if (_tapCount > _tutorialList.Count - 1)
            {
                _tapCount = 0;
            }

            foreach (var tutorial in _tutorialList)
            {
                tutorial.gameObject.SetActive(false);
            }

            _tutorialList[_tapCount].gameObject.SetActive(true);

            this._rt.anchoredPosition = Vector2.zero;
        }
    }
}
