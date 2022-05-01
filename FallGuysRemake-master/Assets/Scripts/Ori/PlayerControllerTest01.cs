using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// 刚体角色控制器控制角色
/// </summary>
public class PlayerControllerTest01 : MonoBehaviour
{
    Rigidbody rigid;
    public float moveSpeed;
    public float sprintSpeed = 18;
    private Vector3 moveDirection = Vector3.zero;//角色的方向
    float moveScale = 0;
    //addtional
    [SerializeField]
    private InputActionReference movementControl;
    [SerializeField]
    private InputActionReference jumpControl;
    public float jumpSpeed = 8f;
    public bool isGround=true;
    private Transform cameraMainTransform;
    private Animator anim;
    public bool isDoubleJump = false;//二段跳
    private bool isJump = true;
    public bool isSprintSpeed = false;//冲刺
    public bool isFreeze=false;//冻结关卡一段时间
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }
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
        cameraMainTransform = Camera.main.transform;
        SetGravity(this.gameObject,Physics.gravity*2f);
        anim = GetComponent<Animator>();
        Cursor.visible = false;
    }

    private void Update()
    {

        //1.跳跃操作[space]
        //为刚体施加一个向上的力
        //该力是由向上的跳跃速度和在刚体原速度方向上的一个速度合成的
        //VelocityChange是一个瞬时速度
        if (isSprintSpeed)
        {
            Invoke("setSprintSpeed",5);
        }

        if (isDoubleJump)
        {
            Invoke("setDoubleJump",8);
        }

        if (isFreeze)
        {
            Invoke("setFreeze",5);
            Invoke("setLevel3",5);
        }
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (isGround)
            {
                // transform.Translate(transform.up*Time.deltaTime*jumpSpeed);
                rigid.AddForce(transform.up*jumpSpeed,ForceMode.VelocityChange);
                anim.SetBool("Jump",true);
                isJump = true;
                isGround = false;
            }
            else if (isDoubleJump && !isGround && isJump)
            {
                rigid.AddForce(transform.up*jumpSpeed,ForceMode.VelocityChange);
                isJump = false;
            }
        }
        
        //读取WASD
        Vector2 movement = movementControl.action.ReadValue<Vector2>();
        moveScale = 0;
        
        //2.角色移动操作[WASD]
        if (movement.x != 0f || movement.y != 0f)
        {
            //相机跟随旋转
            moveDirection = new Vector3(movement.x, 0, movement.y);
            moveDirection = cameraMainTransform.forward * moveDirection.z + cameraMainTransform.right * moveDirection.x;
            moveDirection.y = 0f;
            //执行移动操作
            //Vector3 v = Vector3.Project(rigid.velocity, transform.forward);
            //float s = moveSpeed == 0 ? 0 : 1 - v.magnitude / moveSpeed;
            //第一种：力驱动，好处：比较真实具体移动交给物理系统；缺点：不好控制，容易滑动
            //rigid.AddForce(transform.forward * moveScale * s, ForceMode.VelocityChange);
            //第二种：速度设置，好处：直接设置速度；缺点：直接干涉物理速度，抖动失真
            //rigid.velocity = transform.forward * 10 * moveScale + Vector3.up * rigid.velocity.y;
            //第三种：直接物理位置，好处：直接移动，缺点：目前描述不出来
            if (moveScale == 1) rigid.MovePosition((rigid.position + transform.forward * moveSpeed * Time.deltaTime));
            anim.SetBool("Run", true);
            moveScale = 1;
        }
        else
        {
            anim.SetBool("Run", false);
            moveScale = 0;
        }
        
        // //3.旋转操作[mouse control]
        Quaternion rot = Quaternion.Euler(0, Vector3.SignedAngle(Vector3.forward, moveDirection, Vector3.up), 0);//目标旋转
        Quaternion currentRot = Quaternion.RotateTowards(rigid.rotation, rot, Time.deltaTime * 600);//中间插值旋转
        rigid.MoveRotation(currentRot);
        
        //3.旋转操作方式二[mouse control]
         // if (movement!=Vector2.zero)
         // {
         //     float targetAngle = Mathf.Atan2(movement.x, movement.y) * Mathf.Rad2Deg + cameraMainTransform.eulerAngles.y;
         //     Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
         //     transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
         // }
         
         //4.掉落水中重新复活
         if (transform.position.y < -90)
         {
             transform.position=new Vector3(1,1,-30);
         }
         //5.冲刺
         
         
    }

    private void FixedUpdate()
    {
        if (moveScale == 1)
        {
            if (Input.GetKey(KeyCode.LeftShift)&&isSprintSpeed)
                rigid.MovePosition(rigid.position + transform.forward * sprintSpeed * Time.fixedDeltaTime);
            else 
                rigid.MovePosition(rigid.position + transform.forward * moveSpeed * Time.fixedDeltaTime);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            isGround = true;
            anim.SetBool("Jump",false);
        }

        if (collision.gameObject.tag=="RingCandy")//如果使用戒指糖果，则大摆锤全部静止，小推手全部静止，5秒
        {
            // Debug.Log("销毁"+collision.gameObject.tag);
            Destroy(collision.gameObject);
            isFreeze = true;
            setLevel3();
        }

        if (collision.gameObject.tag == "ManyHoleFly")//如果使用多转盘羽毛，则直接飞到独木桥底部
        {
            
            // Debug.Log("销毁"+collision.gameObject.tag);
            Destroy(collision.gameObject);
            rigid.AddForce(transform.up*20,ForceMode.VelocityChange);//角色先起飞
            GameObject flag = GameObject.Find("翅膀_目的地");
            transform.LookAt(flag.transform);
            rigid.AddForce(transform.forward*13,ForceMode.VelocityChange);
        }
        
        if (collision.gameObject.tag == "ManyHoleDes")
        {
            // Debug.Log("销毁"+collision.gameObject.tag);
            Destroy(collision.gameObject);
        }
        
        if (collision.gameObject.tag == "ThreeHoleFly")//如果使用三转盘羽毛，则直接飞到独木桥底部
        {
            
            // Debug.Log("销毁"+collision.gameObject.tag);
            Destroy(collision.gameObject);
            rigid.AddForce(transform.up*25,ForceMode.VelocityChange);//角色先起飞
            GameObject flag = GameObject.Find("翅膀_目的地2");
            transform.LookAt(flag.transform);
            rigid.AddForce(transform.forward*10,ForceMode.VelocityChange);
        }
        
        if (collision.gameObject.tag == "ThreeHoleDes")
        {
            // Debug.Log("销毁"+collision.gameObject.tag);
            Destroy(collision.gameObject);
        }
        
        if (collision.gameObject.tag == "Rocket")//如果使用火箭冲刺，则可以用sprintspeed冲刺，5秒
        {
            isSprintSpeed = true;
            // Debug.Log("销毁"+collision.gameObject.tag);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.tag == "SpecialShoes")//如果使用特殊羽毛鞋，则可以进行二段跳，8秒
        {
            isDoubleJump = true;
            // Debug.Log("销毁"+collision.gameObject.tag);
            Destroy(collision.gameObject);
        }
    }
    /// <summary>
    /// 设置物体的重力加速度
    /// </summary>
    public static void SetGravity(GameObject obj, Vector3 val)
    {
        var c = obj.GetComponent<GravityControl>();
        if (c == null)
            c = obj.AddComponent<GravityControl>();
 
        c.Set(val);
    }
    
    /// <summary>
    /// 获取物体的重力加速度
    /// </summary>
    public static Vector3 GetGravity(GameObject obj)
    {
        var c = obj.GetComponent<GravityControl>();
        if (c == null)
            return Physics.gravity;
        else
            return c.Get();
    }

    private void setSprintSpeed()
    {
        isSprintSpeed = false;
    }

    private void setDoubleJump()
    {
        isDoubleJump = false;
    }

    private void setFreeze()
    {
        isFreeze = false;
    }

    private void setLevel3()
    {
        if (isFreeze == true)
        {
            GameObject.Find("/---Scene---/Level1-3/hummer_1/hummer_single_color_1").GetComponent<HammerController>().enabled = false;
            GameObject.Find("/---Scene---/Level1-3/hummer_2/hummer_single_color_2").GetComponent<HammerController>().enabled = false;
            GameObject.Find("/---Scene---/Level1-3/hummer_3/hummer_ball_1").GetComponent<HammerController>().enabled = false;
            GameObject.Find("/---Scene---/Level1-3/hummer_4/hummer_ball_2").GetComponent<HammerController>().enabled = false;
            GameObject.Find("/---Scene---/Level1-3/half-donut-obstacle/MovingStick").GetComponent<HorizontalController>().enabled = false;
            GameObject.Find("/---Scene---/Level1-3/half-donut-obstacle_1/MovingStick").GetComponent<HorizontalController>().enabled = false;
            GameObject.Find("/---Scene---/Level1-3/half-donut-obstacle_2/MovingStick").GetComponent<HorizontalController>().enabled = false;
            GameObject.Find("/---Scene---/Level1-3/half-donut-obstacle_3/MovingStick").GetComponent<HorizontalController>().enabled = false;
            GameObject.Find("/---Scene---/Level1-3/half-donut-obstacle_4/MovingStick").GetComponent<HorizontalController>().enabled = false;
            GameObject.Find("/---Scene---/Level1-3/half-donut-obstacle_5/MovingStick").GetComponent<HorizontalController>().enabled = false;
            GameObject.Find("/---Scene---/Level1-3/half-donut-obstacle_6/MovingStick").GetComponent<HorizontalController>().enabled = false;
        }
        else if (isFreeze == false)
        {
            GameObject.Find("/---Scene---/Level1-3/hummer_1/hummer_single_color_1").GetComponent<HammerController>().enabled = true;
            GameObject.Find("/---Scene---/Level1-3/hummer_2/hummer_single_color_2").GetComponent<HammerController>().enabled = true;
            GameObject.Find("/---Scene---/Level1-3/hummer_3/hummer_ball_1").GetComponent<HammerController>().enabled = true;
            GameObject.Find("/---Scene---/Level1-3/hummer_4/hummer_ball_2").GetComponent<HammerController>().enabled = true;
            GameObject.Find("/---Scene---/Level1-3/half-donut-obstacle/MovingStick").GetComponent<HorizontalController>().enabled = true;
            GameObject.Find("/---Scene---/Level1-3/half-donut-obstacle_1/MovingStick").GetComponent<HorizontalController>().enabled = true;
            GameObject.Find("/---Scene---/Level1-3/half-donut-obstacle_2/MovingStick").GetComponent<HorizontalController>().enabled = true;
            GameObject.Find("/---Scene---/Level1-3/half-donut-obstacle_3/MovingStick").GetComponent<HorizontalController>().enabled = true;
            GameObject.Find("/---Scene---/Level1-3/half-donut-obstacle_4/MovingStick").GetComponent<HorizontalController>().enabled = true;
            GameObject.Find("/---Scene---/Level1-3/half-donut-obstacle_5/MovingStick").GetComponent<HorizontalController>().enabled = true;
            GameObject.Find("/---Scene---/Level1-3/half-donut-obstacle_6/MovingStick").GetComponent<HorizontalController>().enabled = true;
        }
        
    }
}