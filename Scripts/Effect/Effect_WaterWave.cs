using UnityEngine;

public class Effect_WaterWave : MonoBehaviour
{
    [SerializeField] private Texture[] waveTextures;
    private int currentWaveIndex;
    private Material material;

    private void Start()
    {
        material = GetComponent<MeshRenderer>().material;
        InvokeRepeating(nameof(ChangeTexture), 0, 0.1f);
    }

    private void ChangeTexture()
    {
        material.mainTexture = waveTextures[currentWaveIndex];
        currentWaveIndex = (currentWaveIndex + 1) % waveTextures.Length;
    }
}