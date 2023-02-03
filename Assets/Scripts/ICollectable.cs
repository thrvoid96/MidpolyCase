using UnityEngine;

public interface ICollectable
{
    public void Collect();
    public Transform GetTransform();

    public void ChangeType(MoneyType type);
    public MoneyType GetType();
}
