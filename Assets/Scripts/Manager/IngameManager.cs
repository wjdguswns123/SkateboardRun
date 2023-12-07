using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : SingletonMonoBehaviour<IngameManager>
{
    #region Inspector

    [SerializeField]
    private PlayerController _player;

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

    private void Start()
    {
        //LoadStage();

        _clearScore = 5000;

        StartGame();
    }

    private void LateUpdate()
    {
        if(_gameState == Enums.eGameState.Playing)
        {
            // ���� ���� ���� ǥ��.
            var currentRate = _tempCurrentStage.GetCurrentRate(_player.transform.position.x);
            int currentScore = (int)(_clearScore * currentRate) + _currentGetCoinCount * ConstantValues.COIN_SCORE + _skillScore;

            _ingameUI.SetScoreUI(currentScore);
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
        _player.Stop();

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
}
