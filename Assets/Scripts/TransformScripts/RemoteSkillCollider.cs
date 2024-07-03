using UnityEngine;

public class RemoteSkillCollider : MonoBehaviour
{
    public RemoteForm remocon;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            Debug.Log("Ž��");
            remocon.remoteObj.Add(other.gameObject);
        }
    }
}
