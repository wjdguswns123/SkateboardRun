using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
    #region Inspector

    [SerializeField]
    private GameObject _startPoint;
    [SerializeField]
    private GameObject _endPoint;

    #endregion

    /// <summary>
    /// �÷��̾� �������� ��ġ.
    /// </summary>
    /// <param name="playerPos"></param>
    public void SetStartPoint(Transform playerPos)
    {
        playerPos.position = _startPoint.transform.position;
    }
}
