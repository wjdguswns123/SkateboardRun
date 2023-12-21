using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : SingletonMonoBehaviour<CameraManager>
{
    #region Inspector

    [SerializeField]
    private Camera _mainCamera;

    [SerializeField]
    private Transform _cameraMoveParent;

    [SerializeField]
    private CameraShake _mainCameraShake;

    #endregion

    // Update is called once per frame
    private void LateUpdate()
    {
        if(IngameManager.Instance.GameState == Enums.eGameState.Playing)
        {
            var player = IngameManager.Instance.Player;
            
            // ĳ���� �ణ ���ʿ��� ���󰡵���.
            Vector3 pos = _cameraMoveParent.position; 
            pos.x = player.transform.position.x + 5f;
            _cameraMoveParent.position = pos;
        }
    }

    /// <summary>
    /// ī�޶� ����.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="range"></param>
    public void CameraShake(float time = 1f, float range = 1f)
    {
        _mainCameraShake.Play(time, range);
    }
}
