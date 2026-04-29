using UnityEngine;
using UnityEngine.UI;

public class SkillPointSlot : MonoBehaviour
{
    public Image filledImage;

    public void SetVisual(float value)
    {
        filledImage.fillAmount = value;
        filledImage.gameObject.SetActive(value >= 1f ? false : true);
    }
}
