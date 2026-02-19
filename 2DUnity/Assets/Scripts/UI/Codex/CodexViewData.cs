using UnityEngine;

public struct CodexViewData
{
    public FishType type;
    public Sprite icon;
    public bool discovered;
    public int index;

    public CodexViewData(FishType _type, Sprite _icon, bool _discovered, int _index)
    {
        type = _type;
        icon = _icon;
        discovered = _discovered;
        index = _index;
    }
}
