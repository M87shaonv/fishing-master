using UnityEngine;

public class Bullet : MonoBehaviour
{
    public int Damage;
    public GameObject WebPrefab;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Border"))
        {
            Destroy(gameObject);
        }

        if (other.CompareTag("Fish"))
        {
            GameObject web = Instantiate(WebPrefab);
            web.transform.SetParent(gameObject.transform.parent, false);
            web.transform.position = gameObject.transform.position;
            web.GetComponent<Web>().Damage = Damage;
            Destroy(gameObject);
        }
    }
}