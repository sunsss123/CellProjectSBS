using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxTestt : MonoBehaviour
{
    public int breakCount; // ���� ��� �������ں��� �� ưư�� ������Ʈ�� ��� ������ ������ �ν����� ����
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
