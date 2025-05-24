using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    [Header("Kamera Ayarları")]
    [SerializeField] float fareHassasiyeti = 100f;
    [SerializeField] Transform oyuncuVücudu;

    float xDonus = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float fareX = Input.GetAxis("Mouse X") * fareHassasiyeti * Time.deltaTime;
        float fareY = Input.GetAxis("Mouse Y") * fareHassasiyeti * Time.deltaTime;

        xDonus -= fareY;
        xDonus = Mathf.Clamp(xDonus, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xDonus, 0f, 0f);
        oyuncuVücudu.Rotate(Vector3.up * fareX);
    }
}