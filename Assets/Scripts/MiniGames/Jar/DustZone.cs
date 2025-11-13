using UnityEngine;

public class DustZone : MonoBehaviour
{
    [HideInInspector] public int totalParticles;
    [HideInInspector] public int cleanedParticles;

    public bool IsClean => cleanedParticles >= totalParticles;

    public void RegisterParticle() => totalParticles++;
    public void CleanParticle() => cleanedParticles++;
}
