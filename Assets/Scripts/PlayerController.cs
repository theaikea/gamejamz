using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.SceneManagement; // Needed for scene reload

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private int count;
    private float movementX;
    private float movementY;

    public float speed = 5f;
    public float jumpForce = 5f;

    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    private bool isGrounded;

    public Vector3 jumpDirection = new Vector3(0.0f, 1.0f, 0.0f);

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
    }

    void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();
        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    void OnJump()
    {
        if (isGrounded)
        {
            rb.AddForce(jumpDirection * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void OnCollisionStay(Collision collision)
    {
        isGrounded = true;
    }

    void OnCollisionExit(Collision collision)
    {
        isGrounded = false;
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        if (movement.magnitude > 0.01f)
        {
            Vector3 normalizedMovement = movement.normalized;
            Quaternion targetRotation = Quaternion.LookRotation(normalizedMovement);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, 0.2f));
            rb.MovePosition(rb.position + normalizedMovement * speed * Time.fixedDeltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        }
        else if (other.gameObject.CompareTag("SpinningObject"))
        {
            DieAndRespawn();
        }
    }

    void DieAndRespawn()
    {
        // Optionally add a short delay before restarting
        Invoke("RestartGame", 0.5f);
    }

    void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 10)
        {
            winTextObject.SetActive(true);
        }
    }
}
