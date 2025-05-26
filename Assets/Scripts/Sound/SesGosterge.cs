using UnityEngine;
using UnityEngine.UI;

public class SesGosterge : MonoBehaviour
{
    public Image[] barlar;
    public Color aktifBeyaz, aktifYesil, aktifSari, aktifTuruncu, aktifKirmizi;
    public AudioListener audioListener;
    [Range(0, 1)] public float sesSeviyesi = 0.0f;

    void Update()
    {
        sesSeviyesi = MicInput.MicLoudness; // Ses seviyesini mikrofon girişinden al
        int aktifBarSayisi = Mathf.RoundToInt(sesSeviyesi * barlar.Length);

        for (int i = 0; i < barlar.Length; i++)
        {
            Color hedefRenk;

            if (i <= 5)
                hedefRenk = aktifBeyaz;
            else if (i <= 11)
                hedefRenk = aktifYesil;
            else if (i <= 14)
                hedefRenk = aktifSari;
            else if (i <= 19)
                hedefRenk = aktifTuruncu;
            else
                hedefRenk = aktifKirmizi;

            if (i < aktifBarSayisi)
            {
                barlar[i].color = hedefRenk;
            }
            else
            {
                Color saydamRenk = hedefRenk;
                saydamRenk.a = 0.1f;
                barlar[i].color = saydamRenk;
            }
        }
    }

}