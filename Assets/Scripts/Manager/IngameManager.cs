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
    /// 게임 시작.
    /// </summary>
    public void StartGame()
    {
        _tempCurrentStage.SetStartPoint(_player.transform);
        _player.Init();
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
        _player.Stop();

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

    /// <summary>
    /// 캐릭터 점프 실행.
    /// </summary>
    public void PlayJump()
    {
        if(_gameState == Enums.eGameState.Playing)
        {
            _player.Jump();
        }
    }
}
