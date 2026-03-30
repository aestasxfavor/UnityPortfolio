using UnityEngine;

public static class LayerUtil
{
    public static bool IsLayer(GameObject obj, Layers layer)
        => obj.layer == (int)layer;
}
