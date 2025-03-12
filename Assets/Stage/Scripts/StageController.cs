using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using AppleGame.Objects;
using AppleGame.Util;

namespace AppleGame.Stage
{
    public class StageController : BehaviourBase
    {

#if UNITY_EDITOR
        protected override void OnBindSerialzedField()
        {

        }
#endif

        int _sum = 0;
        int _currentScore;
        float _maxTime = 120f;

        StageView _stageView;
        RectTransform _dragRectangle;
        Rect _dragRect;
        Vector2 _startPoint = Vector2.zero;
        Vector2 _endPoint = Vector2.zero;
        List<Apple> _selectedAppleList = new List<Apple>();

        public event Action playCompletionSfx;
        public event Action stopBgm;

        void Update()
        {
            if (_stageView.gameObject.activeSelf == false)
            {
                return;
            }

            if (Input.GetMouseButtonDown(0))
            {
                _startPoint = Input.mousePosition;

                _dragRect.Set(0, 0, 0, 0);
            }

            if (Input.GetMouseButton(0))
            {
                _endPoint = Input.mousePosition;

                // 마우스를 클릭한 상태로 드래그하는 동안 드래그 범위를 이미지로 표현
                DrawDragRectangle();
                CalculateDragRect();
                SelectApples();
            }

            if (Input.GetMouseButtonUp(0))
            {
                // Debug.Log($"총합 : {sum}");

                if (_sum == 10)
                {
                    // Debug.Log("10 완성");
                    int addScore = 0;
                    foreach (Apple apple in _selectedAppleList)
                    {
                        addScore++;
                        _stageView.DestroyApple(apple);
                    }

                    playCompletionSfx?.Invoke();
                    _stageView.SetGameScoreText(_currentScore += addScore);
                }
                else
                {
                    foreach (Apple apple in _selectedAppleList)
                    {
                        apple.OnDeselected();
                    }
                }

                // 마우스 클릭을 종료할 때 드래그 범위가 보이지 않도록
                // _startPoint, _endPoint 위치를 (0, 0)으로 설정하고 드래그 범위를 그린다
                _startPoint = Vector2.zero;
                _endPoint = Vector2.zero;
                DrawDragRectangle();
            }
        }

        public void Init(StageView stageView)
        {
            _stageView = stageView;
            _dragRectangle = _stageView.GetDragRectangle();
        }
        public void DrawDragRectangle()
        {
            // 드래그 범위를 나타내는 Image UI의 위치
            _dragRectangle.position = (_startPoint + _endPoint) * 0.5f;
            // 드래그 범위를 나타내는 Image UI의 크기
            _dragRectangle.sizeDelta = new Vector2(Mathf.Abs(_startPoint.x - _endPoint.x), Mathf.Abs(_startPoint.y - _endPoint.y));
        }

        private void CalculateDragRect()
        {
            if (Input.mousePosition.x < _startPoint.x)
            {
                _dragRect.xMin = Input.mousePosition.x;
                _dragRect.xMax = _startPoint.x;
            }
            else
            {
                _dragRect.xMin = _startPoint.x;
                _dragRect.xMax = Input.mousePosition.x;
            }

            if (Input.mousePosition.y < _startPoint.y)
            {
                _dragRect.yMin = Input.mousePosition.y;
                _dragRect.yMax = _startPoint.y;
            }
            else
            {
                _dragRect.yMin = _startPoint.y;
                _dragRect.yMax = Input.mousePosition.y;
            }
        }

        private void SelectApples()
        {
            _sum = 0;
            _selectedAppleList.Clear();
            foreach (Apple apple in _stageView._appleList)
            {
                // 사과의 중심을 드래그 영역에 포함해야 함
                if (_dragRect.Contains(apple.GetPosition()))
                {
                    apple.OnSelected();
                    _selectedAppleList.Add(apple);
                    _sum += apple.Number;
                }
                else
                {
                    apple.OnDeselected();
                }
            }
        }

        public IEnumerator TimeCounter()
        {
            float currentTime = _maxTime;

            while (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
                _stageView.SetTimeGauge(currentTime / _maxTime);
                Debug.Log(currentTime);

                yield return null;
            }

            GameOver();            
        }

        void GameOver()
        {
            stopBgm?.Invoke();
            _stageView.SetResultScoreText(_currentScore);
            _currentScore = 0;
            _stageView.SetGameScoreText(_currentScore);
            _stageView.Refresh();
            _stageView.HideInGamePanel();
            _stageView.ShowResultPanel();
        }
    }
}