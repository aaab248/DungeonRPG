using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Vector3 offset;

    private RectTransform rect;
    private Enemy enemy;

    private void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (enemy == null)
        {
            return;
        }

        Vector3 worldPos = enemy.transform.position + offset;
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);

        rect.position = screenPos;

        float ratio = (enemy.currentHealth / enemy.maxHealth);
        slider.value = ratio;
    }

    public void Init(Enemy target) => enemy = target;

    private void OnDisable() => enemy = null;

}
