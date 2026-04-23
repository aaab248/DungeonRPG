using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillPointUI : MonoBehaviour
{
    public SkillPoint skillPoint;
    public Image[] spImage;

    private void Update()
    {
        int filled = skillPoint.Filled;
        float charging = skillPoint.Charging;

        for(int i = 0; i < spImage.Length; i++)
        {
            float value = (i < filled) ? 1f : (i == filled ? charging : 0f);
            spImage[i].fillAmount = value;

            if (value >= 1f)
            {
                spImage[i].color = Color.red;
            }
            else if (value > 0f)
            {
                spImage[i].color = Color.yellow;
            }
            else
            {
                spImage[i].color = Color.gray;
            }
        }
    }
   
}
