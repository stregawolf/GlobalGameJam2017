using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveMachine : MonoBehaviour {

	public GameObject wavePiece;
	public int numWaves;

	public float frequency = 1.0f;
	public float velocity = 1f;
	public float amplitude = 0.25f;
	public float offsetPerWave = 3;
	public float width = 1f;
	public float wavePosition = 0f;

	public float velocityControlSpeed = 3.0f;
	public float amplitudeControlSpeed = 5.0f;

	public bool usePhysics = false;

	GameObject[] wavePieces;

	// Use this for initialization
	void Start () {
		SpawnWaves();
	}

	void SpawnWaves()
	{
		wavePieces = new GameObject[numWaves];
		for (int i = 0; i < numWaves; i++) {
			wavePieces[i] = GameObject.Instantiate(wavePiece);
			wavePieces[i].transform.parent = this.transform;
			GameObject waveChild = wavePieces[i].transform.GetChild(0).gameObject;
			waveChild.transform.localScale = new Vector3(numWaves * width,1,width);
			waveChild.transform.localPosition = new Vector3(numWaves * width * 0.5f, 0, 0);
			waveChild.GetComponent<Rigidbody>().centerOfMass = -waveChild.transform.localPosition;
			wavePieces[i].transform.localPosition = new Vector3(-numWaves * 0.5f * width, 0, i*width - numWaves * 0.5f * width);
		}
	}
	
	// Update is called once per frame
	void Update () {
		wavePosition += Time.deltaTime * -velocity;
		for (int i = 0; i < numWaves; i++) {
			float angle = Mathf.Sin((wavePosition + offsetPerWave * i) * frequency) * Mathf.PI * amplitude;
			wavePieces[i].transform.localRotation = Quaternion.Euler(0, 0, angle);
		}

		if (Input.GetKey(KeyCode.LeftArrow)) {
			velocity -= Time.deltaTime * velocityControlSpeed;
		} else if (Input.GetKey(KeyCode.RightArrow)) {
			velocity += Time.deltaTime * velocityControlSpeed;
		}

		if (!swapping) {
			if (Input.GetKey(KeyCode.UpArrow)) {
				amplitude += Time.deltaTime * amplitudeControlSpeed;
			} else if (Input.GetKey(KeyCode.DownArrow)) {
				amplitude -= Time.deltaTime * amplitudeControlSpeed;
			}
		}

		if (Input.GetKeyDown(KeyCode.Space)) {
			StartCoroutine(SwapAmplitude());
		}

	}

	bool swapping = false;
	bool killSwap = false;
	float currentTargetAmplitude;

	IEnumerator SwapAmplitude()
	{
		float sourceAmplitude = amplitude;
		float targetAmplitude = amplitude * -1f;
		if (swapping) {
			killSwap = true;
			sourceAmplitude = amplitude;
			targetAmplitude = -currentTargetAmplitude;
			yield return new WaitForEndOfFrame();
		}
		currentTargetAmplitude = targetAmplitude;
		killSwap = false;
		swapping = true;
		float time = 0.2f;
		float timer = time;
		while (timer > 0.0f && !killSwap) {
			timer = Mathf.Max(0f, timer - Time.deltaTime);
			amplitude = Mathf.Lerp(sourceAmplitude, targetAmplitude, 1.0f - timer / time);
			yield return new WaitForEndOfFrame();
		}
		swapping = false;
		yield break;
	}
}
