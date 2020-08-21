using ExtensionMethods;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(RotateTowardsTarget))]
public class DungeonCrawlerPlayer : Movable
{
    public bool strafes = true;
    public GameObject moveTargetPrefab;
    public GameObject rotateTargetPrefab;
    public InputMaster controls;
    public TMP_Text textPlayerHP;

    private static int playerHP = 10;
    public static int playerMaxHP = 10;
    private Transform rotationTarget; // TODO delete this, maybe.
    private DunCraLevelInterpreter levelInterpreter;
    private bool isMoving = false;
    private RotateTowardsTarget rotateTowardsTarget;

    private List<Collider> wallColliders;
    private List<Collider> doorColliders;
    private ReDungInventory playerInventory;
    private AudioSourceReDungPlayer audioSource;
    private ImageWipe screenWipe;

    private void Awake()
    {
        audioSource = GetComponentInChildren<AudioSourceReDungPlayer>();
        textPlayerHP = FindObjectOfType<ReDungHealthText>().GetComponent<TMP_Text>();
        controls = InputMasterSingleton.Get();
        controls.DungeonCrawlerPlayer.moveBackward.performed += ctx => StartCoroutine(MoveTowards(-transform.forward));
        controls.DungeonCrawlerPlayer.moveForward.performed += ctx => StartCoroutine(MoveTowards(transform.forward));
        controls.DungeonCrawlerPlayer.strafeLeft.performed += ctx => StartCoroutine(MoveTowards(-transform.right));
        controls.DungeonCrawlerPlayer.strafeRight.performed += ctx => StartCoroutine(MoveTowards(transform.right));
        controls.DungeonCrawlerPlayer.turnLeft.performed += ctx => StartCoroutine(Turn(-90));
        controls.DungeonCrawlerPlayer.turnRight.performed += ctx => StartCoroutine(Turn(90));
        controls.DungeonCrawlerPlayer.Enable();
    }

    public static void RestoreHealth(){
        playerHP = playerMaxHP;
    }

    private void OnEnable() {
        controls.DungeonCrawlerPlayer.Enable();
    }

    private void OnDisable() {
        controls.DungeonCrawlerPlayer.Disable();
    }
    
    private void Start() {
        SetPlayerHp(playerHP); // draw player hp on-screen.
        InitPlayer();
    }

    private void InitPlayer() {
        playerInventory = GetComponentInChildren<ReDungInventory>();

        movementTarget = Instantiate(moveTargetPrefab, transform.position, Quaternion.identity).transform;
        rotationTarget = Instantiate(rotateTargetPrefab, transform.position + transform.forward, Quaternion.identity).transform;
        levelInterpreter = FindObjectOfType<DunCraLevelInterpreter>();

        rotateTowardsTarget = GetComponent<RotateTowardsTarget>();
        rotateTowardsTarget.target = rotationTarget;
        InitPlayerPos();
    }

    private List<Collider> GetDoorColliders() {
        if (doorColliders != null) { return doorColliders; }
        doorColliders = FindObjectsOfType<Collider>().Where(c => c.gameObject.CompareTag("Door")).ToList();
        return doorColliders;
    }

    private List<Collider> GetWallColliders() {
        if (wallColliders != null) { return wallColliders; }
        wallColliders = FindObjectsOfType<Collider>().Where(c => c.gameObject.CompareTag("Wall")).ToList();
        return wallColliders;
    }

    private IEnumerator MoveTowards(Vector3 direction) {
        if (isMoving) { yield break; }
        if (strafes) { rotateTowardsTarget.enabled = false; }
        Vector3 newPos = transform.position + direction;
        Collider door = GetDoor(newPos);
        isMoving = true;
        if (IsInsideAWall(newPos)) {
            Debug.Log("BONK");
            audioSource.PlaySound(EReDungPlayerSound.BONK);
            isMoving = false;
        } else if (door != null) {
            if (!playerInventory.HasAny(EReDungItem.KEY)) {
                Debug.Log("BONK (DOOR)");
                audioSource.PlaySound(EReDungPlayerSound.LOCKED);
                isMoving = false;
            } else {
                Destroy(door.gameObject);
                playerInventory.AddItem(EReDungItem.KEY, -1);
                audioSource.PlaySound(EReDungPlayerSound.UNLOCK);
                doorColliders = null;
            }
        }
        if (isMoving) {
            movementTarget.position = newPos;
            yield return new WaitUntil(HasArrived);
            movementTarget.SnapToGrid(); // TODO messy, instead find root of problem.
            transform.SnapToGrid();
            rotationTarget.position = transform.position + transform.forward;
            audioSource.PlaySound(EReDungPlayerSound.STEP);
            SetPlayerHp(playerHP - 1);
        }
        if (strafes) { rotateTowardsTarget.enabled = true; }
        isMoving = false;
    }

    private void SetPlayerHp(int qty) {
        playerHP = qty;
        if (textPlayerHP) { FindObjectOfType<ReDungHealthText>().UpdateText(playerHP); };
        if (playerHP <= 0) { Die(); }
    }

    private bool IsInsideAWall(Vector3 position) {
        return GetWallColliders().Any(coll => coll.bounds.Contains(position));
    }

    private Collider GetDoor(Vector3 position) {
        return GetDoorColliders().Find(coll => coll.bounds.Contains(position));
    }

    private IEnumerator Turn(int angle) {
        if (isMoving) { yield break; }
        isMoving = true;
        rotationTarget.RotateAround(transform.position, Vector3.up, angle);
        yield return new WaitForSeconds(0.5f);
        isMoving = false;
    }

    public void Die() {
        PlayerInventory.playerInventory.Clear(); // lose all items on death.
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine() {
        GetComponentInChildren<Collider>().enabled = false;
        DeathAnimation();
        controls.Disable();
        yield return new WaitForSeconds(2f);
        screenWipe = FindObjectOfType<ImageWipe>();
        audioSource.PlaySound(EReDungPlayerSound.TIMEWARP1);
        screenWipe.ToggleWipe(true);
        yield return new WaitUntil(() => screenWipe.isDone);

        RestoreHealth();
        levelInterpreter.curLevelIndex = 0;
        levelInterpreter.InitLevel();

        audioSource.PlaySound(EReDungPlayerSound.TIMEWARP2);
        FindObjectOfType<ImageWipe>().ToggleWipe(false);
        yield return new WaitUntil(() => screenWipe.isDone);
        GetComponentInChildren<Collider>().enabled = true;
        controls.Enable();
    }

    private void DeathAnimation() {
        audioSource.PlaySound(EReDungPlayerSound.DIE);
        movementTarget.position = transform.position - Vector3.up * 0.5f;
        rotationTarget.position = transform.position + Vector3.up;
    }

    private void InitPlayerPos() {
        doorColliders = null;
        wallColliders = null;
        movementTarget.position = transform.position;
        transform.rotation = Quaternion.Euler(0, 0, 0);
        rotationTarget.position = transform.position + transform.forward;
    }
}