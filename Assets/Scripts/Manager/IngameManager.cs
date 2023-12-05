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
            int currentScore = (int)(_clearScore * currentRate) + _currentGetCoinCount * ConstantValues.COIN_SCORE;

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
        _currentGetCoinCount = 0;
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
    public void PlayJump()
    {
        if(_gameState == Enums.eGameState.Playing)
        {
            _player.Jump();
        }
    }
}
