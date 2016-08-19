using UnityEngine;
using System.Collections;

[System.Serializable]
public class HueBridgeV2
{
    public string id;
    public string internalipaddress;

    public static HueBridgeV2 CreateFromJSON(string jsonString)
    {
        return JsonUtility.FromJson<HueBridgeV2>(jsonString);
    }
}
