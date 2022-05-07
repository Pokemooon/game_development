using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerControls : MonoBehaviour
{
    public static bool flag;
    [SerializeField]
    private InputActionReference movementControl;
    [SerializeField]
    private InputActionReference jumpControl;
    [SerializeField]
    private float playerSpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 4f;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool isGround;
    private Transform cameraMainTransform;
    private Animator anim;

    private void OnEnable()
    {
        movementControl.action.Enable();
        jumpControl.action.Enable();
    }

    private void OnDisable()
    {
        movementControl.action.Disable();
        jumpControl.action.Disable();
    }

    private void Start()
    {
        //获取CharacterController对象和主相机的Transform信息
        controller = gameObject.GetComponent<CharacterController>();
        cameraMainTransform = Camera.main.transform;
        anim = GetComponent<Animator>();
        Cursor.visible = false;
        flag = false;
    }

    void Update()
    {
        if(flag)////
        {
            isGround = controller.isGrounded; //判断Player是否在地面
            if (isGround && playerVelocity.y < 0)
            {
                playerVelocity.y = 0f;
                anim.SetBool("Jump", false);
            }

            Vector2 movement = movementControl.action.ReadValue<Vector2>(); //读取WASD键盘输入值
                                                                            //Debug.Log("movement.x:"+movement.x);
                                                                            //Debug.Log("movement.y:" + movement.y);
            if (movement.x != 0f || movement.y != 0f)
            {
                //计算移动
                Vector3 moveDirection = new Vector3(movement.x, 0, movement.y);
                moveDirection = cameraMainTransform.forward * moveDirection.z + cameraMainTransform.right * moveDirection.x;
                moveDirection.y = 0f;
                //执行移动操作
                controller.Move(playerSpeed * moveDirection * Time.deltaTime);
                anim.SetBool("Run", true);
            }
            else
            {
                controller.Move(new Vector3(0f, 0f, 0f));
                anim.SetBool("Run", false);
            }


            //处理Player跳跃
            if (jumpControl.action.triggered && isGround)
            {
                playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                anim.SetBool("Jump", true);
            }

            //处理Player下落
            playerVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(playerVelocity * Time.deltaTime);

            //处理Player随镜头旋转
            if (movement != Vector2.zero)
            {
                float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
                Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
            }
        }        
    }

    public void setSpeed(float speed)
    {
        playerSpeed = speed;
    }
}
