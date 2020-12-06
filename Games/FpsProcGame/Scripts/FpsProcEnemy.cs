using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpsProcEnemy : EntityDamageable
{
    private PlayerMovement playerController;
    private LerpMovement lerpMovement;

    // Start is called before the first frame update
    new void Awake()
    {
        base.Awake();
        playerController = FindObjectOfType<PlayerMovement>();        
        lerpMovement = GetComponentInChildren<LerpMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        lerpMovement.destinationPos = playerController.transform.position;
    }
}
