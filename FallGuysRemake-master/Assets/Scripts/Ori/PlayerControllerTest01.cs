using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using Cursor = UnityEngine.Cursor;

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
    public GameObject CM_FreeLook1;
    public GameObject ButtonPauseMenu;
    public GameObject ButtonSaveMenu;
    public AudioSource Vectory;
    public AudioSource Vectory2;
    public bool vector_flag = true;
    public AudioSource Candy_music;
    public AudioSource Fly_music;
    public AudioSource Jump_music;
    public AudioSource Sprint_music;
    public AudioSource Revive_music;
    public AudioSource Obstacle_music;
    private int flag_pos=0;
    private int x_pos=0;
    private int y_pos=0;
    private int z_pos=0;
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
        if (GameObject.Find("flag_position")!=null)
        {
            flag_pos = GameObject.Find("flag_position").GetComponent<GameData>().param;
            x_pos = GameObject.Find("x_position").GetComponent<GameData>().param;
            y_pos = GameObject.Find("y_position").GetComponent<GameData>().param;
            z_pos = GameObject.Find("z_position").GetComponent<GameData>().param;
            GameObject.Find("FallGuysPlayer1").transform.position = new Vector3(x_pos, y_pos, z_pos);
            Destroy(GameObject.Find("flag_position"));
        }
        cameraMainTransform = Camera.main.transform;
        SetGravity(this.gameObject,Physics.gravity*2f);
        anim = GetComponent<Animator>();
        Cursor.visible = false;
        // Cursor.lockState = 0;
    }

    private void Update()
    {
        //0.鼠标识别，只有当鼠标不出现时，才可以进行角色控制
        //0.1 当菜单没出现时，并且鼠标按下时，原来鼠标看不见->变成鼠标可见，视角不可动
        if (!ButtonSaveMenu.activeSelf&&!ButtonPauseMenu.activeSelf && Input.GetMouseButtonDown(0) &&
            Cursor.visible == false)
        {
            Cursor.visible = true;
            CM_FreeLook1.SetActive(false);
        }
        //0.2 当菜单出现了，鼠标一直可见，进行操作
        else if (ButtonPauseMenu.activeSelf||ButtonSaveMenu.activeSelf)
        {
            Cursor.visible = true;
        }
        //0.3 当菜单关闭后，并且鼠标按下了时，原来鼠标看得见->变成鼠标不可见，视角恢复可动
        else if (!ButtonSaveMenu.activeSelf&&!ButtonPauseMenu.activeSelf && Input.GetMouseButtonDown(0) &&
                 Cursor.visible)
        {
            Cursor.visible = false;
            CM_FreeLook1.SetActive(true);
        }
       

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
                rigid.AddForce(transform.up*jumpSpeed*2,ForceMode.VelocityChange);
                Jump_music.Play();
                isJump = false;
            }
        }
        
        //读取WASD
        Vector2 movement = movementControl.action.ReadValue<Vector2>();
        moveScale = 0;
        if (!Cursor.visible)
        {
            //2.角色移动操作[WASD]
            if (movement.x != 0f || movement.y != 0f)
            {
                //相机跟随旋转
                moveDirection = new Vector3(movement.x, 0, movement.y);
                moveDirection = cameraMainTransform.forward * moveDirection.z + cameraMainTransform.right * moveDirection.x;
                moveDirection.y = 0f;
                //执行移动操作
                if (moveScale == 1) rigid.MovePosition((rigid.position + transform.forward * moveSpeed * Time.deltaTime));
                anim.SetBool("Run", true);
                moveScale = 1;
            }
            else
            {
                anim.SetBool("Run", false);
                moveScale = 0;
            }
        
            // 3.旋转操作[mouse control]
            Quaternion rot = Quaternion.Euler(0, Vector3.SignedAngle(Vector3.forward, moveDirection, Vector3.up), 0);//目标旋转
            Quaternion currentRot = Quaternion.RotateTowards(rigid.rotation, rot, Time.deltaTime * 600);//中间插值旋转
            rigid.MoveRotation(currentRot);

        }
        //4.判断是否掉入水中
         if (transform.position.y < -60)
         {
             Revive_music.Play();
             revive();
         }


    }

    // 判断复活点
    private void revive()
    {
        if (transform.position.z > -31 && transform.position.z < 115)
            transform.position = new Vector3(1, 1, -30);
        else if (transform.position.z >= 115 && transform.position.z < 184)
            transform.position = new Vector3(9, 27, 115);
        else if (transform.position.z >= 184 && transform.position.z < 242)
            transform.position = new Vector3(9, 27, 184);
        else if (transform.position.z >= 242 && transform.position.z < 316)
            transform.position = new Vector3(-1, 1, 242);
        else if (transform.position.z >= 316 && transform.position.z < 385)
            transform.position = new Vector3(-3, 1, 316);
        else if (transform.position.z >= 385 && transform.position.z < 450)
            transform.position = new Vector3(0, 1, 385);
        else if (transform.position.z >= 450 && transform.position.z < 556)
            transform.position = new Vector3(0, 1, 450);
        else if (transform.position.z >= 556)
            transform.position = new Vector3(1, 1, -30);
    }
    // 刚体移动
    private void FixedUpdate()
    {
        if (moveScale == 1)
        {
            if (Input.GetKey(KeyCode.LeftShift) && isSprintSpeed)
            {
                rigid.MovePosition(rigid.position + transform.forward * sprintSpeed * Time.fixedDeltaTime);
                Sprint_music.Play();
            }
            else 
                rigid.MovePosition(rigid.position + transform.forward * moveSpeed * Time.fixedDeltaTime);
        }
    }

    // 碰撞机制
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
            Candy_music.Play();
            isFreeze = true;
            setLevel3();
        }

        if (collision.gameObject.tag == "ManyHoleFly")//如果使用多转盘羽毛，则直接飞到独木桥底部
        {
            
            // Debug.Log("销毁"+collision.gameObject.tag);
            Destroy(collision.gameObject);
            Fly_music.Play();
            Invoke("StopFlyMusic",3f);
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
            Fly_music.Play();
            Invoke("StopFlyMusic",3f);
            rigid.AddForce(transform.up*25,ForceMode.VelocityChange);//角色先起飞
            GameObject flag = GameObject.Find("翅膀_目的地2");
            transform.LookAt(flag.transform);
            rigid.AddForce(transform.forward*8,ForceMode.VelocityChange);
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
            Candy_music.Play();
        }

        if (collision.gameObject.tag == "SpecialShoes")//如果使用特殊羽毛鞋，则可以进行二段跳，8秒
        {
            isDoubleJump = true;
            // Debug.Log("销毁"+collision.gameObject.tag);
            Destroy(collision.gameObject);
            Candy_music.Play();
        }

        if (collision.gameObject.tag == "EndFlag"&&vector_flag)
        {
            Vectory.Play();
            Vectory2.Play();
            vector_flag = false;
        }

        if (collision.gameObject.tag == "Trampoline")
        {
            Jump_music.Play();
        }

        if (collision.gameObject.tag == "Obstacle")
        {
            Obstacle_music.Play();
        }
    }
    // 设置物体的重力加速度
    public static void SetGravity(GameObject obj, Vector3 val)
    {
        var c = obj.GetComponent<GravityControl>();
        if (c == null)
            c = obj.AddComponent<GravityControl>();
 
        c.Set(val);
    }
    
    // 获取物体的重力加速度
    public static Vector3 GetGravity(GameObject obj)
    {
        var c = obj.GetComponent<GravityControl>();
        if (c == null)
            return Physics.gravity;
        else
            return c.Get();
    }

    // 解除加速
    private void setSprintSpeed()
    {
        isSprintSpeed = false;
    }

    // 解除二段跳
    private void setDoubleJump()
    {
        isDoubleJump = false;
    }

    // 解除冻结
    private void setFreeze()
    {
        isFreeze = false;
    }

    // 解除冻结2
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
    
    //解除飞翔声音
    private void StopFlyMusic()
    {
        Fly_music.Stop();
    }
}
