using System.Collections.Generic;
using UnityEngine;

public class AfterImagePool : MonoBehaviour
{
    public static AfterImagePool Instance { get; private set; }

    [SerializeField] private AfterImage prefab;
    [SerializeField] private int initcount = 20;

    private Queue<AfterImage> afterImagePool = new();

    private void Awake()
    {
        Instance = this;

        for(int i = 0; i < initcount; i++)
        {
            afterImagePool.Enqueue(CreateAfterImages());
        }
    }

    /// <summary>
    /// 잔상 이미지 pool 생성
    /// </summary>
    /// <returns></returns>
    private AfterImage CreateAfterImages()
    {
        var obj = Instantiate(prefab, transform);
        obj.gameObject.SetActive(false);

        return obj;
    }


    /// <summary>
    /// 잔상 이미지 가져오기
    /// </summary>
    /// <param name="sprite"></param>
    /// <param name="pos"></param>
    /// <param name="rotate"></param>
    /// <param name="scale"></param>
    /// <param name="color"></param>
    /// <param name="dir"></param>
    /// <returns></returns>
    public AfterImage GetAfterImage(Sprite sprite, Vector3 pos, Vector3 rotate ,Vector3 scale, Color color)
    {
        AfterImage afterImage;

        // 남은 체력바 없으면 새로 생성
        if (afterImagePool.Count <= 0)
        {
            afterImagePool.Enqueue(CreateAfterImages());
        }

        afterImage = afterImagePool.Dequeue();

        afterImage.gameObject.SetActive(true);
        afterImage.Init(sprite, pos, rotate, scale, color);

        return afterImage;
    }

    /// <summary>
    /// 잔상 이미지 return
    /// </summary>
    /// <param name="afterImage"></param>
    public void ReturnAfterImage(AfterImage afterImage)
    {
        afterImage.gameObject.SetActive(false);
        afterImagePool.Enqueue(afterImage);
    }    
}
