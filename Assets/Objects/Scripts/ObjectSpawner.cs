using UnityEngine;

namespace AppleGame.Objects
{
    public class ObjectSpawner<T> where T : Component
    {
        T _objectPrefab;


        public void Init(T objectPrefab)
        {
            if(objectPrefab == null)
            {
                Debug.LogError("프리팹이 null 입니다.");
                return;
            }

            _objectPrefab = objectPrefab;
        }
        
        public T CreateObject(Transform transform)
        {
            return UnityEngine.Object.Instantiate(_objectPrefab, transform);
        }

        public T CreateObject(RectTransform rectTransform)
        {
            return UnityEngine.Object.Instantiate(_objectPrefab, rectTransform);
        }
    }
}
