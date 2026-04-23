using UnityEngine;

public abstract class ItemSO : ScriptableObject
{
    [SerializeField] private string id;
    public string ID => id;

    public string itemName; // 이름
    public Sprite Icon; // 아이콘
    public int maxStack = 999; // 최대 스택
        
}
