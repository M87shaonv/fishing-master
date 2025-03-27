using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GunUI : MonoBehaviour
{
    [Header("枪炮设置")] public GameObject[] Guns;

    public GameObject[] bullet1;
    public GameObject[] bullet2;
    public GameObject[] bullet3;
    public GameObject[] bullet4;
    public GameObject[] bullet5;

    [Header("UI绑定")] public Button BtnAddCost;

    public Button BtnSubCost;
    public Text TextBulletCost; //子弹价格显示

    public Transform BulletHolder; //子弹的父物体
    private readonly float fireRate = 0.1f; // 每n秒发射一次

    //每一炮弹药的价格
    private readonly int[] oneShootCost = {
        100,
        200,
        300,

        400,
        500,
        600,

        700,
        800,
        900,

        1000,
        2000,
        3000,

        6666,
        8888,
        9999,
    };

    private int currentOneShootCost; //当前的弹药价格
    private int currentShootCostIndex; //当前的弹药索引
    private Coroutine firingCoroutine;
    private int currentGunIndex => currentShootCostIndex / 3;
    private bool isFiring; // 添加一个布尔变量来跟踪发射状态

    private void Start()
    {
        isFiring = false;
        BtnAddCost.onClick.AddListener(OnAddCost);
        BtnSubCost.onClick.AddListener(OnSubCost);
        currentOneShootCost = oneShootCost[currentShootCostIndex];
        TextBulletCost.text = "$" + currentOneShootCost;
    }

    private void Update()
    {
        // 处理输入
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            HandleMobileInput();
        }
        else
        {
            HandlePCInput();
        }
    }

    private void HandleMobileInput()
    {
        // 处理手机的输入逻辑，比如触摸输入
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began &&
                !EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                if (!isFiring)
                {
                    isFiring = true;
                    StartFiring();
                }
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                StopFiring();
                isFiring = false;
            }
        }
    }

    private void HandlePCInput()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
            OnAddCost();
        if (Input.GetAxis("Mouse ScrollWheel") < 0)
            OnSubCost();
        // 检查是否按下鼠标左键，启动协程
        if (Input.GetMouseButtonDown(0) &&
            EventSystem.current.IsPointerOverGameObject() is false
           )
        {
            // 只在没有发射时启动发射
            if (!isFiring)
            {
                isFiring = true; // 设置为正在发射
                StartFiring();
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopFiring();
            isFiring = false; // 释放鼠标后重置为未发射状态
        }
    }


    private void StartFiring()
    {
        // 如果协程已经在运行，先停止再重新启动
        if (firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
        }

        firingCoroutine = StartCoroutine(Fire());
    }

    private void StopFiring()
    {
        if (firingCoroutine != null)
        {
            StopCoroutine(firingCoroutine);
            firingCoroutine = null;
        }
    }

    private IEnumerator Fire()
    {
        while (true) // 无限循环，直到协程被停止
        {
            if (PlayerManager.Instance.Gold >= currentOneShootCost)
            {
                int currentBulletIndex = Mathf.Clamp(PlayerManager.Instance.LV / 10, 0, 10); // 根据等级计算当前的子弹索引
                GameObject[] useBullets = currentGunIndex switch {
                    0 => bullet1,
                    1 => bullet2,
                    2 => bullet3,
                    3 => bullet4,
                    4 => bullet5,
                };
                GameObject bullet = Instantiate(useBullets[currentBulletIndex]);
                bullet.transform.SetParent(BulletHolder, false);
                bullet.transform.position = Guns[currentGunIndex].GetComponent<Gun>().FirePoint.position;
                bullet.transform.rotation = Guns[currentGunIndex].GetComponent<Gun>().FirePoint.rotation;
                bullet.AddComponent<Effect_AutoMove>().Direction = Vector3.up;
                bullet.GetComponent<Effect_AutoMove>().Speed = 8;
                bullet.GetComponent<Bullet>().Damage = currentOneShootCost; // 子弹伤害等于弹药价格
                PlayerManager.Instance.SubGold(currentOneShootCost);
                AudioManager.Instance.PlaySound(AudioType.FireGun, transform.position);
                yield return new WaitForSeconds(fireRate + (currentGunIndex * 0.02f)); // 等待指定的时间
            }
            else
            {
                // 如果金钱不足，等待金钱增加再重新检查
                PlayerManager.Instance.goldUI.CheckGold();
                yield return new WaitForSeconds(0.5f); // 每0.5秒检查一次
            }
        }
    }


    private void OnAddCost()
    {
        Guns[currentGunIndex].SetActive(false);
        ++currentShootCostIndex;
        currentShootCostIndex = currentShootCostIndex > oneShootCost.Length - 1 ? 0 : currentShootCostIndex;
        Guns[currentGunIndex].SetActive(true);
        currentOneShootCost = oneShootCost[currentShootCostIndex];
        TextBulletCost.text = "$" + currentOneShootCost;
    }


    private void OnSubCost()
    {
        Guns[currentGunIndex].SetActive(false);
        --currentShootCostIndex;
        currentShootCostIndex = currentShootCostIndex < 0 ? oneShootCost.Length - 1 : currentShootCostIndex;
        Guns[currentGunIndex].SetActive(true);
        currentOneShootCost = oneShootCost[currentShootCostIndex];
        TextBulletCost.text = "$" + currentOneShootCost;
    }
}