using UnityEngine;

public class CameraControl : MonoBehaviour
{
    private float startPosX;
    [SerializeField] private float startPosY;
    private float startPosZ;

    [SerializeField] private Vector3 followOffset;

    public bool isFollowing = false;
    public bool isReset = false;



    private void Update()
    {
        if (isFollowing && GameManager.instance.player != null)
        {
            FollowTarget(GameManager.instance.player.transform);
        }

        if (isReset)
        {

        }
    }

    public void SetCameraStart(int maxRows, int maxCols)
    {
        startPosX = maxCols * 0.5f;
        startPosZ = maxRows;

        Vector3 cameraPos = new Vector3(startPosX, startPosY, -startPosZ);
        transform.position = cameraPos;
    }

    public void FollowTarget(Transform target)
    {
        Vector3 targetPos = target.position;

        targetPos.x += followOffset.x;
        targetPos.y += followOffset.y;
        targetPos.z += followOffset.z;

        transform.position = Vector3.Lerp(transform.position, targetPos, 0.1f);
    }

    public void GoToStart()
    {

    }
}
