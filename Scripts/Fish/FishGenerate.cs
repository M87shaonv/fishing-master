using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

internal enum MoveType
{
    Straight = 0,
    Turn = 1,
    Cruise = 2
}

public class FishGenerate : MonoBehaviour
{
    public Transform[] GenPositions;
    public GameObject[] FishPrefabs;
    public Transform FishHolder;

    [Tooltip("生成鱼的时间间隔")] public float GenerateInterval = 1f;

    private void Start()
    {
        InvokeRepeating(nameof(GenerateFishes), 0, GenerateInterval);
    }

    public static int[] FishWeights = {
        10, //乌龟
        10, //刺猬鱼
        10, //大眼睛鱼
        10, //孔雀鱼
        10, //小丑鱼
        10, //小蓝鱼
        10, //小青鱼
        10, //小黄鱼
        6, //彩云
        8, //枪鱼
        8, //灯笼鱼
        8, //石斑鱼
        8, //神仙鱼
        8, //蝴蝶鱼
        10, //蟋蟀鱼
        3, //金鲨
        5, //银鲨
        6, //魔鬼鱼
    }; // 鱼的权重

    private int GetWeightedRandomIndex(int[] weights)
    {
        int totalWeight = 0;
        int t = 0;
        foreach (int weight in weights)
        {
            totalWeight += weight;
        }

        int randomValue = Random.Range(0, totalWeight);

        for (int i = 0; i < weights.Length; i++)
        {
            t += weights[i];
            if (randomValue <= t)
                return i;
        }

        return 0; // 默认返回第一个索引
    }

    private void GenerateFishes()
    {
        int genPosIndex = Random.Range(0, GenPositions.Length);
        int fishIndex = GetWeightedRandomIndex(FishWeights);
        int maxNum = FishPrefabs[fishIndex].GetComponent<Fish>().MaxNum;
        int maxSpeed = FishPrefabs[fishIndex].GetComponent<Fish>().MaxSpeed;
        float generateWaitTime = FishPrefabs[fishIndex].GetComponent<Fish>().GenerateWaitTime;
        int currentNum;
        int currentSpeed = Random.Range(maxSpeed / 2, maxSpeed);
        MoveType moveType = (MoveType)Random.Range(0, 3); //0直走,1转弯,2巡游
        int angleOffset; //直走倾斜角度
        int angleSpeed; //转弯角度速度
        switch (moveType)
        {
            case MoveType.Straight:
                currentNum = Random.Range((maxNum / 2), maxNum / 2 + 1);
                angleOffset = Random.Range(-22, 22);
                StartCoroutine(GenerateStraightFish(genPosIndex, fishIndex, currentNum, currentSpeed, angleOffset, generateWaitTime));
                break;
            case MoveType.Turn:
                currentNum = Random.Range((maxNum / 2), maxNum / 2 + 1);
                angleSpeed = Random.Range(0, 2) == 0 ? Random.Range(-15, 9) : Random.Range(9, 15);
                StartCoroutine(GenerateTurnFish(genPosIndex, fishIndex, currentNum, currentSpeed, angleSpeed, generateWaitTime));
                break;
            case MoveType.Cruise:
                currentNum = Random.Range((maxNum / 2) + 1, maxNum);
                GenerateCircularFish(GenPositions[genPosIndex], fishIndex, currentNum, currentSpeed);
                break;
        }
    }

    private IEnumerator GenerateStraightFish(int genPosIndex, int fishIndex, int currentNum, int currentSpeed, int angleOffset,
        float GenerateWaitTime)
    {
        for (var i = 0; i < currentNum; i++)
        {
            GameObject fish = Instantiate(FishPrefabs[fishIndex], FishHolder, false);
            fish.transform.localPosition = GenPositions[genPosIndex].localPosition;
            fish.transform.localRotation = GenPositions[genPosIndex].localRotation;
            fish.transform.Rotate(0, 0, angleOffset);
            fish.GetComponent<SpriteRenderer>().sortingOrder += i;
            fish.AddComponent<Effect_AutoMove>().Speed = currentSpeed;
            yield return new WaitForSeconds(GenerateWaitTime);
        }
    }

    private IEnumerator GenerateTurnFish(int genPosIndex, int fishIndex, int currentNum, int currentSpeed, int angleSpeed,
        float GenerateWaitTime)
    {
        for (var i = 0; i < currentNum; i++)
        {
            GameObject fish = Instantiate(FishPrefabs[fishIndex], FishHolder, false);
            fish.transform.localPosition = GenPositions[genPosIndex].localPosition;
            fish.transform.localRotation = GenPositions[genPosIndex].localRotation;
            fish.GetComponent<SpriteRenderer>().sortingOrder += i;
            fish.AddComponent<Effect_AutoMove>().Speed = currentSpeed;
            fish.AddComponent<Effect_AutoRotate>().Speed = angleSpeed;
            yield return new WaitForSeconds(GenerateWaitTime);
        }
    }

    private void GenerateCircularFish(Transform genPos, int fishIndex, int currentNum, int currentSpeed)
    {
        float baseRadius = Random.Range(10, 30); // 基础半径
        float angle = 2 * Mathf.PI / currentNum;
        float angleOffset = Random.Range(10, 30) * Mathf.Deg2Rad;

        for (var i = 0; i < currentNum; i++)
        {
            GameObject fish = Instantiate(FishPrefabs[fishIndex], genPos, false);
            fish.transform.localPosition = genPos.localPosition; // 重置位置

            // 获取鱼的大小
            float fishSize = fish.transform.localScale.x; // 假设鱼的大小是基于 x 轴的比例

            // 根据鱼的大小调整半径
            float adjustedRadius = baseRadius + fishSize * 1f; //调整系数，可以根据实际情况调整

            // 计算鱼的位置
            fish.transform.localPosition = new Vector3(
                adjustedRadius * Mathf.Cos(i * angle + angleOffset),
                adjustedRadius * Mathf.Sin(i * angle + angleOffset),
                0
            );

            // 设置鱼的角度偏移
            fish.transform.localRotation = Quaternion.Euler(0, 0, Random.Range(-10, 10));

            // 设置排序顺序
            fish.GetComponent<SpriteRenderer>().sortingOrder += i;

            // 添加移动和旋转组件
            fish.AddComponent<Effect_AutoMove>().Speed = currentSpeed;
            fish.AddComponent<Effect_AutoRotate>().Speed = angle;
        }
    }
}