using UnityEngine;
using System.Collections;
using System.Linq;

// Any thing that makes a sound/explosion and gets removed from the playing field.
// Items, bullets, monsters...
[RequireComponent(typeof(AudioSource))]
public class Expirable : MonoBehaviour
{
    public GameObject prefabEffectExpire;
    public AudioSource soundExpire;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        soundExpire = GetComponent<AudioSource>();
    }

    public void Expire()
    {
        if (soundExpire != null) { soundExpire.Play(); }
        if (prefabEffectExpire) { Instantiate(prefabEffectExpire, gameObject.transform.position, gameObject.transform.rotation); }
        GetComponents<BoxCollider2D>().ToList().ForEach(it => it.enabled = false);
        this.spriteRenderer.enabled = false;
        StartCoroutine(DestroyCoroutine());
    }

    private IEnumerator DestroyCoroutine() {
        yield return new WaitWhile(() => soundExpire.isPlaying);
        Destroy(this.gameObject);
    }
}
