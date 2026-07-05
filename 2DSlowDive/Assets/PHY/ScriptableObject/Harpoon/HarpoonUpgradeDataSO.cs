using UnityEngine;
[CreateAssetMenu(fileName = "HarpoonUpgradeDataSO", menuName = "Scriptable Objects/HarpoonUpgradeDataSO")]
public class HarpoonUpgradeDataSO : ScriptableObject
{
    public int cost;        // 업글 작살 가격
    public int shootCount;  // 발사되는 작살 개수 3개
}
