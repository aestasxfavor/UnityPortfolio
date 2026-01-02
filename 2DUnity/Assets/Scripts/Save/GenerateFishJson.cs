#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
[System.Serializable]
public class FishInfo
{
    public string fishName;
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
        string fishPath = "Assets/Resources/Fish"; // Resources 폴더 안으로 한정
        string savePath = "Assets/Resources/fish_data.json";

        // Resources/Fish 폴더 안에 있는 스프라이트만 검색
        string[] spriteGUIDs = AssetDatabase.FindAssets("t:Sprite", new[] { fishPath });
        Dictionary<string, string> addedFish = new Dictionary<string, string>();
        FishList fishList = new FishList();

        foreach (string guid in spriteGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string fileName = Path.GetFileNameWithoutExtension(path);

            // 예: Blue_0 → Blue 로 변환
            string baseName = Regex.Replace(fileName, @"_\d+$", "");

            // 중복 방지 (같은 물고기 여러 프레임 있을 때 첫 번째만)
            if (addedFish.ContainsKey(baseName)) continue;

            // ✅ Resources.Load에서 접근 가능한 상대경로로 변환
            string relativePath = path.Replace("Assets/Resources/", "").Replace(".png", "");

            FishInfo info = new FishInfo
            {
                fishName = baseName,
                worldSpritePath = relativePath,
                description = "자동 생성된 물고기입니다."
            };

            fishList.fishList.Add(info);
            addedFish[baseName] = path;
        }

        string json = JsonUtility.ToJson(fishList, true);
        File.WriteAllText(savePath, json);
        AssetDatabase.Refresh();

        Debug.Log($"{fishList.fishList.Count}마리 물고기 데이터 생성 완료: {savePath}");
    }
}
#endif