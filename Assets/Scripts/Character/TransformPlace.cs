using UnityEngine;

public class TransformPlace : MonoBehaviour
{
    public TransformType type;
    public virtual void transformStart(Collider other)
    {
        if (PlayerHandler.instance.CurrentType == TransformType.Default)
        {
            other.transform.position = this.transform.position;
            PlayerHandler.instance.LastTransformPlace = this;
            gameObject.SetActive(false);

            other.GetComponent<Player>().FormChange(type);
        }
    }
    private void OnTriggerStay(Collider other)
    {

        /*if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            PlayerHandler.instance.transformed(type);
            PlayerHandler.instance.CurrentPower = PlayerHandler.instance.MaxPower;
        }*/

        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F) || other.CompareTag("Player") && PlayerHandler.instance.CurrentPlayer.downAttack)
        {
            /*PlayerHandler.instance.CurrentPlayer.downAttack = false;
            other.transform.position = this.transform.position;
            PlayerHandler.instance.transformed(type);
            PlayerHandler.instance.CurrentPower = PlayerHandler.instance.MaxPower;*/
            transformStart(other);
        }
    }
}
