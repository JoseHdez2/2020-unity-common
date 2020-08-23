using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

// Any thing that makes a sound/explosion and gets removed from the playing field.
// Items, bullets, monsters...
[RequireComponent(typeof(AudioSource))]
public class Expirable : MonoBehaviour
{
    public GameObject prefabEffectExpire;
    public AudioSource soundExpire;
    private List<SpriteRenderer> spriteRenderers = new List<SpriteRenderer>();

    private void Awake()
    {
        spriteRenderers.Add(GetComponent<SpriteRenderer>());
        spriteRenderers.AddRange(GetComponentsInChildren<SpriteRenderer>().ToList());
        soundExpire = GetComponent<AudioSource>();
    }

    public void Expire()
    {
        if (soundExpire != null) { soundExpire.Play(); }
        if (prefabEffectExpire) { Instantiate(prefabEffectExpire, gameObject.transform.position, gameObject.transform.rotation); }
        GetComponents<BoxCollider2D>().ToList().ForEach(it => it.enabled = false);
        spriteRenderers.Where(sr => sr != null).ToList().ForEach(sr => sr.enabled = false );
        StartCoroutine(DestroyCoroutine());
    }

    private IEnumerator DestroyCoroutine() {
        yield return new WaitWhile(() => soundExpire.isPlaying);
        Destroy(this.gameObject);
    }
}
