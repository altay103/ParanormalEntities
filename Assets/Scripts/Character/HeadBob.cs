using UnityEngine;

public class HeadBob : MonoBehaviour
{
    [Header("Ayarlar")]
    [SerializeField] Transform kameraTransform;  
    [SerializeField] float dönüşHızı = 5f;       
    [SerializeField] float maksimumAçı = -70f;    
    [SerializeField] float minimumAçı = 9f;       
    [SerializeField] float kafaEtkiOranı = 0.5f;  

    private float hedefAçı;

    void Update()
    {
        float kameraAçı = kameraTransform.localEulerAngles.x;
        
        if (kameraAçı > 180f) kameraAçı -= 360f;
        
        hedefAçı = Mathf.Clamp(kameraAçı * kafaEtkiOranı, maksimumAçı, minimumAçı);
        
        Quaternion hedefRotasyon = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y, hedefAçı);
        
        transform.localRotation = Quaternion.Lerp(transform.localRotation, hedefRotasyon, Time.deltaTime * dönüşHızı);
    }
}
