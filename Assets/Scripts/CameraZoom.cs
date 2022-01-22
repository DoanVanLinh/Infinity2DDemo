using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{

    Camera mainCamera;

    float touchesPrevPosDifference, touchesCurPosDifference, zoomModifier;

    Vector2 firstTouchPrevPos, secondTouchPrevPos;

    [SerializeField]
    float zoomModifierSpeed = 0.1f;

    private Vector3 beginTouch;
    private Vector3 endTouch;

    private static CameraZoom instance;

    public static CameraZoom Instance { get => instance; set => instance = value; }
	public int Direction { get; set; }
	public bool IsHold { get; set; }
    #region Singleton
    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }
    #endregion
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch firstTouch = Input.GetTouch(0);
            Touch secondTouch = Input.GetTouch(1);

            firstTouchPrevPos = firstTouch.position - firstTouch.deltaPosition;
            secondTouchPrevPos = secondTouch.position - secondTouch.deltaPosition;

            touchesPrevPosDifference = (firstTouchPrevPos - secondTouchPrevPos).magnitude;
            touchesCurPosDifference = (firstTouch.position - secondTouch.position).magnitude;

            zoomModifier = (firstTouch.deltaPosition - secondTouch.deltaPosition).magnitude * zoomModifierSpeed;

            if (touchesPrevPosDifference > touchesCurPosDifference)
                mainCamera.orthographicSize += zoomModifier;
            if (touchesPrevPosDifference < touchesCurPosDifference)
                mainCamera.orthographicSize -= zoomModifier;

        }

        mainCamera.orthographicSize = Mathf.Clamp(mainCamera.orthographicSize, 2f, 100f);

        //change position
        if (Input.GetMouseButtonDown(1))
            beginTouch = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetMouseButton(1))
        {
            Vector3 direct = beginTouch - mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mainCamera.transform.position += direct * 0.2f;
        }
        if (Input.mouseScrollDelta.y != 0)
            mainCamera.orthographicSize += Time.deltaTime * 100f * Input.mouseScrollDelta.y > 0 ? -1 : 1;
		
		//mobile
		if(IsHold)
			mainCamera.orthographicSize += Time.deltaTime* 10f * Direction;
    }
	
}