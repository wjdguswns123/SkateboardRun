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
    /// ĳ���� �˸� ����.
    /// </summary>
    public void PlayOllie()
    {
        _player.Ollie();
    }
}
