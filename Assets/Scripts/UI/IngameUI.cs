using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IngameUI : MonoBehaviour
{
    #region Inspector

    [SerializeField]
    private TMP_Text _scoreText;

    #endregion

    /// <summary>
    /// 점수 텍스트 설정.
    /// </summary>
    /// <param name="score"></param>
    public void SetScoreUI(int score)
    {
        _scoreText.text = score.ToString("n0");
    }

    /// <summary>
    /// 점프 버튼 터치 처리.
    /// </summary>
    public void OnJumpButtonClick()
    {
        IngameManager.Instance.PlayJump();
    }
}
