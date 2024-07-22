using UnityEngine;

public class TransformPlace: MonoBehaviour
{
    public TransformType type;
    public GameObject TransformPlaceEffect;
    private void Update()
    {
        if (PlayerHandler.instance.OnDeformField)
        {
            TransformPlaceEffect.gameObject.SetActive(false);
        }
        else
        {
            TransformPlaceEffect.gameObject.SetActive(true);
        }
    }
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

        if (other.CompareTag("Player") && Input.GetKeyDown(KeyCode.F) || other.CompareTag("Player") && PlayerHandler.instance.CurrentPlayer.downAttack)
        {
            if (PlayerHandler.instance.CurrentPlayer.downAttack || Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.X))
                /*PlayerHandler.instance.CurrentPlayer.downAttack = false;
                other.transform.position = this.transform.position;
                PlayerHandler.instance.transformed(type);
                PlayerHandler.instance.CurrentPower = PlayerHandler.instance.MaxPower;*/
                if (!PlayerHandler.instance.OnDeformField)
                {
                    transformStart(other);
                   
                }
        }
    }
}
