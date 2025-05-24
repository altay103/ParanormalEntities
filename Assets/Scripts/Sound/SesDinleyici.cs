using UnityEngine;

public class SesDinleyici : MonoBehaviour
{
    public Transform oyuncu;
    public Transform canavar;
    public bool canavarDuydu = false;
    private bool tetiklendi = false;

    void Update()
    {
        if (tetiklendi || oyuncu == null || canavar == null)
            return;

        float mesafe = Vector3.Distance(oyuncu.position, canavar.position);
        float sesSeviyesi = MicInput.MicLoudness;
        float gerekenEsik = HesaplaEsik(mesafe);

        if (sesSeviyesi >= gerekenEsik)
        {
            canavarDuydu = true;
            tetiklendi = true;
            Debug.Log("👂 Canavar seni duydu Mesafe: " + mesafe + " | Ses: " + sesSeviyesi.ToString("F2"));
        }
    }

    float HesaplaEsik(float mesafe)
    {
        if (mesafe <= 5f) return 0.2f;      
        if (mesafe <= 10f) return 0.4f;     
        if (mesafe <= 15f) return 0.6f;     
        return 0.8f;                        
    }
}