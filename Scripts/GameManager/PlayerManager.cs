using System;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;

    [Tooltip("当前等级")] public int LV;
    [Tooltip("当前经验值")] public int EXP;
    [Tooltip("当前金币数量")] public int Gold;
    public GameObject GoldHolder;
    public GameObject LittleGold;
    public GameObject BigGold;
    public GoldUI goldUI;
    public LVUI lvUI;

    public int RequrireExp => 100 * 100 * LV; // 升级所需的经验值
    public PlayerData playerData;

    public static PlayerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerManager>();
                if (instance == null)
                {
                    var singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<PlayerManager>();
                    singletonObject.name = nameof(PlayerManager) + " (Singleton)";
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        // 确保只有一个实例存在
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 使其在场景切换时不被销毁
        }
        else
        {
            Destroy(gameObject); // 销毁多余的实例
        }
    }

    private void Start()
    {
        playerData = LoadPlayerData();
        LV = playerData.LV;
        EXP = playerData.EXP;
        Gold = playerData.Gold;
        goldUI.UpdateGold(Gold);
        lvUI.UpdateLV(LV, RequrireExp, EXP);
    }

    public void AddGold(int amount)
    {
        Gold += amount;
        goldUI.UpdateGold(Gold);
    }

    public void AddExp(int amount)
    {
        EXP += amount;
        while (EXP >= RequrireExp)
        {
            LV++;
            EXP -= RequrireExp;
        }

        lvUI.UpdateLV(LV, RequrireExp, EXP);
    }

    public void SubGold(int amount)
    {
        Gold -= amount;
        goldUI.UpdateGold(Gold);
    }

    public void SavePlayerData()
    {
        playerData.LV = LV;
        playerData.EXP = EXP;
        playerData.Gold = Gold;
        string json = JsonConvert.SerializeObject(playerData, Formatting.Indented);
        string filePath = Path.Combine(Application.persistentDataPath, $"playerData_{PlayerPrefs.GetInt("CurrentSaveSlot")}.json");
        Debug.Log("SavePlayerData: " + filePath);
        File.WriteAllText(filePath, json);
    }

    public PlayerData LoadPlayerData()
    {
        string filePath = Path.Combine(Application.persistentDataPath, $"playerData_{PlayerPrefs.GetInt("CurrentSaveSlot")}.json");
        Debug.Log("LoadPlayerData: " + filePath);
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<PlayerData>(json);
        }

        // 如果文件不存在，创建一个新的 PlayerData 实例并保存到文件
        PlayerData newPlayerData = new PlayerData(); // 初始化 PlayerData
        string newJson = JsonConvert.SerializeObject(newPlayerData, Formatting.Indented);
        File.WriteAllText(filePath, newJson); // 保存到文件
        return newPlayerData; // 返回新创建的玩家数据
    }

    // 在应用程序退出时调用
    private void OnApplicationQuit()
    {
        SavePlayerData(); // 保存玩家数据
    }

    private float lastPlayTime = 0f; // 上次播放时间
    private const float soundCooldown = 0.2f; // 声音冷却时间

    public void DieFish(int gold, int exp, Vector3 pos)
    {
        AddGold(gold);
        AddExp(exp);
        GameObject go = Instantiate(gold > 5000 ? BigGold : LittleGold, transform.position, Quaternion.identity);
        if (Time.time - lastPlayTime >= soundCooldown)
        {
            AudioManager.Instance.PlaySound(gold > 5000 ? AudioType.BigGold : AudioType.LittleGold, pos);
            lastPlayTime = Time.time; // 更新上次播放时间
        }

        go.GetComponent<Gold>().goldHolder = GoldHolder;
        go.transform.SetParent(GoldHolder.transform, false);
        go.transform.position = pos;
        go.GetComponent<Gold>().MoveToGoldHolderCoroutine();
    }
}

public class PlayerData
{
    public int LV;
    public int EXP;
    public int Gold;
    public int LastShownLevel;

    public PlayerData()
    {
        LV = 1;
        EXP = 100 * 100;
        Gold = 100 * 100;
        LastShownLevel = 1;
    }
}