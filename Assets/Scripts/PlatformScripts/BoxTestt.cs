using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTestt : MonoBehaviour
{
    public int breakCount; // 예를 들어 나무상자보다 더 튼튼한 오브젝트인 경우 여러번 떄려야 부숴지게 설정
    public GameObject enemy;

    public Animator animator;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            //gameObject.SetActive(false);
            GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(Broken());
            Instantiate(enemy, transform.position, Quaternion.identity).GetComponent<Enemy>().onStun = true;
        }
    }

    IEnumerator Broken()
    {
        animator.SetTrigger("Broken");

        yield return new WaitForSeconds(4f);

        gameObject.SetActive(false);
    }
}
