using UnityEngine;

public class TransformPlace : MonoBehaviour
{
    public TransformType type;

    private void OnTriggerStay(Collider other)
    {
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.X))
        {
            PlayerHandler.instance.CurrentPlayer.downAttack = true;
        }

        /*if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            PlayerHandler.instance.transformed(type);
            PlayerHandler.instance.CurrentPower = PlayerHandler.instance.MaxPower;
        }*/

        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F) || other.CompareTag("Player") && PlayerHandler.instance.CurrentPlayer.downAttack)
        {
            PlayerHandler.instance.CurrentPlayer.downAttack = false;
            other.transform.position = this.transform.position;
            PlayerHandler.instance.transformed(type);
            PlayerHandler.instance.CurrentPower = PlayerHandler.instance.MaxPower;

            gameObject.SetActive(false);

            //other.GetComponent<Player>().FormChange(type);
        }
    }
}
