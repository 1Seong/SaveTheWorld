using UnityEngine;

public class DustZone : MonoBehaviour
{
    public int totalParticles;
    public int cleanedParticles;
    public JarText jarText;

    private bool _isClean = false;
    public bool IsClean
    {
        get => _isClean;
        set
        {
            if (!_isClean && value) // false -> true
            {
                _isClean = value;
                jarText.Show();

            }
        }
    }

    public void RegisterParticle() => totalParticles++;
    public void CleanParticle()
    {
        cleanedParticles++;
        if (cleanedParticles >= totalParticles)
            IsClean = true;
    }
}
