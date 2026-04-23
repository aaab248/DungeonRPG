
using UnityEngine;

public struct ItemStack
{
    public ItemSO item;
    [Min(0)] public int count;

    public ItemStack(ItemSO item,int count)
    {
        this.item = item;
        this.count = count;
        Clamp();
    }

    public bool IsEmpty => (item == null || count <= 0);

    public int RoomLeft => (item == null) ? 0 : Mathf.Max(0,item.maxStack - count);

    /// <summary>
    /// 아이템 추가
    /// </summary>
    /// <param name="add"></param>
    /// <returns></returns>
    public int Add(int add)
    {
        if (item == null) return 0;

        int before = count;
        count = Mathf.Min(item.maxStack, count + add);
        return count - before;
    }

    /// <summary>
    /// 아이템 감소
    /// </summary>
    /// <param name="n"></param>
    public void Decrement(int n)
    {
        count -= n;
        Clamp();
    }

    /// <summary>
    /// 아이템 없음
    /// </summary>
    public void Clear()
    {
        item = null;
        count = 0;
    }

    /// <summary>
    /// 아이템 ?
    /// </summary>
    private void Clamp()
    {
        if(item == null || count <= 0)
        {
            Clear();
            return;
        }
        count = Mathf.Clamp(count, 0, item.maxStack);
    }
}