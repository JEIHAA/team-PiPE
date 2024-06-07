using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullingRay : MonoBehaviour
{
    public Camera playerCamera; // 플레이어 카메라
    public LayerMask cullingLayerMask; // Raycast를 위한 레이어 마스크
    public float activationRadius = 10f; // 주변 활성화 범위
    private HashSet<GameObject> activeObjects = new HashSet<GameObject>(); // 활성화된 오브젝트들을 추적
    private List<Renderer> allRenderers = new List<Renderer>(); // 모든 오브젝트의 렌더러

    private bool initialize = false;

    public void InitializeRenderers()
    {
        // 처음에 모든 오브젝트를 비활성화하고, 모든 렌더러를 추적
        Renderer[] renderers = FindObjectsOfType<Renderer>();
        foreach (Renderer renderer in renderers)
        {
            if (((1 << renderer.gameObject.layer) & cullingLayerMask) != 0)
            {
                allRenderers.Add(renderer);
                renderer.gameObject.SetActive(false);
            }
        }
        initialize = true;
    }

    void Update()
    {
        if (initialize)
        {
            CullObjects();
        }
    }

    void CullObjects()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(playerCamera);
        Vector3 playerPosition = playerCamera.transform.position;

        // 시야 내에 있거나 범위 내에 있는 오브젝트들을 활성화
        foreach (Renderer renderer in allRenderers)
        {
            if (renderer == null) continue;
            GameObject obj = renderer.gameObject;

            bool inView = IsInView(obj, planes);
            bool withinRange = IsWithinRange(obj, playerPosition);

            if ((inView || withinRange) && !activeObjects.Contains(obj))
            {
                if (inView && !IsOccluded(obj))
                {
                    obj.SetActive(true);
                    activeObjects.Add(obj);
                }
                else if (withinRange)
                {
                    obj.SetActive(true);
                    activeObjects.Add(obj);
                }
            }
        }

        // 시야 밖에 있고 범위 밖에 있는 활성화된 오브젝트들을 비활성화
        foreach (GameObject obj in new HashSet<GameObject>(activeObjects))
        {
            bool inView = IsInView(obj, planes);
            bool withinRange = IsWithinRange(obj, playerPosition);

            if (!inView && !withinRange)
            {
                obj.SetActive(false);
                activeObjects.Remove(obj);
            }
        }
    }

    bool IsInView(GameObject obj, Plane[] planes)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer == null) return false;

        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }

    bool IsWithinRange(GameObject obj, Vector3 playerPosition)
    {
        return Vector3.Distance(obj.transform.position, playerPosition) <= activationRadius;
    }

    bool IsOccluded(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
        if (renderer == null) return true;

        Vector3 direction = (renderer.bounds.center - playerCamera.transform.position).normalized;
        float distance = Vector3.Distance(playerCamera.transform.position, renderer.bounds.center);

        if (Physics.Raycast(playerCamera.transform.position, direction, out RaycastHit hitInfo, distance, cullingLayerMask))
        {
            if (hitInfo.collider.gameObject != obj)
            {
                return true; // If the hit object is not the current object, it is occluded
            }
        }

        return false; // No occlusion
    }
}