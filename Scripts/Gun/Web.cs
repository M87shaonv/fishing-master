using UnityEngine;

public class Web : MonoBehaviour
{
    public float DisappearTime;
    public int Damage;

    private void Start()
    {
        Destroy(gameObject, DisappearTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Fish"))
        {
            other.SendMessage("TakeDamage", Damage);
        }
    }
}