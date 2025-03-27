using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LVUI : MonoBehaviour
{
    public Text TextLV;
    public Text TextLVName;
    public Slider SliderLV;
    public Text TextUpgrade;

    private LVInfo[] lvInfos = {
        new(lvIndex: 0, lvName: "见习", lvColor: new Color32(211, 211, 211, 255)), // 浅灰色
        new(lvIndex: 1, lvName: "初级", lvColor: new Color32(0, 116, 217, 255)), // 蓝色
        new(lvIndex: 2, lvName: "中级", lvColor: new Color32(46, 204, 64, 255)), // 绿色
        new(lvIndex: 3, lvName: "高级", lvColor: new Color32(255, 220, 0, 255)), // 黄色
        new(lvIndex: 4, lvName: "专家", lvColor: new Color32(255, 133, 27, 255)), // 橙色
        new(lvIndex: 5, lvName: "超级", lvColor: new Color32(177, 13, 201, 255)), // 紫色
        new(lvIndex: 6, lvName: "大师", lvColor: new Color32(255, 215, 0, 255)), // 金色
        new(lvIndex: 7, lvName: "神级", lvColor: new Color32(229, 228, 226, 255)), // 白金
        new(lvIndex: 8, lvName: "至尊", lvColor: new Color32(255, 65, 54, 255)), // 红色
        new(lvIndex: 9, lvName: "传奇", lvColor: new Color32(108, 10, 143, 255)), // 深紫色
        new(lvIndex: 10, lvName: "无敌", lvColor: new Color32(192, 192, 192, 255)), // 银色
        new(lvIndex: 11, lvName: "恶魔", lvColor: new Color32(139, 0, 0, 255)), // 暗红色
        new(lvIndex: 12, lvName: "魔王", lvColor: new Color32(0, 0, 0, 255)), // 黑色
        new(lvIndex: 13, lvName: "末日", lvColor: new Color32(51, 51, 51, 255)), // 深灰色
        new(lvIndex: 14, lvName: "末世", lvColor: new Color32(0, 100, 0, 255)) // 暗绿色
    };

    public void UpdateLV(int lv, int requireExp, int currentExp)
    {
        TextLV.text = lv.ToString();
        // 计算应使用的lv索引，每10级更新一次
        int lvIndex = lv / 10;
        // 限制lvIndex在lvInfos数组的范围内
        if (lvIndex < lvInfos.Length)
        {
            TextLVName.text = lvInfos[lvIndex].lvName;
            TextLV.color = lvInfos[lvIndex].lvColor;
            // 更新TextUpgrade的内容和颜色
            if (lv % 10 == 0)
            {
                int lastShownLevel = PlayerManager.Instance.playerData.LastShownLevel; // 获取最后一次显示称号的等级，未显示时默认为-1
                if (lv != lastShownLevel)
                {
                    TextUpgrade.gameObject.SetActive(true);
                    TextUpgrade.text = $"获得新称号:{lvInfos[lvIndex].lvName}!";
                    TextUpgrade.color = lvInfos[lvIndex].lvColor; // 设置字体颜色
                    AudioManager.Instance.PlaySound(AudioType.Upgrade, Vector3.zero);

                    // 存储最后一次显示称号的等级
                    PlayerManager.Instance.playerData.LastShownLevel = lv;
                }
            }

            SliderLV.value = (float)currentExp / requireExp;
        }
    }

    public struct LVInfo
    {
        public int lvIndex;
        public string lvName;
        public Color32 lvColor;

        public LVInfo(int lvIndex, string lvName, Color32 lvColor)
        {
            this.lvIndex = lvIndex;
            this.lvName = lvName;
            this.lvColor = lvColor;
        }
    }
}