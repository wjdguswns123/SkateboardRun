using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : SingletonMonoBehaviour<IngameManager>
{
    #region Inspector

    [SerializeField]
    private PlayerController _player;

    [SerializeField]
    private BackgroundManager _bgMgr;

    [SerializeField]
    private Stage _tempCurrentStage;

    [SerializeField]
    private IngameUI _ingameUI;

    #endregion

    private Enums.eGameState _gameState;
    public Enums.eGameState GameState { get { return _gameState; } }

    public PlayerController Player { get { return _player; } }

    private int _clearScore;
    private int _currentGetCoinCount;
    private int _skillScore;
    private int _coinScore;

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

        yield return EffectManager.Instance.PreloadEffect();    // 이펙트 프리로딩.

        _clearScore = 5000;

        yield return YieldCache.WaitForSeconds(1f); // 로딩 UI 볼려고 임시로 1초 딜레이.

        SystemUIManager.Instance.HideLoadingUI();

        StartGame();
    }

    private void LateUpdate()
    {
        if(_gameState == Enums.eGameState.Playing)
        {
            var currentPlayerPosX = _player.transform.position.x;
            // 현재 진행 점수 표시.
            var currentRate = _tempCurrentStage.GetCurrentRate(currentPlayerPosX);
            int currentScore = (int)(_clearScore * currentRate) + _currentGetCoinCount * _coinScore + _skillScore;

            _ingameUI.SetScoreUI(currentScore);

            _bgMgr.UpdateBackground(_tempCurrentStage.GetMoveLength(currentPlayerPosX));
        }
    }

    #region Process

    /// <summary>
    /// 게임 시작.
    /// </summary>
    public void StartGame()
    {
        _tempCurrentStage.SetStartPoint(_player.transform);
        _player.Init();
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
        StartGame();
    }

    /// <summary>
    /// 일시 정지.
    /// </summary>
    public void Pause()
    {

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

        GameResultUI resultUI = UIManager.Instance.LoadUI("GameResultUI").GetComponent<GameResultUI>();
        resultUI.ShowResult(isSuccess);
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
            // 스킬 점수 임시 처리.
            var skillScore = 500;
            _skillScore += skillScore;

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


    /// <summary>
    /// 게임 일시정지 처리.
    /// </summary>
    public void SetGamePause()
    {
        switch(_gameState)
        {
            case Enums.eGameState.Playing:
                {
                    _gameState = Enums.eGameState.Pause;
                    Time.timeScale = 0f;
                }
                break;
            case Enums.eGameState.Pause:
                {
                    _gameState = Enums.eGameState.Playing;
                    Time.timeScale = 1f;
                }
                break;
        }
    }
}
