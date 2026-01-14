using UnityEngine;

[CreateAssetMenu(fileName = "SpawnerConfigSO", menuName = "Scriptable Objects/SpawnerConfigSO")]
public class SpawnerConfigSO : ScriptableObject
{
    public float fishInterval = 2f;
    public float fishSpawnX = 5f;
    public float fishSpawnY = -10f;
}
