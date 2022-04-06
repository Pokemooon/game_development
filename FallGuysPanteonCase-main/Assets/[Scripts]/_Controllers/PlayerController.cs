using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class PlayerController : MonoBehaviour
{
    #region Variables
    [SerializeField] private float runSpeed;
    [SerializeField] private float turnSpeed;

    private Animator mAnimator;
    private Collider mCollider;
    private Rigidbody mRigidbody;
    private Painter mPainter;

    private List<Rigidbody> ragdollRB = new List<Rigidbody>();
    private List<BoneStats> bones = new List<BoneStats>();

    private Transform root;
    private Transform lookAt = null;
    private Quaternion rotation = Quaternion.identity;
    private CharacterState state = CharacterState.None;

    private bool move = false;
    private float currentSpeed;

    private Vector3 lastPosition;

    private Coroutine fallCoroutine = null;
    private Outline outline;
    #endregion
    #region Awake - Start
    private void Awake()
    {
        mAnimator = GetComponentInChildren<Animator>(); root = mAnimator.transform.GetChild(0).transform;
        mPainter = GetComponent<Painter>();
        mCollider = GetComponent<Collider>();
        mRigidbody = GetComponent<Rigidbody>();
        outline = GetComponentInChildren<Outline>();
        ragdollRB = GetComponentsInChildren<Rigidbody>().Where(x => x.gameObject != gameObject).ToList();
    }
    private void Start()
    {
        if(lookAt == null) 
        {
            GameObject go = new GameObject();
            go.transform.SetParent(transform);
            go.transform.localPosition = go.transform.localEulerAngles = Vector3.zero;
            lookAt = go.transform;
        }
        
        foreach (Rigidbody rb in ragdollRB) { rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; rb.isKinematic = true; rb.velocity = Vector3.zero; rb.gameObject.layer = 9; }
        for(int i = 0; i < ragdollRB.Count; i++) 
        {
            BoneStats stat = new BoneStats();
            stat.localEulerAngles = ragdollRB[i].transform.localEulerAngles;
            stat.localPosition = ragdollRB[i].transform.localPosition;
            bones.Add(stat);
        }

        LevelManager.instance.startEvent.AddListener(() => 
        {
            move = true;
            outline.enabled = true;
        });

        state = CharacterState.Idle;
        outline.enabled = false;
        mPainter.enabled = false;

    }
    #endregion
    #region Update - FixedUpdate - Collision - Phyics
    private void Update()
    {
        InputValues();
    }
    private void FixedUpdate()
    {
        Movement();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Obstacle"))
        {
            EnableRagdoll();
            state = CharacterState.Falling;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Boundary"))
        {
            EnableRagdoll();
            state = CharacterState.Falling;
        }
        if(other.CompareTag("Finish"))
        {
            mPainter.enabled = true;
            CameraController.instance.SetTarget(Level.instance.paintableWall,Level.instance.paintableWall);
            CameraController.instance.SetSmooths(1, 1);
            CameraController.instance.SetOffset(Vector3.zero);
            CameraController.instance.SetValues(-7.5f,0, 0);
            mAnimator.SetTrigger("Dance");
            move = false;
            state = CharacterState.Painting;

            UIManager.instance.gamePanel.AppaearSlider();
        }
    }
    private void Movement()
    {
        if (!move) 
        {
            if(state == CharacterState.Falling)
            {
                if (fallCoroutine == null) { fallCoroutine = StartCoroutine(FallCoroutine()); outline.enabled = false; }
            }
            else if (state == CharacterState.Painting)
            {
                float paintRate = Level.instance.paintableWall.GetComponent<PaintArea>().paintRate;
                UIManager.instance.gamePanel.processSlider.value = paintRate;
                UIManager.instance.gamePanel.processText.text = (paintRate * 100).ToString("00") + "%";

                if (paintRate * 100 == 100)
                {
                    mPainter.enabled = false;

                    CameraController.instance.SetTarget(transform, transform);
                    CameraController.instance.SetValues(-15, 10, 0);
                    CameraController.instance.rotateAnimation = true;
                    CameraController.instance.PlayConfetti();

                    LevelManager.instance.Success();

                    state = CharacterState.None;

                    UIManager.instance.gamePanel.processText.text = 100 + "%";
                }
            }
            return;
        }
        if (hold)
        {
            if (state != CharacterState.Running)
            {
                mAnimator.SetTrigger("Run");
                mRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; state = CharacterState.Running;
                outline.enabled = true;
            }
            currentSpeed = (lastPosition - transform.position).magnitude * 10;
            lastPosition = transform.position;

            mAnimator.SetFloat("Speed", runSpeed /7.5f + (currentSpeed));
            
            var lookPos = lookAt.position - transform.position;
            lookPos.y = 0;
            rotation = Quaternion.LookRotation(lookPos);
            lookAt.position = new Vector3(transform.position.x - x, transform.position.y, transform.position.z - y);
            mRigidbody.MovePosition(transform.position + (transform.forward * Time.fixedDeltaTime * runSpeed));
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);

        }
        else if (state != CharacterState.Idle)
        {
            mRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            lookAt.localPosition = Vector3.zero;
            mAnimator.SetTrigger("Idle");

            state = CharacterState.Idle;
        }
    }
    private void DisableRagdoll()
    {
        state = CharacterState.None;
        foreach(Rigidbody rb in ragdollRB) { rb.isKinematic = true; rb.velocity = Vector3.zero; }
        FixBones();
        mAnimator.enabled = true;
        mRigidbody.isKinematic = mCollider.isTrigger = false;
        move = true;
    }
    private void EnableRagdoll()
    {
        mAnimator.enabled = false;
        mRigidbody.isKinematic = mCollider.isTrigger = true;
        foreach (Rigidbody rb in ragdollRB) { rb.isKinematic = false; }
        move = false;
    }
    private void FixBones()
    {
        for(int i = 0; i < ragdollRB.Count; i++)
        {
            ragdollRB[i].transform.localPosition = bones[i].localPosition;
            ragdollRB[i].transform.localEulerAngles = bones[i].localEulerAngles;
        }
    }
    private void ReSpawn()
    {
        Transform point = Level.instance.GetSpawnPoint(transform.position);
        transform.position = point.position;
        transform.rotation = point.rotation;

        Transform pt = Instantiate(Resources.Load<GameObject>("particles/buff"), transform).transform;
        pt.position = transform.position;

        DisableRagdoll();
    }
    #endregion
    #region Input - Enum - Coroutine
    [System.Serializable]
    public class BoneStats
    {
        public Vector3 localPosition;
        public Vector3 localEulerAngles;
    }
    [System.Serializable] 
    public enum CharacterState
    {
        Running,
        Idle,
        Falling,
        Painting,
        None
    }
    private IEnumerator FallCoroutine()
    {
        yield return new WaitForSeconds(1f);

        ReSpawn();

        fallCoroutine = null;
    }

    [HideInInspector] public UnityEvent tap = new UnityEvent();

    private bool touch = false;
    private bool hold = false;

    private Vector2 startPos;
    private Vector2 touchPos;

    [HideInInspector] public float deltaX;
    [HideInInspector] public float deltaY;
    private float x;
    private float y;

    private float holdTimer = 0f;
    private void InputValues()
    {
        if (Input.GetMouseButton(0))
        {
            if (!touch)
            {
                startPos = Input.mousePosition;
                touch = true;
                holdTimer = Time.time;
            }
            else
            {
                if (holdTimer + 0.1f < Time.time) hold = true;
                touchPos = Input.mousePosition;
                deltaX = (startPos.x - touchPos.x);
                deltaY = (startPos.y - touchPos.y);
                if (Mathf.Abs(deltaX) > 200)
                {
                    if (deltaX > 0)
                        startPos.x = touchPos.x + 200;
                    else
                        startPos.x = touchPos.x - 200;
                }
                if (Mathf.Abs(deltaY) > 200)
                {
                    if (deltaY > 0)
                        startPos.y = touchPos.y + 200;
                    else
                        startPos.y = touchPos.y - 200;
                }
                x = Mathf.Clamp(deltaX / 200, -1, 1);
                y = Mathf.Clamp(deltaY / 200, -1, 1);

            }
        }
        else if (touch)
        {
            //if (!hold) tap.Invoke();
            holdTimer = 0;
            touch = hold = false;
            deltaX = deltaY = x = y = 0;
            startPos = Vector2.zero;
        }
    }
    #endregion
}
