using UnityEngine;

[CreateAssetMenu(fileName = "FishCodexSO", menuName = "Scriptable Objects/FishCodexSO")]
public class FishCodexSO : ScriptableObject
{
    [Header("기본 정보")]
    public string fishName;

    [Header("설명")]
    [TextArea(2, 5)]
    public string description;
}