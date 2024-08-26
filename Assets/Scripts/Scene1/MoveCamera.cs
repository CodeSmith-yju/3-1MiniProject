using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    public Transform target;

    [Header("Camera Speed")]
    [SerializeField] private float cameraSpeed;// Time.deltaTime* (cameraSpeed == 5)


    public Vector2 center;
    public Vector2 size;
    private float height;
    private float width;

    bool oneTime = true;

    private void Start()
    {
        height = Camera.main.orthographicSize;
        width = height * Screen.width / Screen.height;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, size);
    }

    private void LateUpdate()
    {
        if (GameUiMgr.single.nowPlayerPlace == PlaceState.Town)
        {
            if (oneTime)
            {
                oneTime = !oneTime;
                gameObject.SetActive(false);
                center = target.position;//얘는 추후 이동된에 수정해야함.
                transform.position = target.position;
                gameObject.SetActive(true);
                return;
            }
        }
        //transform.position = new Vector3(target.position.x, target.position.y, -10f);
        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * cameraSpeed);
        //transform.position = new Vector3(transform.position.x, transform.position.y, -10f); -> Line: 42

        float lx = size.x * 0.5f - width;
        float clampX = Mathf.Clamp(transform.position.x, -lx + center.x, lx + center.x);

        float ly = size.y * 0.5f - height;
        float clampY = Mathf.Clamp(transform.position.y, -ly + center.y, ly + center.y);

        transform.position = new Vector3(clampX, clampY, -10f);
    }
}
