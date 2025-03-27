using UnityEngine;

public class Effect_AutoMove : MonoBehaviour
{
    public float Speed = 1.0f;
    public Vector3 Direction = Vector3.right;

    private void Update()
    {
        transform.Translate(Direction * (Speed * Time.deltaTime));
    }
}