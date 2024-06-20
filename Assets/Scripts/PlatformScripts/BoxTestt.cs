using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTestt : MonoBehaviour
{
    public int breakCount; // 예를 들어 나무상자보다 더 튼튼한 오브젝트인 경우 여러번 떄려야 부숴지게 설정
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
