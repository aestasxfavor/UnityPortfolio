using UnityEngine;

public struct CodexViewData
{
    public FishType type;
    public Sprite icon;
    public bool discovered;
    public int index;

    public CodexViewData(FishType type, Sprite icon, bool discovered, int index)
    {
        this.type = type;
        this.icon = icon;
        this.discovered = discovered;
        this.index = index;
    }
}
