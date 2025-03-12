using UnityEngine;
using UnityEngine.UI;
using TMPro;

using AppleGame.Util;

namespace AppleGame.Objects
{
    public class Apple : BehaviourBase
    {
        [SerializeField] TextMeshProUGUI _textNumber;
        [SerializeField] Image _appleImage;
        [SerializeField] RectTransform _rectTransform;
#if UNITY_EDITOR
        protected override void OnBindSerialzedField()
        {
            TextMeshProUGUI textNumber = this.transform.Find("Text - Number").GetComponent<TextMeshProUGUI>();
            if(textNumber == null)
            {
                Debug.LogError("Text - Number가 없습니다.");
                return;
            }
            _textNumber = textNumber;

            Image appleImage = GetComponent<Image>();
            if(appleImage == null)
            {
                Debug.LogError("컴포넌트에 Image를 추가 해주세요.");
                return;
            }
            _appleImage = appleImage;

            RectTransform rectTransform = GetComponent<RectTransform>();
            if(rectTransform == null)
            {
                Debug.LogError("객체에 RectTransform이 없습니다. 추가해 주세요.");
                return;
            }
            _rectTransform = rectTransform;
        }
#endif
        int _number = 0;
        public int Number
        {
            get => _number;

            set
            {
                _number = value;
                _textNumber.text = _number.ToString();
            }
        }

        public Vector3 GetPosition() => _rectTransform.position;

        // 마우스 드래그 영역 내에 해당 사과가 포함될 때 호출
        public void OnSelected()
        {
            _appleImage.color = Color.blue;
        }

        // 마우스 드래그 영역을 벗어날 때 호출
        public void OnDeselected()
        {
            _appleImage.color = Color.red;
        }
    }
}
