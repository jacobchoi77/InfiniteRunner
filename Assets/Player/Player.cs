using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour{
    private PlayerInput playerInput;
    [SerializeField] private Transform[] laneTransforms;
    [SerializeField] private float moveSpeed = 20.0f;
    [SerializeField] private float jumpHeight = 1.5f;

    [SerializeField] private Transform groundCheckTransform;
    [SerializeField] private LayerMask groundCheckMask;
    [SerializeField] private Vector3 blockageCheckHalfExtend;
    [SerializeField] private string blockageCheckTag = "Threat";

    [SerializeField] [Range(0, 1)] private float groundCheckRadius = 0.2f;
    [Header("Audio")] [SerializeField] private AudioClip jumpAudioClip;
    [SerializeField] private AudioClip moveAudioClip;
    [SerializeField] private AudioSource actionAudioSrc;
    private Vector3 destination;
    private Animator animator;
    private Camera playerCamera;
    private Vector3 playerCameraOffset;

    private int currentLaneIndex;

    [SerializeField] private InGameUI playerUI;
    private static readonly int OnGround = Animator.StringToHash("isOnGround");

    private void OnEnable(){
        playerInput ??= new PlayerInput();
        playerInput.Enable();
    }

    private void OnDisable(){
        playerInput?.Disable();
    }

    private void Start(){
        playerInput.gameplay.Move.performed += MovePerformed;
        playerInput.gameplay.Jump.performed += JumpPerformed;
        playerInput.menu.Pause.performed += TogglePause;
        for (int i = 0; i < laneTransforms.Length; i++){
            if (laneTransforms[i].position == transform.position){
                currentLaneIndex = i;
                destination = laneTransforms[i].position;
            }
        }

        animator = GetComponent<Animator>();
        playerCamera = Camera.main;
        if (playerCamera != null) playerCameraOffset = playerCamera.transform.position - transform.position;
    }

    private void TogglePause(InputAction.CallbackContext obj){
        var gameMode = GameplayStatics.GetGameMode();
        if (playerInput.gameplay.enabled){
            playerInput.gameplay.Disable();
        }
        else{
            playerInput.gameplay.Enable();
        }

        if (gameMode != null && !gameMode.IsGameOver()){
            gameMode.TogglePause();
            playerUI.SignalPause(gameMode.IsGamePaused());
        }
    }

    private void Update(){
        animator.SetBool(OnGround, IsOnGround());
        var position = transform.position;
        var transformX = Mathf.Lerp(position.x, destination.x, Time.deltaTime * moveSpeed);
        position = new Vector3(transformX, position.y, position.z);
        transform.position = position;
    }

    private void LateUpdate(){
        playerCamera.transform.position = transform.position + playerCameraOffset;
    }

    private void MovePerformed(InputAction.CallbackContext obj){
        if (!IsOnGround()) return;
        var inputValue = obj.ReadValue<float>();
        var goalIndex = currentLaneIndex;
        if (inputValue > 0){
            if (goalIndex == laneTransforms.Length - 1) return;
            goalIndex++;
        }
        else{
            if (goalIndex == 0) return;
            goalIndex--;
        }

        var goalPos = laneTransforms[goalIndex].position;
        if (GameplayStatics.IsPositionOccupied(goalPos, blockageCheckHalfExtend, blockageCheckTag)){
            return;
        }

        actionAudioSrc.clip = moveAudioClip;
        actionAudioSrc.Play();
        currentLaneIndex = goalIndex;
        destination = goalPos;
    }

    private void JumpPerformed(InputAction.CallbackContext obj){
        if (IsOnGround()){
            var rb = GetComponent<Rigidbody>();
            if (rb == null) return;
            var jumpUpSpeed = Mathf.Sqrt(2 * jumpHeight * Physics.gravity.magnitude);
            rb.AddForce(new Vector3(0f, jumpUpSpeed, 0f), ForceMode.VelocityChange);
            actionAudioSrc.clip = jumpAudioClip;
            actionAudioSrc.Play();
        }
    }

    private bool IsOnGround(){
        return Physics.CheckSphere(groundCheckTransform.position, groundCheckRadius, groundCheckMask);
    }
}