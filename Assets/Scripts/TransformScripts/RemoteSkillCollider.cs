using UnityEngine;

public class RemoteSkillCollider : MonoBehaviour
{
    public RemoteTransform remocon;

    private void Update()
    {
        transform.localPosition = new Vector3((int)PlayerHandler.instance.CurrentPlayer.direction * Mathf.Abs(transform.localPosition.x), transform.localPosition.y, transform.localPosition.z);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("GameController"))
        {
            //Debug.Log("Å½Áö");
            remocon.remoteObj.Add(other.gameObject);
        }
    }
}
