using System.Collections.Generic;
using UnityEngine;

public class FishDataLoader : MonoBehaviour
{
    public static FishDataLoader Instance { get; private set; }

    [System.Serializable]
    public class FishInfo
    {
        public FishType fishType;
        public string worldSpritePath;
        public string iconSpriteName;   // 추가: 대표 아이콘 sprite 이름
        public string description;
    }

    [System.Serializable]
    public class FishList
    {
        public List<FishInfo> fishList = new();
    }

    private Dictionary<FishType, FishInfo> fishInfoDict;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadFishData();
    }

    private void LoadFishData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("fish_data");
        if (jsonFile == null)
        {
            Debug.LogError("[FishDataLoader] fish_data.json을 Resources 폴더에서 찾을 수 없습니다!");
            return;
        }

        FishList loadedList = JsonUtility.FromJson<FishList>(jsonFile.text);

        if (loadedList == null || loadedList.fishList == null)
        {
            Debug.LogError("[FishDataLoader] fish_data.json 파싱 실패 또는 fishList null");
            return;
        }

        fishInfoDict = new Dictionary<FishType, FishInfo>(loadedList.fishList.Count);

        foreach (var info in loadedList.fishList)
        {
            fishInfoDict[info.fishType] = info;
        }

        // 누락 검증
        foreach (FishType type in System.Enum.GetValues(typeof(FishType)))
        {
            if (!fishInfoDict.ContainsKey(type))
                Debug.LogError($"[FishDataLoader] JSON 데이터 누락: {type} ({(int)type})");
        }

        Debug.Log($"[FishDataLoader] 로드 완료 ({fishInfoDict.Count}종)");
    }

    public FishInfo GetFishInfo(FishType fishType)
    {
        if (fishInfoDict == null)
        {
            Debug.LogWarning("[FishDataLoader] 데이터가 로드되지 않음");
            return null;
        }

        if (fishInfoDict.TryGetValue(fishType, out FishInfo info))
            return info;

        return null;
    }
}
