using EzySlice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.UI;
using System.Linq;
using StarterAssets;
public class PhotoCameraController : MonoBehaviour
{
    [SerializeField]
    private Camera Photocamera;

    private StarterAssetsInputs _input;

    private UnityEngine.Plane[] cameraFrustum;
    private GameObject[] allObjects;
    private List<GameObject> objectsInSight = new List<GameObject>();
    private bool hasPicture = false;

    [SerializeField]
    private GameObject prefabPicture;
    [SerializeField]
    private RenderTexture cameraView;
    [SerializeField]
    private PhotoUI photoUI;


    private GameObject[] cameraPlanes;
    private Vector3[] currFarCorners;
    private Vector3[] currNearCorners;


    [SerializeField]
    private GameObject planeGO;
    // Start is called before the first frame update

    private bool isCameraActivated = false;

    private GameObject photo;
    private GameObject HoldingPosPicture;
    private GameObject TakingPosPicture;
    private GameObject viewFinder;

    [SerializeField]
    private GameObject shadowProjector;

    private void Awake()
    {
        photo = photoUI.GetPhoto();
        HoldingPosPicture = photoUI.GetHoldingPos();
        TakingPosPicture = photoUI.GetTakingPos();
        viewFinder = photoUI.GetViewFinder();

    }

    private void Start()
    {
        Photocamera.transform.position = Camera.main.transform.position;
        Photocamera.transform.rotation = Camera.main.transform.rotation;
        DrawFrustum(Photocamera);
        _input = transform.root.GetComponent<StarterAssetsInputs>();

        photo.SetActive(false);
        allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        ActivateCamera(false);

        InitializePlanes();

    }

    private void InitializePlanes()
    {
        Vector3[] VerticesArray = new Vector3[3];
        int[] trianglesArray = new int[3];

        trianglesArray[0] = 0;
        trianglesArray[1] = 1;
        trianglesArray[2] = 2;

        Mesh m = new Mesh();
        GameObject currPlane;
        cameraPlanes = Array.Empty<GameObject>();
        
        for(int i =0; i<4; i++)
        {
            m = new Mesh();

            if(i == 3)
            {
                VerticesArray[0] = transform.position;
                VerticesArray[1] = currFarCorners[i];
                VerticesArray[2] = currFarCorners[0];
            }
            else
            {
                VerticesArray[0] = transform.position;
                VerticesArray[1] = currFarCorners[i];
                VerticesArray[2] = currFarCorners[i + 1];

            }

            m.vertices = VerticesArray;
            m.triangles = trianglesArray;

            currPlane = Instantiate(planeGO, Vector3.zero, Quaternion.identity);
            currPlane.GetComponent<MeshFilter>().mesh = m;
            currPlane.GetComponent<MeshCollider>().sharedMesh = m;
            cameraPlanes = cameraPlanes.Append(currPlane).ToArray();
        }

        VerticesArray = new Vector3[4];
        trianglesArray = new int[6];

        trianglesArray[0] = 0;
        trianglesArray[1] = 1;
        trianglesArray[2] = 2;
        trianglesArray[3] = 0;
        trianglesArray[4] = 2;
        trianglesArray[5] = 3;

        m = new Mesh();

        VerticesArray[0] = currFarCorners[0];
        VerticesArray[1] = currFarCorners[1];
        VerticesArray[2] = currFarCorners[2];
        VerticesArray[3] = currFarCorners[3];

        m.vertices = VerticesArray;
        m.triangles = trianglesArray;

        currPlane = Instantiate(planeGO, Vector3.zero, Quaternion .identity);
        currPlane.GetComponent <MeshFilter>().mesh = m;
        cameraPlanes = cameraPlanes.Append(currPlane).ToArray() ;

        Mesh newMesh = CombineAndDestroyMeshes(cameraPlanes);
        planeGO.GetComponent<MeshFilter>().mesh = newMesh;
        planeGO.GetComponent<MeshCollider>().sharedMesh = newMesh;
        planeGO.GetComponent<MeshCollider>().convex = true;
        planeGO.GetComponent<MeshCollider>().isTrigger = true;



    }

