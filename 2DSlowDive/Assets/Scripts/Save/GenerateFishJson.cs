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

        if (!Directory.Exists(fishPath))
        {
           // Debug.LogError($"[GenerateFishJson] 폴더 없음: {fishPath}");
            return;
        }

        // Fish 폴더 안의 Sprite만 검색
        string[] spriteGUIDs = AssetDatabase.FindAssets("t:Sprite", new[] { fishPath });

        if (spriteGUIDs == null || spriteGUIDs.Length == 0)
        {
           //Debug.LogError($"[GenerateFishJson] Sprite가 없음: {fishPath}");
            return;
        }

        Dictionary<FishType, bool> addedFish = new Dictionary<FishType, bool>();
        FishList fishList = new FishList();

        foreach (string guid in spriteGUIDs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            string fileName = Path.GetFileNameWithoutExtension(path);

            // ----------------------------
            // baseName 정규화 (핵심)
            // ----------------------------
            string baseName = fileName;

            // 1) 뒤 프레임 번호 제거: _0, _1 ...
            baseName = Regex.Replace(baseName, @"_\d+$", "");

            // 2) 뒤 크기 꼬리표 제거: " - 32x16" 같은거
            baseName = Regex.Replace(baseName, @"\s*-\s*\d+x\d+$", "");

            // 3) Trim
            baseName = baseName.Trim();

            // ----------------------------
            // enum 매핑
            // ----------------------------
            if (!System.Enum.TryParse(baseName, true, out FishType fishType))
            {
                // 매핑 실패한건 로그로 출력해서 바로 원인 확인 가능
                //Debug.LogWarning($"[GenerateFishJson] FishType 없음: baseName={baseName} / sprite={fileName}");
                continue;
            }

            // 이미 추가된 fishType이면 스킵
            if (addedFish.ContainsKey(fishType))
                continue;

            // Resources.Load를 위해 "Assets/Resources/" 제거한 상대 경로로 저장
            string relativePath = path
                .Replace("Assets/Resources/", "")
                .Replace(".png", "")
                .Replace(".PNG", "");

            FishInfo info = new FishInfo
            {
                fishType = fishType,
                worldSpritePath = relativePath,
                description = "자동 생성된 물고기입니다."
            };

            fishList.fishList.Add(info);
            addedFish[fishType] = true;
        }

        // ----------------------------
        // 누락 체크 (강제 확인)
        // ----------------------------
        foreach (FishType type in System.Enum.GetValues(typeof(FishType)))
        {
            if (!addedFish.ContainsKey(type))
            {
               // Debug.LogError($"[GenerateFishJson] JSON 누락됨: {type} ({(int)type})");
            }
        }

        // JSON 저장
        string json = JsonUtility.ToJson(fishList, true);
        File.WriteAllText(savePath, json);

        AssetDatabase.Refresh();

       // Debug.Log($"[GenerateFishJson] 물고기 {fishList.fishList.Count}개 JSON 생성 완료 → {savePath}");
    }
}
#endif


