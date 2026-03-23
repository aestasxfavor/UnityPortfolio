using System.Collections.Generic;
using UnityEngine;

public class CoinValueTable
{
    private static Dictionary<FishType, int> table;

    public static void Load()
    {
        if (table != null) return;
        table = new Dictionary<FishType, int>();

        TextAsset csv = Resources.Load<TextAsset>("FishToCoin");
        if (csv == null)
        {
            Debug.Log("FishToCoin ¸øÃ£À½");
            return;
        }

        string[] lines = csv.text.Split('\n');
        for (int i = 0; i < lines.Length; i++)
        {
            if (string.IsNullOrWhiteSpace(lines[i])) continue;

            string[] cols = lines[i].Split(',');

            if (!System.Enum.TryParse(cols[0], out FishType fishType))
            {
                continue;
            }

            int value = int.Parse(cols[1]);
            table[fishType] = value;
        }
    }
    
    public static int GetCoinValue(FishType fishType)
    {
        Load();

        if(table.TryGetValue(fishType, out int coinValue))
        {
            return coinValue;
        }

        return 10;
    }
}
