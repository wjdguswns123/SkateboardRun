using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : SingletonMonoBehaviour<IngameManager>
{
    #region Inspector

    [SerializeField]
    private Player _player;

    #endregion

    /// <summary>
    /// 캐릭터 알리 실행.
    /// </summary>
    public void PlayOllie()
    {
        _player.Ollie();
    }
}
