using UnityEngine;

public class SuperliminalScale : MonoBehaviour
{
    // 시작 스케일 범위
    public Vector3 startScale = new Vector3(1f, 1f, 1f);

    // 최대 스케일 범위
    public Vector3 maxScale = new Vector3(2f, 2f, 2f);

    // 최소 스케일 범위
    public Vector3 minScale = new Vector3(0.5f, 0.5f, 0.5f);

    bool shrinking;

    void Update()
    {
        
        Vector3 position = transform.position;

        //오브젝트의 최대 크기 설정 및 크기 증가 연산
        if (position.y >= 4f)
        {
           
            transform.localScale = Vector3.Lerp(transform.localScale, maxScale, Time.deltaTime);
        }
        else
        {

            transform.localScale = Vector3.Lerp(transform.localScale, startScale, Time.deltaTime);
        }

        if (Input.GetMouseButton(1))
        {
            shrinking = true;
        }
        else
        {
            shrinking = false;
        }

        if (shrinking)
        {
            // 스케일을 감소 연산
            transform.localScale = Vector3.Lerp(transform.localScale, minScale, Time.deltaTime);
        }
    }
}