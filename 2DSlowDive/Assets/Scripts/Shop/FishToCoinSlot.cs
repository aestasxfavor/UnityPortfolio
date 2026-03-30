using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishToCoinSlot : MonoBehaviour 
{ 
    [SerializeField] private Image icon; 
    [SerializeField] private TMP_Text countText; 

    private FishType fishType; 
    private bool hasItem; 
    public void Set(Sprite sprite, int count, FishType type) 
    { 
        fishType = type; 
        hasItem = true; 
        icon.sprite = sprite; 
        icon.enabled = true; 
        icon.color = Color.white; 
        countText.text = count.ToString(); 
        countText.enabled = true; 
    } 
    public void SetEmpty(Sprite emptySprite) 
    {
        hasItem = false; 
        fishType = default; 
        icon.sprite = emptySprite; 
        icon.enabled = true; 
        icon.color = Color.gray; 
        countText.enabled = false; 
    } 
}
