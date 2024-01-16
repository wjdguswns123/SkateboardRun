using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(LoadData());
    }

    /// <summary>
    /// 데이터 로딩.
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadData()
    {
        DataManager.Instance.Init();

        yield return null;

        DataManager.Instance.LoadData();

        yield return YieldCache.WaitForSeconds(2f);

        SceneManager.LoadSceneAsync("Lobby");
    }
}
