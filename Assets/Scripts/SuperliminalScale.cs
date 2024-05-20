using UnityEngine;

public class SuperliminalScale : MonoBehaviour
{
    // ���� ������ ����
    public Vector3 startScale = new Vector3(1f, 1f, 1f);

    // �ִ� ������ ����
    public Vector3 maxScale = new Vector3(2f, 2f, 2f);

    // �ּ� ������ ����
    public Vector3 minScale = new Vector3(0.5f, 0.5f, 0.5f);

    bool shrinking;

    void Update()
    {
        
        Vector3 position = transform.position;

        //������Ʈ�� �ִ� ũ�� ���� �� ũ�� ���� ����
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
            // �������� ���� ����
            transform.localScale = Vector3.Lerp(transform.localScale, minScale, Time.deltaTime);
        }
    }
}