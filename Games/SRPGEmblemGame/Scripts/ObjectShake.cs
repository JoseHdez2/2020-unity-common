using UnityEngine;
using System.Collections;

/// http://www.mikedoesweb.com/2012/camera-shake-in-unity/

public class ObjectShake : MonoBehaviour {

	private Vector3 originPosition;
	private Quaternion originRotation;
	public float shakeDecay = 0.5f;
	public float shakeIntensity = .3f;

	public float tempShakeIntensity = 0;

	private bool isShaking = false;
	
	// void OnGUI (){
	// 	if (GUI.Button (new Rect (20,40,80,20), "Shake")){
	// 		Shake ();
	// 	}
	// }
	
	void Update (){
		if (tempShakeIntensity > 0){
			isShaking = true;
			transform.position = originPosition + Random.insideUnitSphere * tempShakeIntensity;
			transform.rotation = new Quaternion(
				originRotation.x + Random.Range (-tempShakeIntensity,tempShakeIntensity) * .2f,
				originRotation.y + Random.Range (-tempShakeIntensity,tempShakeIntensity) * .2f,
				originRotation.z + Random.Range (-tempShakeIntensity,tempShakeIntensity) * .2f,
				originRotation.w + Random.Range (-tempShakeIntensity,tempShakeIntensity) * .2f);
			tempShakeIntensity -= shakeDecay * Time.deltaTime;
		} else if(isShaking) {
			isShaking = false;
			// GetComponent<Collider2D>().enabled = true;
		}
	}
	
	public void Shake(){
		originPosition = transform.position;
		originRotation = transform.rotation;
		tempShakeIntensity = shakeIntensity;
	}
}