using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHoverTarget : MonoBehaviour {

    public Transform TargetObject;
    public Vector3 Offset;
    public Canvas canvas;

    public string TagToFind = "Player";

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        if (!TargetObject)
            TargetObject = GameObject.FindGameObjectWithTag(TagToFind).transform;           

        if (TargetObject)
            transform.position = worldToUISpace(canvas, TargetObject.position) + Offset;
    }

    public Vector3 worldToUISpace(Canvas parentCanvas, Vector3 worldPos)
    {
        //Convert the world for screen point so that it can be used with ScreenPointToLocalPointInRectangle function
        Vector3 screenPos = Camera.main.WorldToScreenPoint(worldPos);
        Vector2 movePos;

        //Convert the screenpoint to ui rectangle local point
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentCanvas.transform as RectTransform, screenPos, parentCanvas.worldCamera, out movePos);
        //Convert the local point to world point
        return parentCanvas.transform.TransformPoint(movePos);
    }
}
