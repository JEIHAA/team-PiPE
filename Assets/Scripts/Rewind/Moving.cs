using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Jobs;

public class Moving : MonoBehaviour
{
    [SerializeField] private float speed = 5.0f;
    public bool isMoving = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
            transform.position += Vector3.forward * Time.deltaTime * speed;
    }

    public void ToggleBtn(bool _toggle)
    {
        isMoving = _toggle;
    }
}
