using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class AIController : MonoBehaviour
{
    #region Variables
    [SerializeField] private float runSpeed;
    [SerializeField] private float turnSpeed;

    private Animator mAnimator;
    private Collider mCollider;
    private Rigidbody mRigidbody;

    private List<Rigidbody> ragdollRB = new List<Rigidbody>();
    private List<PlayerController.BoneStats> bones = new List<PlayerController.BoneStats>();

    private Transform root;
    public Transform lookAt = null;
    private Quaternion rotation = Quaternion.identity;
    private PlayerController.CharacterState state = PlayerController.CharacterState.None;

    private bool move = false;
    private float currentSpeed;

    private Vector3 lastPosition;

    private Coroutine fallCoroutine = null;
    #endregion
    #region Awake - Start
    private void Awake()
    {
        mAnimator = GetComponentInChildren<Animator>(); root = mAnimator.transform.GetChild(0).transform;
        mCollider = GetComponent<Collider>();
        mRigidbody = GetComponent<Rigidbody>();
        ragdollRB = GetComponentsInChildren<Rigidbody>().Where(x => x.gameObject != gameObject).ToList();
    }
    private void Start()
    {
        foreach (Rigidbody rb in ragdollRB) { rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic; rb.isKinematic = true; rb.velocity = Vector3.zero; rb.gameObject.layer = 9; }
        for (int i = 0; i < ragdollRB.Count; i++)
        {
            PlayerController.BoneStats stat = new PlayerController.BoneStats();
            stat.localEulerAngles = ragdollRB[i].transform.localEulerAngles;
            stat.localPosition = ragdollRB[i].transform.localPosition;
            bones.Add(stat);
        }

        LevelManager.instance.startEvent.AddListener(() =>
        {
            move = true;
            GetLookAt();
        });

        state = PlayerController.CharacterState.Idle;
    }
    #endregion
    private void FixedUpdate()
    {
        Movement();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Obstacle"))
        {
            EnableRagdoll();
            state = PlayerController.CharacterState.Falling;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boundary"))
        {
            EnableRagdoll();
            state = PlayerController.CharacterState.Falling;
        }
        if (other.CompareTag("Finish") && move)
        {
            move = false;
            Level.instance.finishedPlayers++;
            mAnimator.SetTrigger("Dance");
            StartCoroutine(Delay(1.5f,() => 
            {
                transform.GetChild(0).gameObject.SetActive(false);
                mCollider.isTrigger = mRigidbody.isKinematic = true;
                Transform pt = Instantiate(Resources.Load<GameObject>("particles/buff"), transform).transform;
                pt.position = transform.position;
            }));
        }
    }
    private void Movement()
    {
        if (!move)
        {
            if (state == PlayerController.CharacterState.Falling)
            {
                if (fallCoroutine == null) { fallCoroutine = StartCoroutine(FallCoroutine());}
            }
            return;
        }
        if (lookAt != null)
        {
            if (state != PlayerController.CharacterState.Running)
            {
                mAnimator.SetTrigger("Run");
                mRigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ; state = PlayerController.CharacterState.Running;
            }
            currentSpeed = (lastPosition - transform.position).magnitude * 10;
            lastPosition = transform.position;

            mAnimator.SetFloat("Speed", runSpeed /7.5f + (currentSpeed));

            float distance = Vector3.Distance(transform.position, lookAt.position);

            if(lookAt.GetComponent<AITargetController>().safe || distance > 3f || distance < 1f)
            {
                mAnimator.Play("Run");
                mRigidbody.MovePosition(transform.position + (transform.forward * Time.fixedDeltaTime * runSpeed));
            }
            else
            {
                mAnimator.Play("Idle");
            }

            var lookPos = lookAt.position - transform.position;
            lookPos.y = 0;
            rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * turnSpeed);

            if (transform.position.z > lookAt.position.z - 1) GetLookAt();
        }
        else
        {
            mRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

            lookAt.localPosition = Vector3.zero;
            mAnimator.SetTrigger("Idle");

            state = PlayerController.CharacterState.Idle;
        }
    }
    private void DisableRagdoll()
    {
        state = PlayerController.CharacterState.None;
        foreach (Rigidbody rb in ragdollRB) { rb.isKinematic = true; rb.velocity = Vector3.zero; }
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
        for (int i = 0; i < ragdollRB.Count; i++)
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

        GetLookAt();

        DisableRagdoll();

        StartCoroutine(ReSpawnCoroutine());
    }
    private void GetLookAt()
    {
        lookAt = Level.instance.GetAITarget(transform.position);
    }
    private IEnumerator Delay(float _waitTime = 1f,UnityAction onComplete = null)
    {
        yield return new WaitForSeconds(_waitTime);
        onComplete?.Invoke();
    }
    private IEnumerator FallCoroutine()
    {
        yield return new WaitForSeconds(1f);

        ReSpawn();

        fallCoroutine = null;
    }
    private IEnumerator ReSpawnCoroutine()
    {
        gameObject.layer = 13;
        yield return new WaitForSeconds(1f);
        gameObject.layer = 10;
    }
}
