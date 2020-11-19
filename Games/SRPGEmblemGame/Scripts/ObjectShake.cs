using UnityEngine;
using System.Collections;

/// http://www.mikedoesweb.com/2012/camera-shake-in-unity/

public class ObjectShake : MonoBehaviour {

	private Vector3 originPosition;
	private Quaternion originRotation;
	public float shakeDecay = 0.2f;
	public float shakeIntensity = .2f;

	private float curShakeIntensity = 0;

	private bool isShaking = false;
	
	// void OnGUI (){
	// 	if (GUI.Button (new Rect (20,40,80,20), "Shake")){
	// 		Shake ();
	// 	}
	// }
	
	void Update (){
		if (curShakeIntensity > 0){
			isShaking = true;
			transform.position = originPosition + Random.insideUnitSphere * curShakeIntensity;
			transform.rotation = new Quaternion(
				originRotation.x + Random.Range (-curShakeIntensity,curShakeIntensity) * .2f,
				originRotation.y + Random.Range (-curShakeIntensity,curShakeIntensity) * .2f,
				originRotation.z + Random.Range (-curShakeIntensity,curShakeIntensity) * .2f,
				originRotation.w + Random.Range (-curShakeIntensity,curShakeIntensity) * .2f);
			curShakeIntensity -= shakeDecay * Time.deltaTime;
		} else if(isShaking) {
			isShaking = false;
			// GetComponent<Collider2D>().enabled = true;
		}
	}
	
	public void Shake(){
		originPosition = transform.position;
		originRotation = transform.rotation;
		curShakeIntensity = shakeIntensity;
	}
}