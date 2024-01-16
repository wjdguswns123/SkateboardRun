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
    /// ���� ���� ó��.
    /// </summary>
    /// <returns></returns>
    private IEnumerator OnStartGame()
    {
        SystemUIManager.Instance.ShowLoadingUI();

        yield return EffectManager.Instance.PreloadEffect();    // ����Ʈ �����ε�.

        _clearScore = 5000;

        yield return YieldCache.WaitForSeconds(1f); // �ε� UI ������ �ӽ÷� 1�� ������.

        SystemUIManager.Instance.HideLoadingUI();

        StartGame();
    }

    private void LateUpdate()
    {
        if(_gameState == Enums.eGameState.Playing)
        {
            var currentPlayerPosX = _player.transform.position.x;
            // ���� ���� ���� ǥ��.
            var currentRate = _tempCurrentStage.GetCurrentRate(currentPlayerPosX);
            int currentScore = (int)(_clearScore * currentRate) + _currentGetCoinCount * _coinScore + _skillScore;

            _ingameUI.SetScoreUI(currentScore);

            _bgMgr.UpdateBackground(_tempCurrentStage.GetMoveLength(currentPlayerPosX));
        }
    }

    #region Process

    /// <summary>
    /// ���� ����.
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
    /// ���� �����.
    /// </summary>
    public void RestartGame()
    {
        StartGame();
    }

    /// <summary>
    /// �Ͻ� ����.
    /// </summary>
    public void Pause()
    {

    }

    /// <summary>
    /// ���� ����.
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
    /// ���� �ð� ���� ���� ��� UI ���.
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
    /// ���� ������.
    /// </summary>
    public void ExitGame()
    {
        GameManager.Instance.ExitGame();
    }

    #endregion

    /// <summary>
    /// ���� ȹ�� ó��.
    /// </summary>
    public void GetCoin()
    {
        ++_currentGetCoinCount;
    }

    /// <summary>
    /// ĳ���� ���� ����.
    /// </summary>
    public void PlayJump(int skillIndex)
    {
        if(_gameState == Enums.eGameState.Playing)
        {
            // ��ų ���� �ӽ� ó��.
            var skillScore = 500;
            _skillScore += skillScore;

            _player.Jump(skillIndex);
        }
    }

    /// <summary>
    /// ĳ���� �׶��ε� ����.
    /// </summary>
    public void PlayGrind()
    {
        if (_gameState == Enums.eGameState.Playing)
        {
            _player.Grind();
        }
    }

    /// <summary>
    /// ĳ���� �׶��ε� ����.
    /// </summary>
    public void EndGrind()
    {
        if (_gameState == Enums.eGameState.Playing)
        {
            _player.EndGrind();
        }
    }

    /// <summary>
    /// ��ų ��ư Ȱ��ȭ.
    /// </summary>
    /// <param name="skillIndexs"></param>
    public void SetReadySkill(List<int> skillIndexs)
    {
        _ingameUI.SetActiveSkillButton(skillIndexs);
    }

    /// <summary>
    /// �׶��ε� ��� ����.
    /// </summary>
    /// <param name="enable"></param>
    public void SetGrindMode(bool enable)
    {
        _ingameUI.SetGrindMode(enable);
    }


    /// <summary>
    /// ���� �Ͻ����� ó��.
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
