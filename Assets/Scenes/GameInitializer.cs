using UnityEngine;

using AppleGame.Singleton;

namespace AppleGame
{
    public class GameInitializer : MonoBehaviour
    {
        const string uiCanvasPath = "UI/UICanvas";

        void Start()
        {
            Canvas uiCanvas = Resources.Load<Canvas>(uiCanvasPath);
            if(uiCanvas == null)
            {
                Debug.LogError($"{uiCanvasPath}에 UICanvas가 없습니다.");
                return;
            }
            Canvas _uiCanvas = Object.Instantiate(uiCanvas);

            LobbyManager.Instance.Init(_uiCanvas);
            StageManager.Instance.Init(_uiCanvas);
            SoundManager.Instance.Init();
        }
    }
}
