using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 控制单个物体的重力加速度(附加额外的加速度去补正全局设置)
/// </summary>
public class GravityControl : MonoBehaviour
{
    private Vector3 _newGravity = Physics.gravity;
    private Vector3 _compensate = Vector3.zero;
    private Rigidbody _rig;
 
    private void Start()
    {
        _rig = gameObject.GetComponent<Rigidbody>();
    }
 
    public void Set(Vector3 val)
    {
        if (_newGravity == val)
            return;
 
        _newGravity = val;
        _compensate = -(Physics.gravity - val);
    }
 
    public Vector3 Get() { return _newGravity; }
 
    private void FixedUpdate()
    {
        if (!_rig.isKinematic && !_rig.IsSleeping() && _compensate!=Vector3.zero)
        {
            _rig.AddForce(_compensate, ForceMode.Acceleration);
        }
    }
}