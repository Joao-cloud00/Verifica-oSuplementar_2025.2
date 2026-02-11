using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movimento (WASD)")]
    public float moveSpeed = 15f;

    [Header("Altitude (Q/E)")]
    public float verticalSpeed = 10f;
    public float minHeight = 2f;  // Altura mínima (para não entrar no chão)
    public float maxHeight = 20f; // Altura máxima

    [Header("Suavização da Rotação")]
    [Tooltip("Quanto menor, mais suave e lento. Quanto maior, mais rápido.")]
    public float rotationSmoothness = 5f;

    [Header("Referência do Chão")]
    public float groundLevel = 0f;

    void Update()
    {
        HandleMovement();
        HandleRotation();
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 moveDir = (Vector3.right * -x + Vector3.forward * -z).normalized;
        transform.position += moveDir * moveSpeed * Time.deltaTime;

        float yMove = 0f;
        if (Input.GetKey(KeyCode.E)) yMove = 1f; // Sobe
        if (Input.GetKey(KeyCode.Q)) yMove = -1f; // Desce

        Vector3 newPosition = transform.position + (Vector3.up * yMove * verticalSpeed * Time.deltaTime);

        newPosition.y = Mathf.Clamp(newPosition.y, minHeight, maxHeight);

        transform.position = newPosition;
    }

    void HandleRotation()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, new Vector3(0, groundLevel, 0));
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 pointToLook = ray.GetPoint(rayDistance);
            Vector3 direction = pointToLook - transform.position;
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothness * Time.deltaTime);
        }
    }
}