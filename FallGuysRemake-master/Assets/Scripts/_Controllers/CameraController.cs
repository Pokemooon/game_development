using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class CameraController : MonoBehaviour
{
    #region Singleton
    public static CameraController instance = null;
    private void Awake()
    {
        if (instance == null) instance = this;
        cam = GetComponent<Camera>();
    }
    #endregion
    #region Variables

    public CameraUpdateType updateType;

    public Transform positionTarget, rotatitonTarget;

    public bool follow = false;
    public bool rotate = false;
    public bool rotateAllAxis = false;
    public bool rotateAnimation = false;
    public bool offset = false;

    public float mxdistance;
    public float mydistance;
    public float mzdistance;

    public float mangle;
    public float msmoothforposition;
    public float msmoothforrotation;
    public float msmoothforfov;

    public float shakemagnitude;
    public float cameraFov = 60f;

    public float rotateAnimationSpeed = 2f;
    public float rotateAnimationMinValue = 40f;
    public float rotateAnimationMaxValue = 120f;

    public Vector3 offsetvector;

    private Vector3 refVelocity;

    [SerializeField] private ParticleSystem confetti;
    [SerializeField] private ParticleSystem turbilance;
    [SerializeField] private ParticleSystem snow;
    [SerializeField] private ParticleSystem rain;

    private ParticleSystem.MainModule turbilanceMain;
    private ParticleSystem.EmissionModule turbilanceEmission;

    [HideInInspector] public Camera cam;

    #endregion
    #region Start - Update - FixedUpdate - LateUpdate
    private void FixedUpdate() { if (updateType == CameraUpdateType.Fixed) CameraFocus(); }
    private void LateUpdate() { if (updateType == CameraUpdateType.Late) CameraFocus(); }
    private void Update() { if (updateType == CameraUpdateType.Normal) CameraFocus(); }
    private void Start() { turbilanceMain = turbilance.main; turbilanceEmission = turbilance.emission; }
    #endregion
    #region Focus Components
    public void SetTarget(Transform _targetPosition, Transform _targetRotation = null)
    {
        positionTarget = _targetPosition;
        rotatitonTarget = _targetRotation ? _targetRotation : _targetPosition;
    }
    public void SetValues(float _mXDistance = 0f, float _mYDistance = 0f, float _mZDistance = 0f, float _mAngle = 0f)
    {
        mxdistance = _mXDistance;
        mydistance = _mYDistance;
        mzdistance = _mZDistance;
        mangle = _mAngle;
    }
    public void SetSmooths(float positionSmooth = 1f, float rotationSmooth = 5f, float fovSmooth = 2f)
    {
        msmoothforposition = positionSmooth;
        msmoothforrotation = rotationSmooth;
        msmoothforfov = fovSmooth;
    }
    public void SetOffset(Vector3 _offset)
    {
        offset = true;
        offsetvector = _offset;
    }
    public void SetSpecs(bool _follow = true, bool _rotate = true)
    {
        follow = _follow;
        rotate = _rotate;
    }
    public void SetSpecs(bool _rotateAllAxis = false, bool _rotateAnimation = false, bool _offset = false)
    {
        rotateAllAxis = _rotateAllAxis;
        rotateAnimation = _rotateAnimation;
        offset = _offset;
    }
    public void SetSpecs(bool _follow = true, bool _rotate = true, bool _rotateAllAxis = false, bool _rotateAnimation = false, bool _offset = false)
    {
        follow = _follow;
        rotate = _rotate;
        rotateAllAxis = _rotateAllAxis;
        rotateAnimation = _rotateAnimation;
        offset = _offset;
    }
    #endregion
    #region CameraPhysics
    public void CameraShake(float duration = 0.25f, float _shakeMagnitude = 0.1f, bool _turbilance = false)
    {
        shakemagnitude = _shakeMagnitude;
        StartCoroutine(ShakeCorouitine(duration));
        if (_turbilance) PlayTurbilance();
    }
    public void RotateAnimation()
    {
        mangle += 1 * Time.fixedDeltaTime * rotateAnimationSpeed;
    }
    public void UpdateCameraShake(float _shakeMagnitude = 0.1f, bool _turbilance = false, float turbilanceSpeed = 1f)
    {
        float camerashakeoffsetx = Random.value * _shakeMagnitude * 2 - _shakeMagnitude;
        float camerashakeoffsety = Random.value * _shakeMagnitude * 2 - _shakeMagnitude;
        Vector3 cameraintermadiateposition = transform.position;
        cameraintermadiateposition.x += camerashakeoffsetx;
        cameraintermadiateposition.y += camerashakeoffsety;
        transform.position = cameraintermadiateposition;
        if (_turbilance) PlayTurbilance(turbilanceSpeed);
    }
    IEnumerator ShakeCorouitine(float _duration)
    {
        float time = Time.time + _duration;
        while (time > Time.time)
        {
            float camerashakeoffsetx = Random.value * shakemagnitude * 2 - shakemagnitude;
            float camerashakeoffsety = Random.value * shakemagnitude * 2 - shakemagnitude;
            Vector3 cameraintermadiateposition = transform.position;
            cameraintermadiateposition.x += camerashakeoffsetx;
            cameraintermadiateposition.y += camerashakeoffsety;
            transform.position = cameraintermadiateposition;
            yield return null;
        }
    }
    private void CameraFocus()
    {
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, cameraFov, Time.fixedDeltaTime * msmoothforfov * 3f);

        if (!positionTarget) return;

        Vector3 worldposition = (Vector3.forward * mxdistance) + (Vector3.up * mydistance) + (Vector3.right * mzdistance);
        Vector3 rotatedvector = Quaternion.AngleAxis(mangle, Vector3.up) * worldposition;
        Vector3 flattargetposition = positionTarget.position;
        Vector3 finalposition = flattargetposition + rotatedvector + (offset ? offsetvector : Vector3.zero);

        if (follow) transform.position = Vector3.SmoothDamp(transform.position, finalposition, ref refVelocity, msmoothforposition);
        if (!rotatitonTarget) return;
        if (rotate)
        {
            Vector3 lookPos = (rotatitonTarget.position + (offset ? offsetvector : Vector3.zero)) - transform.position;

            if (!rotateAllAxis) lookPos.y = 0;

            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * msmoothforrotation);
        }
        if (rotateAnimation) { RotateAnimation(); }
    }
    #endregion
    #region Particle - Enum 
    public void PlayConfetti() => confetti?.Play();
    public void PlaySnow() => snow?.Play();
    public void PlayRain() => rain?.Play();
    public void PlayTurbilance(float _speed = 1f)
    {
        turbilance?.Play();

        turbilanceMain.startSpeed = _speed * 50;

        turbilanceEmission.rateOverTime = _speed * 100;
    }
    [System.Serializable]
    public enum CameraUpdateType
    {
        Fixed,
        Late,
        Normal,
    }
    #endregion
}
