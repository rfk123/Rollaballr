using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class PlayerController : MonoBehaviour
{

    public float speed = 0;
    public TextMeshProUGUI countText;
    public GameObject winTextObject;

    //for jumping feature
    public float jumpForce = 5;
    private int jumpCount = 0;
    private int maxJump = 1;
    private bool isGrounded;

    private int count;
    private Rigidbody rb;
    private float movementX;
    private float movementY;

    // Start is called before the first frame update
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

    void SetCountText()
    {
        countText.text = "Count: " + count.ToString();

        if(count >= 2)
        {
            winTextObject.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement * speed);

        //Check if the player object is grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 0.5f);
        if (isGrounded)
            //if grounded then reset jump count
            jumpCount = 0;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //if the ball is grounded or has only jumped once then jump when
            //space is pressed and add to count
            if (isGrounded || jumpCount < maxJump)
            {
                rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
                jumpCount++;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            count += 1;

            SetCountText();
        }
    }
}
