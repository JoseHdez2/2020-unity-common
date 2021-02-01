using UnityEngine;
using System.Collections;

public class CameraZeldaLike : MonoBehaviour
{
    private BoxCollider2D boundary;
    [SerializeField] LerpMovement cam;
    public void Awake()
    {
        boundary = GetComponentInChildren<BoxCollider2D>();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Transform player = collision.gameObject.transform;
            Vector3 boundaryPos = boundary.gameObject.transform.position;

            EDirection exitDir;
            Vector2 playerOffset = player.position - boundaryPos;
            if(Mathf.Abs(playerOffset.x) >= boundary.bounds.size.x / 2)
            {
                exitDir = playerOffset.x < 0 ? EDirection.LEFT : EDirection.RIGHT;
            } else {
                exitDir = playerOffset.y < 0 ? EDirection.DOWN : EDirection.UP;
            }
            // TODO use ArpgWeaponThrust dirsToVects
            float newX = boundaryPos.x, newY = boundaryPos.y;
            if (exitDir == EDirection.LEFT || exitDir == EDirection.RIGHT) {
                newX += boundary.bounds.size.x * (exitDir == EDirection.LEFT ? -1 : 1);
            } else {
                newY += boundary.bounds.size.y * (exitDir == EDirection.DOWN ? -1 : 1);
            }
            boundary.gameObject.transform.position = new Vector2(newX, newY);
            cam.destinationPos = new Vector3(newX, newY, cam.transform.position.z);
        }
    }
}
