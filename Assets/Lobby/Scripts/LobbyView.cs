using System;
using UnityEngine;
using UnityEngine.UI;

using AppleGame.Util;

namespace AppleGame.Lobby
{
    public class LobbyView : BehaviourBase
    {
        [SerializeField] GameObject _panel;
        [SerializeField] Button _startButton;

#if UNITY_EDITOR
        protected override void OnBindSerialzedField()
        {
            _panel = gameObject;

            Button startButton = _panel.transform.Find("Button - GameStart").GetComponent<Button>();
            if (startButton == null)
            {
                Debug.LogError("Button - Start가 없습니다.");
                return;
            }
            _startButton = startButton;
        }
#endif
        public event Action showStageView;
        public event Action startTimeCounter;
        public event Action playBgm;

        public void Init()
        {
            ButtonAddListener();
            Show();
        }

        public void Show()
        {
            _panel.SetActive(true);
        }

        public void Hide()
        {
            _panel.SetActive(false);
        }

        void ButtonAddListener()
        {
            if (_startButton == null)
            {
                Debug.LogError("Start Button null");
                return;
            }

            _startButton.onClick.AddListener(Hide);
            _startButton.onClick.AddListener(() => showStageView?.Invoke());
            _startButton.onClick.AddListener(() => startTimeCounter?.Invoke());
            _startButton.onClick.AddListener(() => playBgm?.Invoke());
        }
    }
}