    private Mesh CombineAndDestroyMeshes(GameObject[] sourcesGO)
    {
        MeshFilter[] sourceMeshFilters = Array.Empty<MeshFilter>();
        foreach (GameObject go in sourcesGO)
        {
            sourceMeshFilters = sourceMeshFilters.Append(go.GetComponent<MeshFilter>()).ToArray();
            Destroy(go);
        }

        var combine = new CombineInstance[sourceMeshFilters.Length];

        for (int i = 0; i < sourceMeshFilters.Length; i++)
        {
            combine[i].mesh = sourceMeshFilters[i].sharedMesh;
            combine[i].transform = sourceMeshFilters[i].transform.localToWorldMatrix;
        }

        var mesh = new Mesh();
        mesh.CombineMeshes(combine);
        return mesh;
    }

    private void Update()
    {
        Photocamera.transform.position = Camera.main.transform.position;
        Photocamera.transform.rotation = Camera.main.transform.rotation;
        DrawFrustum(Photocamera);
        cameraFrustum = GeometryUtility.CalculateFrustumPlanes(Camera.main);

        if (hasPicture)
        {
            if (_input.lookPicture)
            {
                Debug.Log("asdads");
                LookPicture();
                ActivateCamera(false);
                _input.picture = false;
            }
            else
            {
                ActivateCamera(true);
                HoldPicture();
            }
        }

        if (_input.usePicture)
        {
            UsePicture();
            _input.usePicture = false;

        }
        if (_input.picture)
        {
            if (isCameraActivated)
            {
                TakePicture();
            }
            _input.picture = false;
        }

        if (_input.activateCamera)
        {
            
            isCameraActivated = !isCameraActivated;
            ActivateCamera(isCameraActivated);
            _input.activateCamera = false;
        }

    }

    private void UsePicture()
    {
        if (!hasPicture) { return; } // 사진이 없는 경우

        foreach (GameObject go in objectsInSight)
        {
            if (go != null) { continue; }
            GameObject sliced = Slice(go, false);
            Destroy(go);
        }

        if(prefabPicture != null && prefabPicture.transform.childCount > 0)
        {
            GameObject instanciate = Instantiate(prefabPicture, prefabPicture.transform.position, prefabPicture.transform.rotation);
            if (instanciate == null) return;
            for(int i =0; i<instanciate.transform.childCount; i++)
            {
                GameObject child = instanciate.transform.GetChild(i).gameObject;
                child.tag = "Untagged";
                child.SetActive(true);
                child.AddComponent<MeshCollider>();
                child.GetComponent<MeshRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                    
            }

        }
        Destroy(prefabPicture);
        photo.SetActive(false);
        hasPicture = false;
        allObjects = UnityEngine.Object.FindObjectsOfType<GameObject>();
        return;

    }


    private void TakePicture()
    {
        prefabPicture = new GameObject("PrefabPicture");
        prefabPicture.transform.parent = Camera.main.gameObject.transform;



        //카메라뷰 복사
        RenderTexture copytexture;
        copytexture = new RenderTexture(4096, 4096, 0);
        copytexture.enableRandomWrite = true;
        RenderTexture.active = copytexture;
        Graphics.Blit(cameraView, copytexture);
       


        photo.SetActive(true);
        photo.GetComponent<Image>().material.mainTexture = copytexture;


        if (prefabPicture == null) return;

        objectsInSight = CollisionHandler.collidObject;


        foreach(GameObject go in objectsInSight)
        {
            if(go == null) continue;

            GameObject newGo;
            GameObject sliced = Slice(go, true);

            if(sliced == null) {

                newGo = GameObject.Instantiate(go, go.transform.position, go.transform.rotation, prefabPicture.transform);

            }
            else
            {
                newGo = sliced;
                Destroy(sliced);
                newGo = GameObject.Instantiate(newGo, newGo.transform.position, newGo.transform.rotation, prefabPicture.transform);
            }
            newGo.transform.position = prefabPicture.transform.TransformPoint(go.transform.position);
            newGo.SetActive(false);
            newGo.tag = "IgnorePicture";
            newGo.GetComponent<MeshFilter>().mesh.uv = go.GetComponent<MeshFilter>().mesh.uv;
        }
        PrefabUtility.SaveAsPrefabAsset(prefabPicture, "Assets/Prefabs/picturePrefab.prefab", out bool success);

        hasPicture = true;

        StartCoroutine("MovePictureTowardPosition", HoldingPosPicture.GetComponent<RectTransform>().position);
        StartCoroutine("ShrinkOrGrowPicture", true);


    }
   

