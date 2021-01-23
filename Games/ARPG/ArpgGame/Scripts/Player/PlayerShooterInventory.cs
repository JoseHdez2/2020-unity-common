using UnityEngine;
using System.Collections;
using System.Linq;

[System.Serializable]
public class AmmoTypePrefab
{
    public EArpgGameItemType bulletType;
    public PlayerShooterBullet bulletPrefab;
    public AudioClip shotSound;
}

public class PlayerShooterInventory : AbstractPlayerShooter
{
    public EArpgGameItemType selectedBulletType;
    public AmmoTypePrefab[] ammoTypePrefabs;

    private ArpgGamePlayerInventory inventory;
    protected override void Awake()
    {
        base.Awake();
        inventory = FindObjectOfType<ArpgGamePlayerInventory>();
    }

    public override bool HasBulletsLeft()
    {
        return inventory.HasAnyOfType(selectedBulletType);
    }
    
    public override void SpendBullet()
    {
        inventory.AddItem(selectedBulletType, -1);
    }

    public override PlayerShooterBullet GetBulletPrefab()
    {
        return ammoTypePrefabs.First(e => e.bulletType == selectedBulletType).bulletPrefab;
    }

    public override AudioClip GetShotSound()
    {
        return ammoTypePrefabs.First(e => e.bulletType == selectedBulletType).shotSound;
    }

    // TODO improve efficiency with ChangeBulletType method and new private variables.
}
