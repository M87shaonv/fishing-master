using UnityEngine;
using UnityEngine.UI;

public class BgChange : MonoBehaviour
{
    [SerializeField] private Image bg;
    [SerializeField] private Sprite[] bgSprites;
    [SerializeField] private GameObject[] lasnshaWaves;

    private void Start()
    {
        int r = Random.Range(0, bgSprites.Length);
        bg.sprite = bgSprites[r];
        lasnshaWaves[r].SetActive(true);
    }
}