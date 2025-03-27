using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    [HideInInspector] public GameObject goldHolder;

    public void MoveToGoldHolderCoroutine()
    {
        StartCoroutine(MoveToGoldHolder());
    }

    private IEnumerator MoveToGoldHolder()
    {
        yield return new WaitForSeconds(0.5f);
        while (transform.position != goldHolder.transform.position)
        {
            transform.position = Vector3.MoveTowards(transform.position, goldHolder.transform.position, 30 * Time.deltaTime);
            yield return null; // 等待下一帧
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("GoldHolder"))
            Destroy(this.gameObject);
    }
}