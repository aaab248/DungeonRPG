using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillUI : MonoBehaviour
{
    [SerializeField]
    private Player player;
    [SerializeField]
    private Image cooldown_Image;
    [SerializeField]
    private TextMeshProUGUI cooldownTime;

    private void Update()
    {
        switch(player.DashCondition)
        {
            case SkillCondition.Ready:
                cooldown_Image.fillAmount = 0f;
                cooldownTime.gameObject.SetActive(false);
                break;

            case SkillCondition.NotEnoughPoint:
                cooldown_Image.fillAmount = 1f;
                cooldownTime.gameObject.SetActive(false);
                break;

            case SkillCondition.CoolDown:
                float ratio = Mathf.Clamp01(1f - (Time.time - player.EndDashTime) / player.dashCoolDown);
                cooldown_Image.fillAmount = ratio;

                cooldownTime.gameObject.SetActive(true);
                cooldownTime.text = (player.dashCoolDown - (Time.time - player.EndDashTime)).ToString("F1");
                break;
        }
    }
}
