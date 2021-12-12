using UnityEngine;
using Cinemachine;

public enum DoorType
{
    IN, OUT
}
public class DoorS : MonoBehaviour
{
    public DoorType doorType;
    [SerializeField] Transform DoorOut;
    [SerializeField] CinemachineVirtualCamera vCam;

    public Vector2 GetDoorOutPosition()
    {
        return DoorOut.transform.position;
    }

    public CinemachineVirtualCamera GetVCam()
    {
        return vCam;
    }
}
