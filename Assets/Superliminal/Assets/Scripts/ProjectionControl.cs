using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionControl : MonoBehaviour
{
    [SerializeField] Camera viewCamera = null;
    [SerializeField] Camera projCamera = null;
    [SerializeField] LayerMask interatableLayer = 12;
    [SerializeField] PlayerMovement movement = null;
    [SerializeField] SuperliminalScale superliminalScale = null; 

    public bool dragging;
    ProjectionObject dragObject;
    float dragDistance;
    Vector3? placePoint;

    void Update()
    {
        // 프로젝션 카메라의 위치와 회전을 뷰 카메라와 일치
        projCamera.transform.position = viewCamera.transform.position;
        projCamera.transform.rotation = viewCamera.transform.rotation;

        
        Dragging();
        superliminalScale.enabled = dragging;
    }

    
    void Dragging()
    {       
        if (dragging)
        {
            // 마우스 스크롤로 드래그 거리를 조절
            dragDistance += Input.mouseScrollDelta.y * 10 * Time.deltaTime;
            dragObject.DragMove(transform.position + viewCamera.transform.forward * dragDistance);
           
            if (Input.GetMouseButtonUp(0))
            {
                dragging = false;
                dragObject.Fizzing();
            }
        }
        else
        {            
            if (Input.GetMouseButtonDown(0) && CanDrag())
            {              
                if (Physics.Raycast(viewCamera.transform.position, viewCamera.transform.forward, out RaycastHit hit, 15, interatableLayer))
                {
                    dragging = true;
                    dragObject = hit.collider.GetComponent<ProjectionObject>();
                    dragDistance = hit.distance;
                }
            }
        }
    }

    bool CanDrag()
    {
        return !(placePoint != null && ((Vector3)placePoint - transform.position).sqrMagnitude > 1f);
    }
}