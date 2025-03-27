using UnityEngine;

public class Effect_AutoRotate : MonoBehaviour
{
    public float Speed = 10f;

    private void Update()
    {
        transform.Rotate(Vector3.forward * (Speed * Time.deltaTime));
    }
}