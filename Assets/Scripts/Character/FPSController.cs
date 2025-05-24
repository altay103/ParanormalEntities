using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [Header("Hareket Ayarları")]
    [SerializeField] float yürümeHızı = 2f;
    [SerializeField] float koşmaHızı = 3.5f;
    [SerializeField] float zıplamaGücü = 0.7f;
    [SerializeField] float yerçekimi = -9.81f;

    [Header("Zemin Ayarları")]
    [SerializeField] Transform zeminKontrol;
    [SerializeField] float zeminKontrolMesafesi = 0.4f;
    [SerializeField] LayerMask zeminMaskesi;

    private CharacterController controller;
    private Animator animator;

    private Vector3 hız;
    private bool zeminde;
    private bool zıplıyor = false;
    public bool duvaraDegdi = false;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>(); 
    }

    void Update()
    {
        Ray ray = new Ray(transform.position + Vector3.up * 0.5f, transform.forward);
        duvaraDegdi = Physics.Raycast(ray, 0.6f);

        ZeminiKontrolEt();
        HareketEt();
        Zıpla();
        YerçekiminiUygula();

        controller.Move(hız * Time.deltaTime);
        AnimasyonGüncelle();
    }

    void ZeminiKontrolEt()
    {
        zeminde = Physics.CheckSphere(zeminKontrol.position, zeminKontrolMesafesi, zeminMaskesi);
        if (zeminde && hız.y < 0)
        {
            hız.y = -2f;
            if (zıplıyor)
            {
                zıplıyor = false;
                animator.SetBool("Jump", false);
            }
        }
    }

    void HareketEt()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        float aktifHız = Input.GetKey(KeyCode.LeftShift) ? koşmaHızı : yürümeHızı;
        Vector3 yön = (transform.right * x + transform.forward * z).normalized;
        Vector3 yatayHareket = yön * aktifHız;
        hız.x = yatayHareket.x;
        hız.z = yatayHareket.z;
    }

    void Zıpla()
    {
        if (Input.GetButtonDown("Jump") && zeminde)
        {
            hız.y = Mathf.Sqrt(zıplamaGücü * -2f * yerçekimi);
            animator.SetBool("Jump", true);
            zıplıyor = true;
        }
    }

    void YerçekiminiUygula()
    {
        hız.y += yerçekimi * Time.deltaTime;
    }

    void AnimasyonGüncelle()
    {
        Vector3 yatayHareket = new Vector3(controller.velocity.x, 0, controller.velocity.z);
        float hızMiktarı = yatayHareket.magnitude;

        bool yürüyor = hızMiktarı > 0.1f && !Input.GetKey(KeyCode.LeftShift);
        bool koşuyor = hızMiktarı > 0.1f && Input.GetKey(KeyCode.LeftShift);
        bool duruyor = hızMiktarı <= 0.1f;

        animator.SetBool("Walk", yürüyor);
        animator.SetBool("Run", koşuyor);
        animator.SetBool("Idle", duruyor);
    }
}
