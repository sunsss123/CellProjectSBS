using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTestt : MonoBehaviour
{
    public int breakCount; // ���� ��� �������ں��� �� ưư�� ������Ʈ�� ��� ������ ������ �ν����� ����
    public GameObject enemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            gameObject.SetActive(false);
            Instantiate(enemy, transform.position, Quaternion.identity);
        }
    }
}
