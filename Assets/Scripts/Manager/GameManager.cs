using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : SingletonMonoBehaviour<GameManager>
{
    private void Awake()
    {
        DontDestroyOnLoad(this);


#if UNITY_EDITOR
        Application.targetFrameRate = -1;
#elif UNITY_ANDROID || UNITY_IOS
        Application.targetFrameRate = 60;
#else
        Application.targetFrameRate = -1;
#endif
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ExitGame();
        }
    }

    /// <summary>
    /// 게임 종료 처리.
    /// </summary>
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
