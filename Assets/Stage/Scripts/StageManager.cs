using UnityEngine;

using AppleGame.Stage;

namespace AppleGame.Singleton
{
    public class StageManager
    {
        static StageManager _instance;
        public static StageManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new StageManager();
                }

                return _instance;
            }
        }

        const string stageViewPath = "UI/Stage/Panel - StageView";
        const string stageControllerPath = "Stage/Prefab/StageController";

        Canvas _uiCanvas;
        StageView _stageView;
        StageController _stageController;

        public void Init(Canvas uiCanvas)
        {
            if (uiCanvas == null)
            {
                Debug.LogError("UICanvas가 없습니다.");
                return;
            }
            _uiCanvas = uiCanvas;

            StageView stageView = Resources.Load<StageView>(stageViewPath);
            if (stageView == null)
            {
                Debug.LogError($"{stageViewPath}에 Panel - StageView가 없습니다.");
                return;
            }
            _stageView = UnityEngine.Object.Instantiate(stageView, uiCanvas.transform);

            StageController stageController = Resources.Load<StageController>(stageControllerPath);
            if (stageController == null)
            {
                Debug.LogError($"{stageControllerPath}에 StageController가 없습니다.");
                return;
            }
            _stageController = UnityEngine.Object.Instantiate(stageController);

            _stageView.Init();
            _stageController.Init(_stageView);

            _stageController.playCompletionSfx += () => SoundManager.Instance.OnPlaySfx(0);
            _stageController.stopBgm += SoundManager.Instance.StopBgm;

            _stageView.resetGame += () => stageController.StopAllCoroutines();
            _stageView.resetGame += LobbyManager.Instance.OnShowLobbyView;
        }

        public void OnShowStageView()
        {
            _stageView.Show();
        }

        public void OnHideStageView()
        {
            _stageView.Hide();
        }

        public void OnStartTimeCounter()
        {
            _stageController.StartCoroutine(_stageController.TimeCounter());
        }
    }
}
