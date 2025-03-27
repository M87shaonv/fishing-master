using UnityEngine;

public class Fish : MonoBehaviour
{
    [Tooltip("生成最大数量")] public int MaxNum;

    [Tooltip("生成最大速度")] public int MaxSpeed;
    [Tooltip("鱼群每条鱼之间的时间间隔")] public float GenerateWaitTime = 0.5f;
    public int HP;

    public GameObject DiePrefab;

    private int Exp;

    public int Gold;

    private void Start()
    {
        Exp = HP;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Border"))
            Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;
        if (HP <= 0)
        {
            PlayerManager.Instance.DieFish(Gold, Exp, transform.position);
            this.GetComponent<SharkDie>()?.Die();
            GameObject die = Instantiate(DiePrefab);
            die.transform.SetParent(transform.parent, false);
            die.transform.position = transform.position;
            die.transform.rotation = transform.rotation;
            Destroy(gameObject);
        }
    }
}