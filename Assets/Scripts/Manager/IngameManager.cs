using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : SingletonMonoBehaviour<IngameManager>
{
    #region Inspector

    [SerializeField]
    private PlayerController _player;

    #endregion

    private Enums.eGameState _gameState;
    public Enums.eGameState GameState { get { return _gameState; } }

    public PlayerController Player { get { return _player; } }

    private StageController _currentStageCtrl;

    private IngameUI _ingameUI;

    private int _clearScore;
    private int _currentGetCoinCount;
    private int _skillScore;
    private int _coinScore;
    private int _currentScore;

    private Stage _currentStageData;

    private BackgroundController _bgCtrl => _currentStageCtrl.BGController;

    private void Start()
    {
        _gameState = Enums.eGameState.Loading;
        _coinScore = ConfigData.Instance.GetData("COIN_SCORE").IntValue;

        StartCoroutine(OnStartGame());
    }

    /// <summary>
    /// 게임 시작 처리.
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnStartGame()
    {
        SystemUIManager.Instance.ShowLoadingUI();

        PlayerModel.Instance.Init();
        StageModel.Instance.Init();

        _currentStageData = StageData.Instance.GetData(StageModel.Instance.CurrentStageIndex);

        yield return null;

        LoadStage();

        yield return EffectManager.Instance.PreloadEffect();    // 이펙트 프리로딩.

        LoadUI();

        _clearScore = _currentStageData.Star1Score;

        yield return YieldCache.WaitForSeconds(1f); // 로딩 UI 볼려고 임시로 1초 딜레이.

        SystemUIManager.Instance.HideLoadingUI();

        StartGame();
    }

    /// <summary>
    /// 스테이지 프리팹 불러오기.
    /// </summary>
    private void LoadStage()
    {
        string stagePath = ConstantValues.PATH_STAGE_PREFAB + _currentStageData.ResourcePath;
        var stageObject = Instantiate(Resources.Load(stagePath)) as GameObject;
        _currentStageCtrl = stageObject.GetComponent<StageController>();
    }

    /// <summary>
    /// UI 불러오기.
    /// </summary>
    private void LoadUI()
    {
        _ingameUI = UIManager.Instance.LoadUI("IngameUI").GetComponent<IngameUI>();
    }

    private void LateUpdate()
    {
        if(_gameState == Enums.eGameState.Playing)
        {
            var currentPlayerPosX = _player.transform.position.x;
            // 현재 진행 점수 표시.
            var currentRate = _currentStageCtrl.GetCurrentRate(currentPlayerPosX);
            _currentScore = (int)(_clearScore * currentRate) + _currentGetCoinCount * _coinScore + _skillScore;

            _ingameUI.SetScoreUI(_currentScore);

            _bgCtrl.UpdateBackground(_currentStageCtrl.GetMoveLength(currentPlayerPosX));

            if(currentRate >= 1f)
            {
                EndGame(true);
            }
        }
    }

    #region Process

    /// <summary>
    /// 게임 시작.
    /// </summary>
    public void StartGame()
    {
        _currentStageCtrl.Init(_player.transform);
        _player.Init();
        _bgCtrl.Init(_player.transform.localPosition);
        _ingameUI.Init();
        _currentGetCoinCount = 0;
        _skillScore = 0;
        _gameState = Enums.eGameState.Playing;
    }

    /// <summary>
    /// 게임 재시작.
    /// </summary>
    public void RestartGame()
    {
        SetGamePause(false);
        StartGame();
    }

    /// <summary>
    /// 게임 일시정지 처리.
    /// </summary>
    public void SetGamePause(bool isPause)
    {
        if (isPause)
        {
            _gameState = Enums.eGameState.Pause;
            Time.timeScale = 0f;
        }
        else
        {
            _gameState = Enums.eGameState.Playing;
            Time.timeScale = 1f;
        }
    }

    /// <summary>
    /// 게임 종료.
    /// </summary>
    public void EndGame(bool isSuccess)
    {
        _gameState = Enums.eGameState.EndGame;

        if(isSuccess)
        {
            _player.Stop();
        }
        else
        {
            _player.Die();
        }

        StartCoroutine(DelayShowResultUI(isSuccess));
    }

    /// <summary>
    /// 일정 시간 이후 게임 결과 UI 출력.
    /// </summary>
    /// <param name="isSuccess"></param>
    /// <returns></returns>
    private IEnumerator DelayShowResultUI(bool isSuccess)
    {
        yield return YieldCache.WaitForSeconds(1f);

        int star = 1;
        if(isSuccess)
        {
            if(_currentScore >= _currentStageData.Star3Score)
            {
                star = 3;
            }
            else if (_currentScore >= _currentStageData.Star2Score)
            {
                star = 2;
            }
        }

        GameResultUI resultUI = UIManager.Instance.LoadUI("GameResultUI").GetComponent<GameResultUI>();
        resultUI.ShowResult(isSuccess, star);
    }

    /// <summary>
    /// 게임 나가기.
    /// </summary>
    public void ExitGame()
    {
        GameManager.Instance.ExitGame();
    }

    #endregion

    /// <summary>
    /// 코인 획득 처리.
    /// </summary>
    public void GetCoin()
    {
        ++_currentGetCoinCount;
    }

    /// <summary>
    /// 캐릭터 점프 실행.
    /// </summary>
    public void PlayJump(int skillIndex)
    {
        if(_gameState == Enums.eGameState.Playing)
        {
            //// 스킬 점수 임시 처리.
            //var skillScore = 500;
            //_skillScore += skillScore;

            _player.Jump(skillIndex);
        }
    }

    /// <summary>
    /// 캐릭터 그라인드 실행.
    /// </summary>
    public void PlayGrind()
    {
        if (_gameState == Enums.eGameState.Playing)
        {
            _player.Grind();
        }
    }

    /// <summary>
    /// 캐릭터 그라인드 종료.
    /// </summary>
    public void EndGrind()
    {
        if (_gameState == Enums.eGameState.Playing)
        {
            _player.EndGrind();
        }
    }

    /// <summary>
    /// 스킬 버튼 활성화.
    /// </summary>
    /// <param name="skillIndexs"></param>
    public void SetReadySkill(List<int> skillIndexs)
    {
        _ingameUI.SetActiveSkillButton(skillIndexs);
    }

    /// <summary>
    /// 그라인드 모드 설정.
    /// </summary>
    /// <param name="enable"></param>
    public void SetGrindMode(bool enable)
    {
        _ingameUI.SetGrindMode(enable);
    }
}
