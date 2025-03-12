using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using AppleGame.Util;
using AppleGame.Objects;

namespace AppleGame.Stage
{
    public class StageView : BehaviourBase
    {
        [SerializeField] GameObject _panel;
        [SerializeField] GameObject _inGamePanel;
        [SerializeField] GameObject _resultPanel;
        [SerializeField] Apple _applePrefab;
        [SerializeField] Transform _appleParent;
        [SerializeField] RectTransform _dragRectangle;
        [SerializeField] TextMeshProUGUI _gameScoreText;
        [SerializeField] Image _timeGauge;
        [SerializeField] Button _resetButton;
        [SerializeField] TextMeshProUGUI _resultScoreText;

#if UNITY_EDITOR
        protected override void OnBindSerialzedField()
        {
            _panel = this.gameObject;

            GameObject inGamePanel = _panel.transform.Find("Panel - InGame").gameObject;
            if (inGamePanel == null)
            {
                Debug.LogError("Panel - InGame null");
                return;
            }
            _inGamePanel = inGamePanel;

            GameObject resultPanel = _panel.transform.Find("Panel - Result").gameObject;
            if (resultPanel == null)
            {
                Debug.LogError("Panel - Result null");
                return;
            }
            _resultPanel = resultPanel;

            Apple applePrefab = Resources.Load<Apple>(applePrefabPath);
            if (applePrefab == null)
            {
                Debug.LogError($"{applePrefabPath}에 프리팹이 없습니다.");
                return;
            }
            _applePrefab = applePrefab;

            Transform applesPanel = _inGamePanel.transform.Find("Panel - Apples");
            if (applesPanel == null)
            {
                Debug.LogError("Panel - Apples가 없습니다.");
                return;
            }
            _appleParent = applesPanel;

            RectTransform dragRectangle = _inGamePanel.transform.Find("Panel - DragRectangle").GetComponent<RectTransform>();
            if (dragRectangle == null)
            {
                Debug.LogError("Panel - DragRectangle이 없습니다.");
                return;
            }
            _dragRectangle = dragRectangle;

            TextMeshProUGUI gameScoreText = _inGamePanel.transform.Find("Text - Score").GetComponent<TextMeshProUGUI>();
            if (gameScoreText == null)
            {
                Debug.LogError("Text - Score가 없습니다.");
                return;
            }
            _gameScoreText = gameScoreText;

            Image timeGauge = _inGamePanel.transform.Find("Image - PlayTime").Find("Image - TimeGauge").GetComponent<Image>();
            if (timeGauge == null)
            {
                Debug.LogError("Image - TimeGauge가 없습니다.");
                return;
            }
            _timeGauge = timeGauge;

            Button resetButton = _resultPanel.transform.Find("Button - Reset").GetComponent<Button>();
            if (resetButton == null)
            {
                Debug.LogError("Button - Reset null");
                return;
            }
            _resetButton = resetButton;

            TextMeshProUGUI resultScoreText = _resultPanel.transform.Find("Text - ResultScore").GetComponent<TextMeshProUGUI>();
            if (resultScoreText == null)
            {
                Debug.LogError("Text - ResultScore null");
                return;
            }
            _resultScoreText = resultScoreText;
        }
#endif

        const string applePrefabPath = "Object/Apple";
        const int width = 17;
        const int height = 10;
        const int spacing = 20;

        int _currentScore;

        public List<Apple> _appleList { get; private set; }
        public event Action resetGame;

        ObjectSpawner<Apple> _appleSpawner;

        public RectTransform GetDragRectangle() => _dragRectangle;
        public void Init()
        {
            _appleList = new List<Apple>(170);
            _appleSpawner = new ObjectSpawner<Apple>();

            _appleSpawner.Init(_applePrefab);
            Refresh();
            ButtonAddListener();
            _resultPanel.SetActive(false);
            Hide();
        }

        void CreateApples()
        {
            if (_appleList == null)
            {
                Debug.LogError("StageView가 초기화되지 않았습니다.");
                return;
            }

            //프리팹의 크기를 구해서 간격(Spacing)만큼 더해준다.
            Vector2 size = _applePrefab.GetComponent<RectTransform>().sizeDelta;
            size += new Vector2(spacing, spacing);

            int sum = 0;
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Apple clone = _appleSpawner.CreateObject(_appleParent);
                    RectTransform rect = clone.GetComponent<RectTransform>();

                    float px = (-width * 0.5f + 0.5f + x) * size.x;
                    float py = (height * 0.5f - 0.5f - y) * size.y;
                    rect.anchoredPosition = new Vector2(px, py);

                    clone.Number = UnityEngine.Random.Range(1, 10);

                    //전체 합이 10의 배수가 되도록 마지막 사과의 숫자 설정
                    if (y == height - 1 && x == width - 1)
                    {
                        clone.Number = 10 - (sum % 10);
                    }

                    sum += clone.Number;

                    _appleList.Add(clone);
                }
            }

            if (_appleList[^1].Number == 10)
            {
                Debug.Log("마지막 사과 10");
                _appleList[^1].Number = 9;

                for (int i = _appleList.Count - 2; i >= 0; i--)
                {
                    if (_appleList[i].Number > 1)
                    {
                        _appleList[i].Number -= 1;
                        break;
                    }
                }
            }
        }

        public void Refresh()
        {
            if (_appleList.Count > 0)
            {
                foreach (Apple apple in _appleList)
                {
                    Destroy(apple.gameObject);
                }
            }

            _appleList.Clear();

            CreateApples();
        }

        public void DestroyApple(Apple removeItem)
        {
            _appleList.Remove(removeItem);
            Destroy(removeItem.gameObject);
        }

        void ButtonAddListener()
        {
            _resetButton.onClick.AddListener(() => _resultPanel.SetActive(false));
            _resetButton.onClick.AddListener(() => resetGame?.Invoke());
        }

        public void Show()
        {
            _panel.SetActive(true);
            _inGamePanel.SetActive(true);
        }

        public void Hide()
        {
            _panel.SetActive(false);
        }

        public void ShowResultPanel()
        {
            _resultPanel.SetActive(true);
        }

        public void HideInGamePanel()
        {
            _inGamePanel.SetActive(false);
        }

        public void SetTimeGauge(float value)
        {
            _timeGauge.fillAmount = value;
        }

        public void SetGameScoreText(int score)
        {
            _gameScoreText.text = score.ToString();
        }

        public void SetResultScoreText(float resultScore)
        {
            _resultScoreText.text = $"<color=red>Score</color>\n<color=green>{resultScore}</color>";
        }
    }
}