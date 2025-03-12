using UnityEngine;

using AppleGame.Lobby;

namespace AppleGame.Singleton
{
    public class LobbyManager
    {
        static LobbyManager _instance;
        public static LobbyManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LobbyManager();
                }

                return _instance;
            }
        }

        const string lobbyViewPath = "UI/Lobby/Panel - LobbyView";

        Canvas _uiCanvas;
        LobbyView _lobbyView;

        public void Init(Canvas uiCanvas)
        {   
            if(uiCanvas == null)
            {
                Debug.LogError("UICanvas가 없습니다.");
                return;
            }
            _uiCanvas = uiCanvas;

            LobbyView lobbyView = Resources.Load<LobbyView>(lobbyViewPath);
            if(lobbyView == null)
            {
                Debug.LogError($"{lobbyViewPath}에 Panel - LobbyView 없습니다.");
                return;
            }
            _lobbyView = Object.Instantiate(lobbyView, _uiCanvas.transform);

            _lobbyView.Init();

            _lobbyView.showStageView += StageManager.Instance.OnShowStageView;
            _lobbyView.startTimeCounter += () => StageManager.Instance.OnStartTimeCounter();
            _lobbyView.playBgm += () => SoundManager.Instance.OnPlayBgm(0);
        }

        public void OnShowLobbyView()
        {
            _lobbyView.Show();
        }
    }
}
