using System.Collections.Generic;
using UnityEngine;

public class EnemyDetecter : MonoBehaviour
{

    [SerializeField]
    private LayerMask targetLayer;
    [SerializeField]
    private Enemy enemy;
    [SerializeField]
    private GameObject detectMark;
    //private readonly List<Transform> targetsInRange = new();

    private void Reset()
    {
        enemy = GetComponentInParent<Enemy>();

        var col = GetComponent<CircleCollider2D>();
        col.isTrigger = true;
        col.radius = enemy.detectionRadius;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if((targetLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            Debug.Log("Å½Áö");

            detectMark.SetActive(true);
            enemy.SetIsDetect(collision.transform, true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((targetLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            Debug.Log("»ç¶óÁü");

            detectMark.SetActive(false);
            enemy.SetIsDetect(collision.transform, false);
        }
    }
}
