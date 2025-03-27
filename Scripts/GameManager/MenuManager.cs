using System.IO;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private GameObject SlotPanel;
    [SerializeField] private GameObject FilingSlot;
    [SerializeField] private Button BtnLoadGame;
    public int CurrentSaveSlot; // 当前保存槽位
    private static MenuManager instance;

    public static MenuManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<MenuManager>();
                if (instance == null)
                {
                    var singletonObject = new GameObject();
                    instance = singletonObject.AddComponent<MenuManager>();
                    singletonObject.name = nameof(MenuManager) + " (Singleton)";
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void Start()
    {
        BtnLoadGame.onClick.AddListener(LoadGame);
        Button[] buttons = FilingSlot.GetComponentsInChildren<Button>();
        Button[] deleteBtns = SlotPanel.transform.GetChild(1).GetComponentsInChildren<Button>();
        for (int i = 0; i < deleteBtns.Length; i++)
        {
            int index = i; // 复制一下 i 的值到 index
            deleteBtns[i].onClick.AddListener(() => {
                AudioManager.Instance.PlaySound(AudioType.Hit, transform.position);
                DeleteSaveSlot(index);
            });
        }

        for (int i = 0; i < buttons.Length; i++)
        {
            //! 由于 lambda 表达式的特点，所有的监听器在执行时会捕获 i 的引用，而不是它的值。结果就是当所有按钮的点击事件被触发时，i 的值已经是循环结束后的值（即 buttons.Length）
            int index = i; // 复制一下 i 的值到 index
            buttons[i].onClick.AddListener(() => {
                AudioManager.Instance.PlaySound(AudioType.Hit, transform.position);
                CurrentSaveSlot = index;
                SlotPanel.SetActive(false);
                PlayerPrefs.SetInt("CurrentSaveSlot", CurrentSaveSlot);
                // 跳转到游戏场景
                Loader.LoadScene(SceneType.Main);
            });
            PlayerData playerData = LoadPlayerData(i);

            buttons[i].transform.Find("TextLV").GetComponent<Text>().text = playerData == null ? "新存档" : "LV" + playerData.LV;
            buttons[i].transform.Find("TextGold").GetComponent<Text>().text = playerData == null ? "" : "金币:" + playerData.Gold;
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!RectTransformUtility.RectangleContainsScreenPoint(FilingSlot.GetComponent<RectTransform>(), Input.mousePosition,
                    Camera.main))
            {
                SlotPanel.SetActive(false);
            }
        }
    }

    private void LoadGame()
    {
        AudioManager.Instance.PlaySound(AudioType.Hit, transform.position);
        SlotPanel.SetActive(true);
    }

    private PlayerData LoadPlayerData(int index)
    {
        // TODO: 加载玩家数据
        string filePath = Path.Combine(Application.persistentDataPath, $"playerData_{index}.json");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<PlayerData>(json);
        }

        return null;
    }

    private void DeleteSaveSlot(int index)
    {
        string filePath = Path.Combine(Application.persistentDataPath, $"playerData_{index}.json");

        if (File.Exists(filePath))
        {
            File.Delete(filePath);
            Debug.Log($"存档 {index} 已删除");
            // 更新UI
            Button[] buttons = FilingSlot.GetComponentsInChildren<Button>();
            for (int i = 0; i < buttons.Length; i++)
            {
                if (i == index)
                {
                    buttons[i].transform.Find("TextLV").GetComponent<Text>().text = "新存档";
                    buttons[i].transform.Find("TextGold").GetComponent<Text>().text = "";
                }
            }
        }
        else
        {
            Debug.LogWarning($"存档 {index} 不存在");
        }
    }
}