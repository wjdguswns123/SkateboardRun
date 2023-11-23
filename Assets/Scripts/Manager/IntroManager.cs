using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(EnterLobby());
    }

    /// <summary>
    /// �κ�� �̵�.
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnterLobby()
    {
        yield return YieldCache.WaitForSeconds(2f);

        SceneManager.LoadSceneAsync("Lobby");
    }
}
