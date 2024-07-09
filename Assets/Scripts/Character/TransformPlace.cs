using UnityEngine;

public class TransformPlace : MonoBehaviour
{
    public TransformType type;
    public virtual void transformStart(Collider other)
    {
        if (PlayerHandler.instance.CurrentType == TransformType.Default)
        {
            other.transform.position = this.transform.position;
            gameObject.SetActive(false);

            other.GetComponent<Player>().FormChange(type);
        }
    }
    private void OnTriggerStay(Collider other)
    {                
        if (other.CompareTag("Player")/*&& Input.GetKeyDown(KeyCode.F)*/)
        {
            if(PlayerHandler.instance.CurrentPlayer.downAttack || Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.X))
            /*PlayerHandler.instance.CurrentPlayer.downAttack = false;
            other.transform.position = this.transform.position;
            PlayerHandler.instance.transformed(type);
            PlayerHandler.instance.CurrentPower = PlayerHandler.instance.MaxPower;*/
            transformStart(other);
        }
    }
}
