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
        if (Input.GetKeyDown(KeyCode.Space)) {
            if (isGround)
            {
                // transform.Translate(transform.up*Time.deltaTime*jumpSpeed);
                rigid.AddForce(transform.up*jumpSpeed,ForceMode.VelocityChange);
                anim.SetBool("Jump",true);
                isGround = false;
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
    }

    private void FixedUpdate()
    {
        if (moveScale == 1) rigid.MovePosition((rigid.position + transform.forward * moveSpeed * Time.fixedDeltaTime));
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10)
        {
            isGround = true;
            anim.SetBool("Jump",false);
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
}