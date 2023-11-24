using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : SingletonMonoBehaviour<IngameManager>
{
    #region Inspector

    [SerializeField]
    private Player _player;

    [SerializeField]
    private BackgroundManager _backgroundMgr;

    #endregion

    private Enums.eGameState _gameState;
    public Enums.eGameState GameState { get { return _gameState; } }

    private void Start()
    {
        _gameState = Enums.eGameState.Playing;
    }

    /// <summary>
    /// 게임 종료.
    /// </summary>
    public void EndGame()
    {
        _gameState = Enums.eGameState.EndGame;
        _backgroundMgr.StopBackground();
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
