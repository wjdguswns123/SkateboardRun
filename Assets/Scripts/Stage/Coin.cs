using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    /// <summary>
    /// 플레이어와 코인 트리거 접촉 처리.
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(ConstantValues.TAG_PLAYER))
        {
            this.gameObject.SetActive(false);
        }
    }
}
