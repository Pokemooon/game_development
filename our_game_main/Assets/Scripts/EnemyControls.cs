using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControls : MonoBehaviour
{
    public Transform[] DoorPointsLv1;
    public Transform[] DoorPointsLv2;
    public Transform[] DoorPointsLv3;
    public Transform[] DoorPointsLv4;
    public Transform[] DoorPointsLv5;
    public Transform[] DoorPointsLv6;
    public Transform[] DoorPointsLv7;
    public Transform[] EndDoorPoints;
    [SerializeField]
    private float enemySpeed = 2.0f;
    [SerializeField]
    private float jumpHeight = 1.0f;
    [SerializeField]
    private float gravityValue = -9.81f;
    [SerializeField]
    private float rotationSpeed = 4f;
    [SerializeField]
    private float minDistance;
    [SerializeField]
    private float maxDistance;
    [SerializeField]
    private float switchTime;
    [SerializeField]
    private float returnDistance;


    private CharacterController controller;
    private Vector3 enemyVelocity;
    private bool isGround;
    private Animator anim;
    private int level1DoorNum;
    private int level2DoorNum;
    private int level3DoorNum;
    private int level4DoorNum;
    private int level5DoorNum;
    private int level6DoorNum;
    private int level7DoorNum;
    private int levelEndDoorNum;

    private int currentLevel;
    public static bool flag;
    private float time;

    private bool lv1SetFlag = true;
    private bool lv2SetFlag = true;
    private bool lv3SetFlag = true;
    private bool lv4SetFlag = true;
    private bool lv5SetFlag = true;
    private bool lv6SetFlag = true;
    private bool lv7SetFlag = true;


    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
        anim = GetComponent<Animator>();

        level1DoorNum = Random.Range(0, DoorPointsLv1.Length);
        level2DoorNum = Random.Range(0, DoorPointsLv2.Length);
        level3DoorNum = Random.Range(0, DoorPointsLv3.Length);
        level4DoorNum = Random.Range(0, DoorPointsLv4.Length);
        level5DoorNum = Random.Range(0, DoorPointsLv5.Length);
        level6DoorNum = Random.Range(0, DoorPointsLv6.Length);
        level7DoorNum = Random.Range(0, DoorPointsLv7.Length);
        levelEndDoorNum = Random.Range(0, EndDoorPoints.Length);
        currentLevel = 1;
        flag = false;
        time = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(flag)
        {
            isGround = controller.isGrounded; //判断Enemy是否在地面
            if (isGround && enemyVelocity.y < 0)
            {
                enemyVelocity.y = 0f;
                anim.SetBool("Jump", false);
            }

            //处理Enemy下落
            enemyVelocity.y += gravityValue * Time.deltaTime;
            controller.Move(enemyVelocity * Time.deltaTime);

            Vector3 moveDirection = Vector3.zero;
            Vector2 enemyTrans = Vector2.zero;
            Vector2 doorTrans = Vector2.zero;
            float dis = 10000f;
            
            

            switch (currentLevel)
            {
                case 1:
                    if(GameCtrl.isLv1Open && lv1SetFlag)
                    {
                        //float d1; float d2; float d3;
                        
                        for (int i = 0; i < GenRandomPassDoor.lv1.Length; i++)
                        {
                            if (GenRandomPassDoor.lv1[i] == 1)
                            {
                                level1DoorNum = i; //设置新的可通过门
                                lv1SetFlag = false;
                            }
                        }
                    }
                    moveDirection = (DoorPointsLv1[level1DoorNum].position - transform.position).normalized;
                    moveDirection.y = 0f;
                    Run(moveDirection);
                    RandomJump();
                    enemyTrans = new Vector2(transform.position.x, transform.position.z);
                    doorTrans = new Vector2(DoorPointsLv1[level1DoorNum].position.x, DoorPointsLv1[level1DoorNum].position.z);
                    dis = (enemyTrans - doorTrans).sqrMagnitude;
                    if (dis < minDistance)
                    {
                        currentLevel = 2;
                        GameCtrl.isLv1Open = true;
                        Jump();
                    }
                    //如果dis大于minDistance，又小于某个值，且持续时间3秒钟，判定为撞墙了
                    if (dis > minDistance && dis < maxDistance)
                    {
                        time = time + Time.deltaTime;
                        if(time > switchTime)
                        {
                            for(int i = 0; i<GenRandomPassDoor.lv1.Length; i++)
                            {
                                if (GenRandomPassDoor.lv1[i] == 1)
                                {
                                    //Debug.Log("Pass Door:" + i);
                                    level1DoorNum = i; //设置新的可通过门
                                }
                            }
                            //向后闪现一定距离，然后选择其他通路
                            transform.position = new Vector3(DoorPointsLv1[level1DoorNum].position.x, 
                                                             transform.position.y, 
                                                             DoorPointsLv1[level1DoorNum].position.z - returnDistance);
                            time = 0f;
                        }
                    }
                    break;
                case 2:
                    if (GameCtrl.isLv2Open && lv2SetFlag)
                    {
                        for (int i = 0; i < GenRandomPassDoor.lv2.Length; i++)
                        {
                            if (GenRandomPassDoor.lv2[i] == 1)
                            {
                                level2DoorNum = i; //设置新的可通过门
                                lv2SetFlag = false;
                            }
                        }
                    }
                    moveDirection = (DoorPointsLv2[level2DoorNum].position - transform.position).normalized;
                    moveDirection.y = 0f;
                    Run(moveDirection);
                    RandomJump();
                    enemyTrans = new Vector2(transform.position.x, transform.position.z);
                    doorTrans = new Vector2(DoorPointsLv2[level2DoorNum].position.x, DoorPointsLv2[level2DoorNum].position.z);
                    dis = (enemyTrans - doorTrans).sqrMagnitude;
                    if (dis < minDistance)
                    {
                        currentLevel = 3;
                        GameCtrl.isLv2Open = true;
                        Jump();
                    }
                    //如果dis大于minDistance，又小于某个值，且持续时间3秒钟，判定为撞墙了
                    if (dis > minDistance && dis < maxDistance)
                    {
                        time = time + Time.deltaTime;
                        if (time > switchTime)
                        {
                            for (int i = 0; i < GenRandomPassDoor.lv2.Length; i++)
                            {
                                if (GenRandomPassDoor.lv2[i] == 1)
                                {
                                    //Debug.Log("Pass Door:" + i);
                                    level2DoorNum = i; //设置新的可通过门
                                }
                            }
                            //向后闪现一定距离，然后选择其他通路
                            transform.position = new Vector3(DoorPointsLv2[level2DoorNum].position.x,
                                                             transform.position.y,
                                                             DoorPointsLv2[level2DoorNum].position.z - returnDistance);
                            time = 0f;
                        }
                    }
                    break;
                case 3:
                    if (GameCtrl.isLv3Open && lv3SetFlag)
                    {
                        for (int i = 0; i < GenRandomPassDoor.lv3.Length; i++)
                        {
                            if (GenRandomPassDoor.lv3[i] == 1)
                            {
                                level3DoorNum = i; //设置新的可通过门
                                lv3SetFlag = false;
                            }
                        }
                    }
                    moveDirection = (DoorPointsLv3[level3DoorNum].position - transform.position).normalized;
                    moveDirection.y = 0f;
                    Run(moveDirection);
                    RandomJump();
                    enemyTrans = new Vector2(transform.position.x, transform.position.z);
                    doorTrans = new Vector2(DoorPointsLv3[level3DoorNum].position.x, DoorPointsLv3[level3DoorNum].position.z);
                    dis = (enemyTrans - doorTrans).sqrMagnitude;
                    if (dis < minDistance)
                    {
                        currentLevel = 4;
                        GameCtrl.isLv3Open = true;
                        Jump();
                    }
                    //如果dis大于minDistance，又小于某个值，且持续时间3秒钟，判定为撞墙了
                    if (dis > minDistance && dis < maxDistance)
                    {
                        time = time + Time.deltaTime;
                        if (time > switchTime)
                        {
                            for (int i = 0; i < GenRandomPassDoor.lv3.Length; i++)
                            {
                                if (GenRandomPassDoor.lv3[i] == 1)
                                {
                                    //Debug.Log("Pass Door:" + i);
                                    level3DoorNum = i; //设置新的可通过门
                                }
                            }
                            //向后闪现一定距离，然后选择其他通路
                            transform.position = new Vector3(DoorPointsLv3[level3DoorNum].position.x,
                                                             transform.position.y,
                                                             DoorPointsLv3[level3DoorNum].position.z - returnDistance);
                            time = 0f;
                        }
                    }
                    break;
                case 4:
                    if (GameCtrl.isLv4Open && lv4SetFlag)
                    {
                        for (int i = 0; i < GenRandomPassDoor.lv4.Length; i++)
                        {
                            if (GenRandomPassDoor.lv4[i] == 1)
                            {
                                level4DoorNum = i; //设置新的可通过门
                                lv4SetFlag = false;
                            }
                        }
                    }
                    moveDirection = (DoorPointsLv4[level4DoorNum].position - transform.position).normalized;
                    moveDirection.y = 0f;
                    Run(moveDirection);
                    RandomJump();
                    enemyTrans = new Vector2(transform.position.x, transform.position.z);
                    doorTrans = new Vector2(DoorPointsLv4[level4DoorNum].position.x, DoorPointsLv4[level4DoorNum].position.z);
                    dis = (enemyTrans - doorTrans).sqrMagnitude;
                    if (dis < minDistance)
                    {
                        currentLevel = 5;
                        GameCtrl.isLv4Open = true;
                        Jump();
                    }
                    //如果dis大于minDistance，又小于某个值，且持续时间3秒钟，判定为撞墙了
                    if (dis > minDistance && dis < maxDistance)
                    {
                        time = time + Time.deltaTime;
                        if (time > switchTime)
                        {
                            for (int i = 0; i < GenRandomPassDoor.lv4.Length; i++)
                            {
                                if (GenRandomPassDoor.lv4[i] == 1)
                                {
                                    //Debug.Log("Pass Door:" + i);
                                    level4DoorNum = i; //设置新的可通过门
                                }
                            }
                            //向后闪现一定距离，然后选择其他通路
                            transform.position = new Vector3(DoorPointsLv4[level4DoorNum].position.x,
                                                             transform.position.y,
                                                             DoorPointsLv4[level4DoorNum].position.z - returnDistance);
                            time = 0f;
                        }
                    }
                    break;
                case 5:
                    if (GameCtrl.isLv5Open && lv5SetFlag)
                    {
                        for (int i = 0; i < GenRandomPassDoor.lv5.Length; i++)
                        {
                            if (GenRandomPassDoor.lv5[i] == 1)
                            {
                                level5DoorNum = i; //设置新的可通过门
                                lv5SetFlag = false;
                            }
                        }
                    }
                    moveDirection = (DoorPointsLv5[level5DoorNum].position - transform.position).normalized;
                    moveDirection.y = 0f;
                    Run(moveDirection);
                    RandomJump();
                    enemyTrans = new Vector2(transform.position.x, transform.position.z);
                    doorTrans = new Vector2(DoorPointsLv5[level5DoorNum].position.x, DoorPointsLv5[level5DoorNum].position.z);
                    dis = (enemyTrans - doorTrans).sqrMagnitude;
                    if (dis < minDistance)
                    {
                        currentLevel = 6;
                        GameCtrl.isLv5Open = true;
                        Jump();
                    }
                    //如果dis大于minDistance，又小于某个值，且持续时间3秒钟，判定为撞墙了
                    if (dis > minDistance && dis < maxDistance)
                    {
                        time = time + Time.deltaTime;
                        if (time > switchTime)
                        {
                            for (int i = 0; i < GenRandomPassDoor.lv5.Length; i++)
                            {
                                if (GenRandomPassDoor.lv5[i] == 1)
                                {
                                    //Debug.Log("Pass Door:" + i);
                                    level5DoorNum = i; //设置新的可通过门
                                }
                            }
                            //向后闪现一定距离，然后选择其他通路
                            transform.position = new Vector3(DoorPointsLv5[level5DoorNum].position.x,
                                                             transform.position.y,
                                                             DoorPointsLv5[level5DoorNum].position.z - returnDistance);
                            time = 0f;
                        }
                    }
                    break;
                case 6:
                    if (GameCtrl.isLv6Open && lv6SetFlag)
                    {
                        for (int i = 0; i < GenRandomPassDoor.lv6.Length; i++)
                        {
                            if (GenRandomPassDoor.lv6[i] == 1)
                            {
                                level6DoorNum = i; //设置新的可通过门
                                lv6SetFlag = false;
                            }
                        }
                    }
                    moveDirection = (DoorPointsLv6[level6DoorNum].position - transform.position).normalized;
                    moveDirection.y = 0f;
                    Run(moveDirection);
                    RandomJump();
                    enemyTrans = new Vector2(transform.position.x, transform.position.z);
                    doorTrans = new Vector2(DoorPointsLv6[level6DoorNum].position.x, DoorPointsLv6[level6DoorNum].position.z);
                    dis = (enemyTrans - doorTrans).sqrMagnitude;
                    if (dis < minDistance)
                    {
                        currentLevel = 7;
                        GameCtrl.isLv6Open = true;
                        Jump();
                    }
                    //如果dis大于minDistance，又小于某个值，且持续时间3秒钟，判定为撞墙了
                    if (dis > minDistance && dis < maxDistance)
                    {
                        time = time + Time.deltaTime;
                        if (time > switchTime)
                        {
                            for (int i = 0; i < GenRandomPassDoor.lv6.Length; i++)
                            {
                                if (GenRandomPassDoor.lv6[i] == 1)
                                {
                                    //Debug.Log("Pass Door:" + i);
                                    level6DoorNum = i; //设置新的可通过门
                                }
                            }
                            //向后闪现一定距离，然后选择其他通路
                            transform.position = new Vector3(DoorPointsLv6[level6DoorNum].position.x,
                                                             transform.position.y,
                                                             DoorPointsLv6[level6DoorNum].position.z - returnDistance);
                            time = 0f;
                        }
                    }
                    break;
                case 7:
                    if (GameCtrl.isLv7Open && lv7SetFlag)
                    {
                        for (int i = 0; i < GenRandomPassDoor.lv7.Length; i++)
                        {
                            if (GenRandomPassDoor.lv7[i] == 1)
                            {
                                level7DoorNum = i; //设置新的可通过门
                                lv7SetFlag = false;
                            }
                        }
                    }
                    moveDirection = (DoorPointsLv7[level7DoorNum].position - transform.position).normalized;
                    moveDirection.y = 0f;
                    Run(moveDirection);
                    RandomJump();
                    enemyTrans = new Vector2(transform.position.x, transform.position.z);
                    doorTrans = new Vector2(DoorPointsLv7[level7DoorNum].position.x, DoorPointsLv7[level7DoorNum].position.z);
                    dis = (enemyTrans - doorTrans).sqrMagnitude;
                    if (dis < minDistance)
                    {
                        currentLevel = 8;
                        GameCtrl.isLv7Open = true;
                        Jump();
                    }
                    //如果dis大于minDistance，又小于某个值，且持续时间3秒钟，判定为撞墙了
                    if (dis > minDistance && dis < maxDistance)
                    {
                        time = time + Time.deltaTime;
                        if (time > switchTime)
                        {
                            for (int i = 0; i < GenRandomPassDoor.lv7.Length; i++)
                            {
                                if (GenRandomPassDoor.lv7[i] == 1)
                                {
                                    //Debug.Log("Pass Door:" + i);
                                    level7DoorNum = i; //设置新的可通过门
                                }
                            }
                            //向后闪现一定距离，然后选择其他通路
                            transform.position = new Vector3(DoorPointsLv7[level7DoorNum].position.x,
                                                             transform.position.y,
                                                             DoorPointsLv7[level7DoorNum].position.z - returnDistance);
                            time = 0f;
                        }
                    }
                    break;
                case 8:
                    moveDirection = (EndDoorPoints[levelEndDoorNum].position - transform.position).normalized;
                    moveDirection.y = 0f;
                    Run(moveDirection);
                    RandomJump();
                    enemyTrans = new Vector2(transform.position.x, transform.position.z);
                    doorTrans = new Vector2(EndDoorPoints[levelEndDoorNum].position.x, EndDoorPoints[levelEndDoorNum].position.z);
                    dis = (enemyTrans - doorTrans).sqrMagnitude;
                    if (dis < minDistance)
                    {
                        currentLevel = 9;
                        anim.SetBool("Run", false);
                    }
                    break;
                default:
                    break;
            }
        }        
    }

    void Run(Vector3 direction)
    {
        controller.Move(enemySpeed * direction * Time.deltaTime);
        anim.SetBool("Run", true);
        if(direction != Vector3.zero)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
            Quaternion rotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
        }
    }

    void Jump()
    {
        if (isGround)
        {
            enemyVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            anim.SetBool("Jump", true);
        }
    }

    void RandomJump()
    {
        if (isGround)
        {
            if(Random.Range(0,200) == 50)
            {
                enemyVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
                anim.SetBool("Jump", true);
            }
        }
    }
}
