using UnityEngine;

public class Effect_DestroySelf : MonoBehaviour
{
    public float destroyTime = 1.0f;

    private void Start()
    {
        Destroy(gameObject, destroyTime);
    }
}