using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkDie : MonoBehaviour
{
    public void Die()
    {
        AudioType random = (AudioType)Random.Range(2, 19);
        AudioManager.Instance.PlayVoice(random, transform.position);
    }
}