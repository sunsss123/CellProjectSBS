using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public TextMeshProUGUI PlayerFormText;
    public SpriteRenderer Hpbar;
    public TextMeshProUGUI firstSkillIconText;
    public TextMeshProUGUI secondSkillIconText;
   void FormUIUpdate()
    {
        switch (PlayerHandler.instance.CurrentType)
        {
            case TransformType.Default:
                PlayerFormText.text = "���͸�";
                firstSkillIconText.text = "";
                secondSkillIconText.text = "";
                break;
            case TransformType.remoteform:
                PlayerFormText.text = "������";
                firstSkillIconText.text = "���\n����";
                secondSkillIconText.text = "����\n����";
                break;
            case TransformType.mouseform:
                PlayerFormText.text = "���콺";
                firstSkillIconText.text = "";
                secondSkillIconText.text = "";
                break;
        }
    }

    void HPUIUpdate()
    {

        Hpbar.size = new Vector2(PlayerStat.instance.hp, 1.7f);
        switch (PlayerStat.instance.hp)
        {
            case 1:
                Hpbar.color = Color.red;
                break;
            case 2:
                Hpbar.color = Color.yellow;
                break;
            default:
      Hpbar.color = Color.green;
                break;

        }
    }
    // Update is called once per frame
    void Update()
    {
        FormUIUpdate();
        HPUIUpdate();
    }
}
