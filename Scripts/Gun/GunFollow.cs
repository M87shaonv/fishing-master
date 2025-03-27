using UnityEngine;

public class GunFollow : MonoBehaviour
{
    public Camera camera;
    public RectTransform CanvasRect;
    private Vector3 MousePos;
    private float z;

    private void Update()
    {
        RectTransformUtility.ScreenPointToWorldPointInRectangle(CanvasRect, new Vector2(Input.mousePosition.x, Input.mousePosition.y),
            camera, out MousePos);

        if (MousePos.x > transform.position.x)
        {
            z = -Vector3.Angle(Vector3.up, MousePos - transform.position);
        }
        else
        {
            z = Vector3.Angle(Vector3.up, MousePos - transform.position);
        }

        transform.localRotation = Quaternion.Euler(0, 0, z);
    }
}