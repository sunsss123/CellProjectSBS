using UnityEngine;

public class RemoteSkillCollider : MonoBehaviour
{
    public RemoteTransform remocon;    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            //Debug.Log("Å½Áö");
            remocon.remoteObj.Add(other.gameObject);
        }
    }
}
