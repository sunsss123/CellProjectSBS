using System.Collections;
using UnityEngine;

public class SpecialMeleeCollider : MonoBehaviour
{
    public State characterState;

    private void OnEnable()
    {
        StartCoroutine(ActiveSpecialAttack());
    }

    IEnumerator ActiveSpecialAttack()
    {
        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<Enemy>().eStat.characterState = characterState;
        }
    }
}
