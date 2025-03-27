using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GoldUI : MonoBehaviour
{
    public Sprite[] GoldImgs;
    public Image GoldImg;
    public Text TextGoldNumber;
    public Text TextRewardTimer;
    private Color32 GoldColor;
    private int currentGold = 0; // 当前金币数量
    private const int rewardInterval = 30; // 奖励间隔时间（秒）
    private int timer; // 倒计时变量
    private int rewardGoldNumber => 666 * PlayerManager.Instance.LV / 10; // 奖励金币数量

    private void Start()
    {
        timer = rewardInterval; // 初始化倒计时
        StartCoroutine(RewardGold()); // 开始奖励金币的协程
        GoldColor = TextGoldNumber.color; // 记录初始颜色
    }

    public void UpdateGold(int gold)
    {
        TextGoldNumber.text = "$" + gold.ToString("N0"); // 使用千位分隔符格式化金币显示
        GoldImg.sprite = gold switch {
            < 1000 => GoldImgs[0], //小于1千
            < 10000 => GoldImgs[1], //小于1万
            < 50000 => GoldImgs[2], //小于5万
            < 100000 => GoldImgs[3], //小于10万
            < 200000 => GoldImgs[4], //小于20万
            < 500000 => GoldImgs[5], //小于50万
            < 1000000 => GoldImgs[6], //小于100万
            < 5000000 => GoldImgs[7], //小于500万
            _ => GoldImgs[7] //大于500万
        };
    }

    private IEnumerator RewardGold()
    {
        while (true) // 无限循环
        {
            yield return new WaitForSeconds(1f); // 每秒更新一次倒计时
            timer--; // 倒计时减少

            // 更新倒计时时间
            if (timer > 9)
            {
                int units = timer % 10;
                int tens = (timer / 10) % 10;
                TextRewardTimer.text = $"{tens}  {units}";
            }
            else
            {
                TextRewardTimer.text = $"0  {timer}";
            }

            if (timer <= 0)
            {
                // 到达0时，执行奖励金币逻辑
                PlayerManager.Instance.AddGold(rewardGoldNumber);
                timer = rewardInterval; // 重新开始计时
            }
        }
    }

    public void CheckGold()
    {
        StartCoroutine(GoldNotEnough());
    }

    private IEnumerator GoldNotEnough()
    {
        TextGoldNumber.color = Color.red;
        yield return new WaitForSeconds(0.3f);
        TextGoldNumber.color = GoldColor;
    }
}