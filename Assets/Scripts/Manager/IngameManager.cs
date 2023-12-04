using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : SingletonMonoBehaviour<IngameManager>
{
    #region Inspector

    [SerializeField]
    private Player _player;

    [SerializeField]
    private Stage _tempCurrentStage;

    #endregion

    private Enums.eGameState _gameState;
    public Enums.eGameState GameState { get { return _gameState; } }

    private void Start()
    {
        //LoadStage();

        StartGame();
    }

    /// <summary>
    /// ���� ����.
    /// </summary>
    public void StartGame()
    {
        _tempCurrentStage.SetStartPoint(_player.transform);
        _player.Init();
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
