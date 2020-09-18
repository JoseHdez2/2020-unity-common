using ExtensionMethods;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(RotateTowardsTarget))]
public class DungeonCrawlerPlayer : DunCraMovement
{
    public InputMaster controls;
    public TMP_Text textPlayerHP;

    private static int playerHP = 20;
    public static int playerMaxHP = 20;

    [SerializeField] private ImageWipe screenWipe;
    private DialogueManager dialogManager;

    private void Awake()
    {
        base.Awake();
        textPlayerHP = FindObjectOfType<ReDungHealthText>().GetComponent<TMP_Text>();
        dialogManager = FindObjectOfType<DialogueManager>();
        screenWipe = GameObject.FindGameObjectWithTag("ScreenWipe").GetComponent<ImageWipe>();
        screenWipe.ToggleWipe(false);
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
        base.Start();
        SetPlayerHp(playerHP); // draw player hp on-screen.
        // InitPlayer();
    }

    private void SetPlayerHp(int qty) {
        playerHP = qty;
        if (textPlayerHP) { FindObjectOfType<ReDungHealthText>().UpdateText(playerHP); };
        if (playerHP <= 0) { Die(); }
    }

    public void Die() {
        // PlayerInventory.playerInventory.Clear(); // lose all items on death.
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine() {
        GetComponentInChildren<Collider>().enabled = false;
        DeathAnimation();
        controls.Disable();
        yield return new WaitForSeconds(2f);
        PlaySound(EReDungPlayerSound.TIMEWARP1);
        screenWipe.ToggleWipe(true);
        yield return new WaitUntil(() => screenWipe.isDone());

        // dialogManager.WriteOneShot("You fainted!");
        screenWipe = GameObject.FindGameObjectWithTag("ScreenWipe").GetComponent<ImageWipe>();
        RestoreHealth();
        levelInterpreter.curLevelIndex = 0;
        levelInterpreter.InitLevel();

        PlaySound(EReDungPlayerSound.TIMEWARP2);
        FindObjectOfType<ImageWipe>().ToggleWipe(false);
        yield return new WaitUntil(() => screenWipe.isDone());
        GetComponentInChildren<Collider>().enabled = true;
        // controls.Enable();
    }

    private void DeathAnimation() {
        PlaySound(EReDungPlayerSound.DIE);
        movementTarget.position = transform.position - Vector3.up * 0.5f;
        rotationTarget.position = transform.position + Vector3.up;
    }

    protected override void OnMoveStep()
    {
        SetPlayerHp(playerHP - 1);
    }
}