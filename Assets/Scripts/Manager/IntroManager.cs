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
    /// 로비로 이동.
    /// </summary>
    /// <returns></returns>
    private IEnumerator EnterLobby()
    {
        yield return YieldCache.WaitForSeconds(2f);

        SceneManager.LoadSceneAsync("Lobby");
    }
}
