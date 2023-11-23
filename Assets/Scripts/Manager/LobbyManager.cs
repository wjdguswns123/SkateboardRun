using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : SingletonMonoBehaviour<LobbyManager>
{
    /// <summary>
    /// 임시 게임 시작 버튼 터치 처리.
    /// </summary>
    public void OnStartGameButtonClick()
    {
        SceneManager.LoadSceneAsync("Ingame");
    }
}
