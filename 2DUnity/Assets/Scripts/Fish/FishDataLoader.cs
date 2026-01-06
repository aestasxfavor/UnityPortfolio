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
        public string description;
    }

    [System.Serializable]
    public class FishList
    {
        public List<FishInfo> fishList = new();
    }

    private FishList fishList;
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

    /// <summary>
    /// fish_data.json을 Resources 폴더에서 불러옴
    /// </summary>
    private void LoadFishData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("fish_data");
        if (jsonFile == null)
        {
            Debug.LogError("[FishDataLoader] fish_data.json을 Resources 폴더에서 찾을 수 없습니다!");
            return;
        }

        FishList loadedList = JsonUtility.FromJson<FishList>(jsonFile.text);

        fishInfoDict = new Dictionary<FishType, FishInfo>(loadedList.fishList.Count);

        foreach (var info in loadedList.fishList)
        {
            fishInfoDict[info.fishType] = info;
        }
        //Debug.Log($"[FishDataLoader] 물고기 데이터 {fishList.fishList.Count}개 로드 완료");
    }

    /// <summary>
    /// 이름으로 물고기 데이터 검색
    /// </summary>
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
