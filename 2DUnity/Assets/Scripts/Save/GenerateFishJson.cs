#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

[System.Serializable]
public class FishInfo
{
    public FishType fishType;
    public string worldSpritePath;
    public string description;
}

[System.Serializable]
public class FishList
{
    public List<FishInfo> fishList = new List<FishInfo>();
}

public class GenerateSimpleFishJson
{
    [MenuItem("Tools/Generate Simple Fish JSON")]
    public static void GenerateJson()
    {
        string fishPath = "Assets/Resources/Fish";
        string savePath = "Assets/Resources/fish_data.json";

        string[] spriteGUIDs = AssetDatabase.FindAssets("t:Sprite", new[] { fishPath });

        Dictionary<FishType, bool> addedFish = new Dictionary<FishType, bool>();
        FishList fishList = new FishList();

        foreach (string guid in spriteGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string fileName = Path.GetFileNameWithoutExtension(path);

            // Blue_0 → Blue
            string baseName = Regex.Replace(fileName, @"_\d+$", "");

            if (!System.Enum.TryParse(baseName, out FishType fishType))
            {
                Debug.LogWarning($"[GenerateFishJson] FishType 없음: {baseName}");
                continue;
            }

            if (addedFish.ContainsKey(fishType))
                continue;

            string relativePath = path
                .Replace("Assets/Resources/", "")
                .Replace(".png", "");

            FishInfo info = new FishInfo
            {
                fishType = fishType,
                worldSpritePath = relativePath,
                description = "자동 생성된 물고기입니다."
            };

            fishList.fishList.Add(info);
            addedFish[fishType] = true;
        }

        string json = JsonUtility.ToJson(fishList, true);
        File.WriteAllText(savePath, json);

        AssetDatabase.Refresh();

        Debug.Log($"[GenerateFishJson] 물고기 {fishList.fishList.Count}개 JSON 생성 완료");
    }
}
#endif
