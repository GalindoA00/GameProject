using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PLAYERCONTROLLER : MonoBehaviour
{

   /* public float speed;//Aldo Code for Player Controller 

    private Rigidbody rb; //References from Unity RigiBody Component, creates the variable to hold the reference 

    // Use this for initialization
    void Start() //Run when the game starts, Where all the game code will go
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() //Physics Code will go
    {
        float moveHorizontal = Input.GetAxis("Horizontal");//Grabs our input of our player through the keyboard. Which the vertical and Horizontal Axis is controlled throught the keyboard 
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical); //Vector3 is (x,y,z), Moving with left and right key and y is not move but to stay still 

        rb.AddForce(movement * speed); //Addforce adds force to the rigid body 

        if (Input.GetKeyDown("space")) //&& rb.transform.position.y <= 0.51f) //Hwanyi Code to Jump
		{
			Vector3 jump = new Vector3(0.0f, 500.0f, 0.0f);
			rb.AddForce(jump);
		}
	}*/
    [SerializeField] private string horizontalInputName;
    [SerializeField] private string verticalInputName;
    [SerializeField] private float movementSpeed;

    private CharacterController charController;

    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier;
    [SerializeField] private KeyCode jumpKey;


    private bool isJumping;

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        float horizInput = Input.GetAxis(horizontalInputName) * movementSpeed;
        float vertInput = Input.GetAxis(verticalInputName) * movementSpeed;

        Vector3 forwardMovement = transform.forward * vertInput;
        Vector3 rightMovement = transform.right * horizInput;

        charController.SimpleMove(forwardMovement + rightMovement);

        JumpInput();

    }

    private void JumpInput()
    {
        if (Input.GetKeyDown(jumpKey) && !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }

    private IEnumerator JumpEvent()
    {
        charController.slopeLimit = 90.0f;
        float timeInAir = 0.0f;

        do
        {
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!charController.isGrounded && charController.collisionFlags != CollisionFlags.Above);

        charController.slopeLimit = 45.0f;
        isJumping = false;
    }

}
