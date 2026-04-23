using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private TextMeshProUGUI text;

    [SerializeField]
    private Player player;

    private void Update()
    {
        text.text = ((int)player.maxHealth).ToString() + " / " + ((int)player.currentHealth).ToString();
        float ratio = (player.currentHealth / player.maxHealth);
        slider.value = ratio;
    }
}