    public void LookPicture()
    {
        StopCoroutine("MovePictureTowardPosition");
        StopCoroutine("ShrinkOrGrowPicture");
        StartCoroutine("MovePictureTowardPosition", TakingPosPicture.GetComponent<RectTransform>().position);
        StartCoroutine("ShrinkOrGrowPicture", false);
    }
    public void HoldPicture()
    {
        StopCoroutine("MovePictureTowardPosition");
        StopCoroutine("ShrinkOrGrowPicture");
        StartCoroutine("MovePictureTowardPosition", HoldingPosPicture.GetComponent<RectTransform>().position);
        StartCoroutine("ShrinkOrGrowPicture", true);
    }

    private void ActivateCamera(bool activated)
    {
        viewFinder.SetActive(activated);
    }

    private IEnumerator MovePictureTowardPosition(Vector3 position)
    {
        Debug.Log(position + " ?????");
    
        Transform photo = this.photo.transform.parent;
        float speed = 1000f;
        Vector3 picturePos = photo.position;
        while (Vector3.Distance(picturePos, position) > 0f)
        {
            picturePos = photo.position;
            photo.position = Vector3.MoveTowards(picturePos, new Vector3(position.x, position.y, picturePos.z), Time.deltaTime * speed);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }
    private IEnumerator ShrinkOrGrowPicture(bool shrink)
    {
        Vector3 scaleChange = new Vector3(0.1f, 0.1f, 0f);
        float multiplier = 0.05f;
        Vector3 picturePos = photo.transform.position;
        if (shrink)
        {
            while (photo.transform.localScale.x > 0.5f)
            {
                photo.transform.localScale -= scaleChange * multiplier;
                yield return new WaitForEndOfFrame();
            }
        }
        else
        {
            while (photo.transform.localScale.x < 1f)
            {
                photo.transform.localScale += scaleChange * multiplier;
                yield return new WaitForEndOfFrame();
            }
        }

        yield return null;
    }

    private void DrawFrustum(Camera cam)
    {
        Vector3[] nearCorners = new Vector3[4];
        Vector3[] farCorners = new Vector3[4];
        UnityEngine.Plane[] camPlanes = GeometryUtility.CalculateFrustumPlanes(cam); // get plnaes from matrix

        UnityEngine.Plane temp = camPlanes[1]; camPlanes[1] = camPlanes[2]; camPlanes[2] = temp;
        for(int i =0; i<4; i++)
        {
            nearCorners[i] = Plane3Intersect(camPlanes[4], camPlanes[i], camPlanes[(i + 1) % 4]); //near corners on the created projection matrix
            farCorners[i] = Plane3Intersect(camPlanes[5], camPlanes[i], camPlanes[(i + 1) % 4]); //far corners on the created projection matrix
        }

        currFarCorners = farCorners;
        currNearCorners = nearCorners;

    }

    private Vector3 Plane3Intersect(UnityEngine.Plane p1, UnityEngine.Plane p2, UnityEngine.Plane p3)
    {
        return ((-p1.distance * Vector3.Cross(p2.normal, p3.normal)) +
                (-p2.distance * Vector3.Cross(p3.normal, p1.normal)) +
                (-p3.distance * Vector3.Cross(p1.normal, p2.normal)))/ 
                (Vector3.Dot(p1.normal, Vector3.Cross(p2.normal, p3.normal)));
    }

    private GameObject Slice(GameObject target, bool getUpper)
    {
        bool hasSliced = false;
        GameObject currTarget = null;

        GameObject upperHull = null;
        GameObject lowerHull = null;

        for(int i =0; i<currNearCorners.Length; i++)
        {
            if (hasSliced)
            {
                target = currTarget;
                Destroy(currTarget);
            }
            Material sliceMat = target.GetComponent<Renderer>().material;
            Vector3 norm = i + 1 >= currNearCorners.Length ?
                (currFarCorners[i] + currFarCorners[0]) / 2.0f : (currFarCorners[i] + currFarCorners[i + 1]) / 2.0f;
            Quaternion direction = Quaternion.LookRotation(norm);

            if (i == 1 || i == 3)
                direction *= Quaternion.Euler(0, 0, 90);
            if (i == 3 || i == 2)
                direction *= Quaternion.Euler(0, 0, 180);
            SlicedHull hull = target.Slice(transform.position, direction * Vector3.up);
            
            if (hull != null)
            {
                if (getUpper)
                {
                    upperHull = hull.CreateUpperHull(target, sliceMat);
                    currTarget = upperHull;
                    
                }
                else
                {
                    lowerHull = hull.CreateLowerHull(target, sliceMat);
                    currTarget = target;
                    lowerHull.AddComponent<MeshCollider>();
                }
                hasSliced = true;
            }
            else
            {
                Debug.Log("hull is null");
            }

        }


        return currTarget;
    }

}
