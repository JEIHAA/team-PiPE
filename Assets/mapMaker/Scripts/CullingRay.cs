using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullingRay : MonoBehaviour
{
    public Camera playerCamera; // �÷��̾� ī�޶�
    public LayerMask cullingLayerMask; // Raycast�� ���� ���̾� ����ũ
    public float activationRadius = 10f; // �ֺ� Ȱ��ȭ ����
    private HashSet<GameObject> activeObjects = new HashSet<GameObject>(); // Ȱ��ȭ�� ������Ʈ���� ����
    private List<Renderer> allRenderers = new List<Renderer>(); // ��� ������Ʈ�� ������

    private bool initialize = false;

    public void InitializeRenderers()
    {
        // ó���� ��� ������Ʈ�� ��Ȱ��ȭ�ϰ�, ��� �������� ����
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

        // �þ� ���� �ְų� ���� ���� �ִ� ������Ʈ���� Ȱ��ȭ
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

        // �þ� �ۿ� �ְ� ���� �ۿ� �ִ� Ȱ��ȭ�� ������Ʈ���� ��Ȱ��ȭ
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