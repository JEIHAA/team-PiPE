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
        // �������� ī�޶��� ��ġ�� ȸ���� �� ī�޶�� ��ġ
        projCamera.transform.position = viewCamera.transform.position;
        projCamera.transform.rotation = viewCamera.transform.rotation;

        
        Dragging();
        superliminalScale.enabled = dragging;
    }

    
    void Dragging()
    {       
        if (dragging)
        {
            // ���콺 ��ũ�ѷ� �巡�� �Ÿ��� ����
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