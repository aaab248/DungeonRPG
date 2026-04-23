using UnityEngine;

public class AttackPreviewer : MonoBehaviour
{
    public AttackData previewData;

    private void OnDrawGizmos()
    {
        if (previewData == null) return;

        Gizmos.color = Color.red;

        Vector2 center = new Vector2(transform.position.x + previewData.hitBoxOffset.x,
            transform.position.y + previewData.hitBoxOffset.y);

        Vector2 size = previewData.hitBoxSize;

        Gizmos.DrawWireCube(center, size);
    }
}
