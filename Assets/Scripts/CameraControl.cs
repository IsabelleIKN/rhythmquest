using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float startPosX;
    [SerializeField] private float startPosY;
    private float startPosZ;

    private Vector3 startPos;

    [SerializeField] private Vector3 followOffset;
    [SerializeField] private float moveSpeed = 1f;

    public bool isFollowing = false;
    public bool backToStart = false;

    private Transform player;

    public void SetPlayer(Transform target)
    {
        player = target;
    }

    private void Update()
    {
        if (isFollowing && player != null)
        {
            FollowPlayer(player);
        }

        if (backToStart)
        {
            MoveCamera(startPos);
            if(Vector3.Distance(transform.position, startPos) < 0.1f)
            {
                transform.position = startPos;
                backToStart = false;
            } 
        }
    }

    public void SetCameraStart(int maxRows, int maxCols)
    {
        startPosX = maxCols * 0.5f;
        startPosZ = maxRows;

        startPos = new Vector3(startPosX, startPosY, -startPosZ);
        transform.position = startPos;
    }

    private void FollowPlayer(Transform target)
    {
        Vector3 targetPos = target.position;

        targetPos.x += followOffset.x;
        targetPos.y += followOffset.y;
        targetPos.z += followOffset.z;

        MoveCamera(targetPos);
    }

    private void MoveCamera(Vector3 targetPos)
    {
        transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime);
    }
}
