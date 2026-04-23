using System.Collections.Generic;
using UnityEngine;

public class HealthBarPool : MonoBehaviour
{
    public static HealthBarPool Instance { get; private set; }

    [SerializeField] private HealthBar healthBarPrefab;
    [SerializeField] private Transform parentCanvas;
    [SerializeField] private int initCount;

    private Queue<HealthBar> healthBarPool = new();

    private void Awake()
    {
        Instance = this;

        for(int i = 0; i < initCount; i++)
        {
            healthBarPool.Enqueue(CreateHealthBar());
        }
    }

    /// <summary>
    /// 체력바 풀 생성
    /// </summary>
    /// <returns></returns>
    private HealthBar CreateHealthBar()
    {
        var bar = Instantiate(healthBarPrefab, parentCanvas);
        bar.gameObject.SetActive(false);

        return bar;
    }

    /// <summary>
    /// 체력바 가져오기
    /// </summary>
    /// <param name="enemy"></param>
    /// <returns></returns>
    public HealthBar GetHealthBar(Enemy enemy)
    {
        // 남은 체력바 없으면 새로 생성
        if(healthBarPool.Count == 0)
        {
            healthBarPool.Enqueue(CreateHealthBar());
        }

        var bar = healthBarPool.Dequeue();

        bar.Init(enemy);
        bar.gameObject.SetActive(true);
        return bar;
    }

    /// <summary>
    /// 체력바 리턴
    /// </summary>
    /// <param name="bar"></param>
    public void ReturnHealthBar(HealthBar bar)
    {
        bar.gameObject.SetActive(false);
        healthBarPool.Enqueue(bar);
    }
}
