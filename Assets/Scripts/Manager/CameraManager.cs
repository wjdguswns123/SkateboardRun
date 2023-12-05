using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    #region Inspector

    [SerializeField]
    private Camera _mainCamera;

    #endregion

    // Update is called once per frame
    private void LateUpdate()
    {
        if(IngameManager.Instance.GameState == Enums.eGameState.Playing)
        {
            var player = IngameManager.Instance.Player;
            
            // ĳ���� �ణ ���ʿ��� ���󰡵���.
            Vector3 pos = _mainCamera.transform.position; 
            pos.x = player.transform.position.x + 5f;
            _mainCamera.transform.position = pos;
        }
    }
}
