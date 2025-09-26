using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Camera Angle")]
    private float cameraPitch = 0f;
    public float MaxLookAngle = 60f;

    [Header("Player Components")]
    public Camera PlayerEyes;
    public Rigidbody RB;

    [Header("Character Stats")]
    public float MouseSensitivity = 3;
    public float WalkSpeed = 10;

    [Header("Sounds")]
    public AudioSource Footsteps;

    [SerializeField] private bool playerInteracting = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playerInteracting)
        {
            PlayerMovement();
            CameraControl();
        }
        else
        {
            RB.linearVelocity = Vector3.zero; //Interaction bug fix
        }
    }

    public void SetPlayerInteracting(bool isInteracting)
    {
        playerInteracting = isInteracting;
        Debug.Log("Player is interacting: " + playerInteracting);
    }

    void PlayerMovement()
    {
        /*          Player Movement     */
        if (WalkSpeed > 0)
        {
            Vector3 move = Vector3.zero; // Start from zero

            if (Input.GetKey(KeyCode.W))
                move += transform.forward;
            if (Input.GetKey(KeyCode.S))
                move -= transform.forward;
            if (Input.GetKey(KeyCode.A))
                move -= transform.right;
            if (Input.GetKey(KeyCode.D))
                move += transform.right;

            move = move.normalized * WalkSpeed;

            RB.linearVelocity = move;

            // Footstep audio logic
            if (move.magnitude > 0)
            {
                if (!Footsteps.isPlaying)
                    Footsteps.Play();
            }
            else
            {
                if (Footsteps.isPlaying)
                    Footsteps.Stop();
            }
        }
        else
        {
            if (Footsteps.isPlaying)
                Footsteps.Stop();
        }
    }

    void CameraControl()
    {
        //If my mouse goes left/right my body moves left/right
        float xRot = Input.GetAxis("Mouse X") * MouseSensitivity;
        transform.Rotate(0, xRot, 0);

        //If my mouse goes up/down my aim (but not body) go up/down
        float mouseY = -Input.GetAxis("Mouse Y") * MouseSensitivity;
        cameraPitch += mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -MaxLookAngle, MaxLookAngle);

        PlayerEyes.transform.localEulerAngles = new Vector3(cameraPitch, 0f, 0f);
    }
}
