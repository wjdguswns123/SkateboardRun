using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    /// <summary>
    /// �÷��̾�� ���� Ʈ���� ���� ó��.
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
