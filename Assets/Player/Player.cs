using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour{
    private PlayerInput playerInput;
    [SerializeField] private Transform[] laneTransforms;
    [SerializeField] private float moveSpeed = 20.0f;
    [SerializeField] private float jumpHeight = 1.5f;
    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private LayerMask groundCheckMask;

    [SerializeField] [Range(0, 1)] private float groundCheckRadius = 0.2f;
    private Vector3 destination;
    private Animator animator;

    private int currentLaneIndex;

    private void OnEnable(){
        if (playerInput == null)
            playerInput = new PlayerInput();
        playerInput.Enable();
    }

    private void OnDisable(){
        playerInput?.Disable();
    }

    private void Start(){
        playerInput.gameplay.Move.performed += MovePerformed;
        playerInput.gameplay.Jump.performed += JumpPerformed;
        for (int i = 0; i < laneTransforms.Length; i++){
            if (laneTransforms[i].position == transform.position){
                currentLaneIndex = i;
                destination = laneTransforms[i].position;
            }
        }

        animator = GetComponent<Animator>();
    }

    private void Update(){
        if (!IsOnGround()){
            animator.SetBool("isOnGround", false);
            return;
        }

        animator.SetBool("isOnGround", true);
        var transformX = Mathf.Lerp(transform.position.x, destination.x, Time.deltaTime * moveSpeed);
        transform.position = new Vector3(transformX, transform.position.y, transform.position.z);
    }

    private void MovePerformed(InputAction.CallbackContext obj){
        var inputValue = obj.ReadValue<float>();
        if (inputValue > 0){
            MoveRight();
        }
        else{
            MoveLeft();
        }
    }

    private void JumpPerformed(InputAction.CallbackContext obj){
        if (IsOnGround()){
            var rigidbody = GetComponent<Rigidbody>();
            var jumpUpSpeed = Mathf.Sqrt(2 * jumpHeight * Physics.gravity.magnitude);
            rigidbody?.AddForce(new Vector3(0f, jumpUpSpeed, 0f), ForceMode.VelocityChange);
        }
    }


    private void MoveLeft(){
        if (currentLaneIndex == 0){
            return;
        }

        currentLaneIndex--;
        destination = laneTransforms[currentLaneIndex].position;
    }

    private void MoveRight(){
        if (currentLaneIndex == laneTransforms.Length - 1){
            return;
        }

        currentLaneIndex++;
        destination = laneTransforms[currentLaneIndex].position;
    }

    private bool IsOnGround(){
        return Physics.CheckSphere(groundCheckTransform.position, groundCheckRadius, groundCheckMask);
    }
}