using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyManager : SingletonMonoBehaviour<LobbyManager>
{
    /// <summary>
    /// �ӽ� ���� ���� ��ư ��ġ ó��.
    /// </summary>
    public void OnStartGameButtonClick()
    {
        SceneManager.LoadSceneAsync("Ingame");
    }
}
