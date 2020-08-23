using ExtensionMethods;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public abstract class DunCraMovement : Movable {

    public GameObject moveTargetPrefab;
    public GameObject rotateTargetPrefab;

    public bool strafes = true;
    private bool isMoving = false;

    protected Transform rotationTarget; // TODO delete this, maybe.
    protected DunCraLevelInterpreter levelInterpreter;

    protected AudioSourceReDungPlayer audioSource;
    protected ReDungInventory playerInventory;

    private List<Collider> wallColliders;
    private List<Collider> doorColliders;
    private RotateTowardsTarget rotateTowardsTarget;

    protected void Awake()
    {
        audioSource = GetComponentInChildren<AudioSourceReDungPlayer>();
    }

    protected void Start()
    {
        // SetPlayerHp(playerHP); // draw player hp on-screen.
        InitPlayer();
    }

    private void InitPlayer()
    {
        playerInventory = GetComponentInChildren<ReDungInventory>();

        movementTarget = Instantiate(moveTargetPrefab, transform.position, Quaternion.identity).transform;
        rotationTarget = Instantiate(rotateTargetPrefab, transform.position + transform.forward, Quaternion.identity).transform;
        levelInterpreter = FindObjectOfType<DunCraLevelInterpreter>();

        rotateTowardsTarget = GetComponent<RotateTowardsTarget>();
        rotateTowardsTarget.target = rotationTarget;
        InitPlayerPos();
    }

    private void InitPlayerPos()
    {
        doorColliders = null;
        wallColliders = null;
        movementTarget.position = transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        rotationTarget.position = transform.position + transform.forward;
    }

    protected IEnumerator MoveTowards(Vector3 direction)
    {
        if (isMoving) { yield break; }
        if (strafes) { rotateTowardsTarget.enabled = false; }
        Vector3 newPos = transform.position + direction;
        Collider door = GetDoor(newPos);
        isMoving = true;
        if (IsInsideAWall(newPos)) {
            Debug.Log("BONK");
            PlaySound(EReDungPlayerSound.BONK);
            isMoving = false;
        } else if (door != null) {
            if (!playerInventory.HasAny(EReDungItem.KEY)) {
                Debug.Log("BONK (DOOR)");
                PlaySound(EReDungPlayerSound.LOCKED);
                isMoving = false;
            } else {
                Destroy(door.gameObject);
                playerInventory.AddItem(EReDungItem.KEY, -1);
                PlaySound(EReDungPlayerSound.UNLOCK);
                doorColliders = null;
            }
        }
        if (isMoving) {
            movementTarget.position = newPos;
            yield return new WaitUntil(HasArrived);
            movementTarget.SnapToGrid(); // TODO messy, instead find root of problem.
            transform.SnapToGrid();
            rotationTarget.position = transform.position + transform.forward;
            PlaySound(EReDungPlayerSound.STEP);
            OnMoveStep();
        }
        if (strafes) { rotateTowardsTarget.enabled = true; }
        isMoving = false;
    }

    protected void PlaySound(EReDungPlayerSound sound){
        if (audioSource) { audioSource.PlaySound(sound); }
    }

    protected abstract void OnMoveStep();

    protected IEnumerator Turn(int angle)
    {
        if (isMoving) { yield break; }
        isMoving = true;
        rotationTarget.RotateAround(transform.position, Vector3.up, angle);
        yield return new WaitForSeconds(0.5f);
        isMoving = false;
    }

    private List<Collider> GetDoorColliders()
    {
        if (doorColliders != null) { return doorColliders; }
        doorColliders = FindObjectsOfType<Collider>().Where(c => c.gameObject.CompareTag("Door")).ToList();
        return doorColliders;
    }

    private List<Collider> GetWallColliders()
    {
        if (wallColliders != null) { return wallColliders; }
        wallColliders = FindObjectsOfType<Collider>().Where(c => c.gameObject.CompareTag("Wall")).ToList();
        return wallColliders;
    }

    private bool IsInsideAWall(Vector3 position)
    {
        return GetWallColliders().Any(coll => coll.bounds.Contains(position));
    }

    private Collider GetDoor(Vector3 position)
    {
        return GetDoorColliders().Find(coll => coll.bounds.Contains(position));
    }
}