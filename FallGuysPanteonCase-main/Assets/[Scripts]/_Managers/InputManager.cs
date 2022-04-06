using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;

[RequireComponent(typeof(NonDrawingGraphic))]
public class InputManager : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public LayerMask layerMask;
    public PointerEventData eventData;
    public bool useCursor = true;

    private RectTransform cursor;

    [HideInInspector] public LayerMask defaultLayerMask;
    [HideInInspector] public Vector2 defaultOffset;

    #region Singleton
    public static InputManager instance = null;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    #endregion

    private void Start()
    {
        defaultLayerMask = layerMask;
        defaultOffset = rayOffset;
        cursor = transform.GetChild(0).GetComponent<RectTransform>();
        cursor.localScale = Vector3.zero;
    }
    //// WARNING!!!
    //// comment this block and uncomment neccessary block first
    //// ********************************************************
    //// ********************************************************
    //public void OnPointerDown(PointerEventData _eventData) { }
    //public void OnPointerUp(PointerEventData _eventData) { }
    //// ********************************************************
    //// ********************************************************
    //// END_OF_WARNING!!

    #region RaycastFromTouchPoint
    // if we need send raycast from tap point on screen, uncomment this block
    public void OnPointerDown(PointerEventData _eventData)
    {
        eventData = _eventData;
        if (useCursor) cursor.DOScale(1f, 0.25f);
    }

    public void OnPointerUp(PointerEventData _eventData)
    {
        eventData = null;
        if (useCursor) cursor.DOScale(0f, 0.25f);
    }

    private void Update()
    {
        if (!useCursor || eventData == null) return;
        cursor.position = eventData.position + _rayOffset;
    }

    public RaycastHit hit;
    public Vector2 rayOffset = new Vector2(0, 100); //change this if need to offset
    private Vector2 _rayOffset
    {
        get
        {
            return new Vector2(rayOffset.x, rayOffset.y);
        }
    }
    private void FixedUpdate()
    {
        if (eventData == null) return;

        Ray ray;

        if (useCursor) ray = CameraController.instance.cam.ScreenPointToRay(cursor.position);
        else ray = CameraController.instance.cam.ScreenPointToRay(eventData.position + rayOffset);

        Physics.Raycast(ray, out hit, 100f, layerMask);
    }
    #endregion

    #region ContinousInput
    //if we need to input as X, Y continuously, uncomment this block
    // public void OnPointerDown(PointerEventData _eventData)
    // {
    //     eventData = _eventData;
    //     startedPos = eventData.position;
    // }

    // public void OnPointerUp(PointerEventData _eventData)
    // {  
    //     eventData = null;
    //     delta = Vector2.zero;
    //     startedPos = Vector2.zero;
    //     input = Vector2.zero;
    // }

    // private Vector2 startedPos;
    // private Vector2 delta;
    // public Vector2 input;
    // public float maxDistance = 100f; // Change this to according the desired precision 
    // private void FixedUpdate()
    // {
    //     if (eventData == null) return;
    //     delta = eventData.position - startedPos;
    //     delta.x = Mathf.Clamp(delta.x, -maxDistance, maxDistance);
    //     delta.y = Mathf.Clamp(delta.y, -maxDistance, maxDistance);
    //     input = delta / maxDistance;
    // }
    #endregion

    #region InputDelta
    //if we need to input as X, Y continuously, uncomment this block
    // public void OnPointerDown(PointerEventData _eventData)
    // {
    //     eventData = _eventData;
    //     startedPos = eventData.position;
    // }

    // public void OnPointerUp(PointerEventData _eventData)
    // {
    //     eventData = null;
    //     delta = Vector2.zero;
    //     startedPos = Vector2.zero;
    //     input = Vector2.zero;
    // }

    // private Vector2 startedPos;
    // private Vector2 delta;
    // public Vector2 input;
    // public float maxDistance = 100f; // Change this to according the desired precision 
    // private void FixedUpdate()
    // {
    //     if (eventData == null) return;
    //     delta = eventData.position - startedPos;
    //     delta.x = Mathf.Clamp(delta.x, -maxDistance, maxDistance);
    //     delta.y = Mathf.Clamp(delta.y, -maxDistance, maxDistance);
    //     input = delta / maxDistance;
    //     startedPos = eventData.position;
    // }
    #endregion

    #region SlideInput4Direction    
    // public void OnPointerDown(PointerEventData _eventData)
    // {
    //     eventData = _eventData;
    //     startedPos = eventData.position;
    // }

    // public void OnPointerUp(PointerEventData _eventData)
    // {
    //     eventData = null;
    // }

    // [HideInInspector] public UnityEvent slideLeft = new UnityEvent();
    // [HideInInspector] public UnityEvent slideRight = new UnityEvent();
    // [HideInInspector] public UnityEvent slideUp = new UnityEvent();
    // [HideInInspector] public UnityEvent slideDown = new UnityEvent();
    // private Vector2 startedPos;
    // public float precisionDistance = 50;
    // private void FixedUpdate()
    // {
    //     if (eventData == null) return;

    //     Vector2 diff = eventData.position - startedPos;

    //     if (diff.x <= -precisionDistance)
    //     {
    //         //slide left
    //         slideLeft.Invoke();
    //         OnPointerUp(eventData);
    //     }
    //     else if (diff.x >= precisionDistance)
    //     {
    //         //slide right
    //         slideRight.Invoke();
    //         OnPointerUp(eventData);
    //     }
    //     else if (diff.y >= precisionDistance)
    //     {
    //         //slide up
    //         slideUp.Invoke();
    //         OnPointerUp(eventData);
    //     }
    //     else if (diff.y <= -precisionDistance)
    //     {
    //         //slide down
    //         slideDown.Invoke();
    //         OnPointerUp(eventData);
    //     }
    // }
    #endregion

    #region SlideInput2Direction
    // public void OnPointerDown(PointerEventData _eventData)
    // {
    //     eventData = _eventData;
    //     startedPos = eventData.position;
    // }

    // public void OnPointerUp(PointerEventData _eventData)
    // {
    //     eventData = null;
    // }

    // [HideInInspector] public UnityEvent horizontalSlide = new UnityEvent();
    // [HideInInspector] public UnityEvent verticalSlide = new UnityEvent();

    // private Vector2 startedPos;
    // public float precisionDistance = 50;
    // private void FixedUpdate()
    // {
    //     if (eventData == null) return;
    //     Vector2 diff = eventData.position - startedPos;
    //     if (Mathf.Abs(diff.x) > precisionDistance)
    //     {
    //         // horizontal slide
    //         horizontalSlide.Invoke();
    //         OnPointerUp(eventData);
    //     }
    //     else if (Mathf.Abs(diff.y) > precisionDistance)
    //     {
    //         // vertical slide
    //         verticalSlide.Invoke();
    //         OnPointerUp(eventData);
    //     }
    // }
    #endregion
}