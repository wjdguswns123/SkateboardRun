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
    /// ���� �ؽ�Ʈ ����.
    /// </summary>
    /// <param name="score"></param>
    public void SetScoreUI(int score)
    {
        _scoreText.text = score.ToString("n0");
    }

    /// <summary>
    /// ���� ��ư ��ġ ó��.
    /// </summary>
    public void OnJumpButtonClick()
    {
        IngameManager.Instance.PlayJump();
    }
}
