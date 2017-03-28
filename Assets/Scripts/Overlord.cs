using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Overlord : MonoBehaviour {


	public static Overlord Instance;

	public Transform pathmakerSpherePrefab;

	public List<Transform> pathmakersInScene;

	public float tile1Ratio = 50f;
	public float tile2Ratio = 25f;
	public float tile3Ratio = 25f;

	public int pathmakerMaxSuppply = 50;

	public bool pathmakersInsured = false;
	public static int globalTileCount = 0;
	public int globalTileLimit = 500;

	public float generationSpeed = 1; //how many times to update pathmakers per second
	private float timer = 0f;

	public bool generating = false;

	// Use this for initialization
	void Start () {
		Instance = this;
		pathmakersInScene = new List<Transform>();
		SpawnOriginal();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.R)){//run it
			if (!generating){
				if (GameObject.FindGameObjectWithTag("TileObject") != null){
					SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
				}
				else {
				StartCoroutine("StartGen", generationSpeed);
				}
			}
			else{
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			}
			CameraController.Instance.cameraState = 1;
		}
	}

	void SpawnOriginal(){
		Transform newPathfinder = Instantiate(pathmakerSpherePrefab, Vector3.zero, Quaternion.identity);
		newPathfinder.parent = this.transform; //necessary for the broadcast to reach it
		pathmakersInScene.Add(newPathfinder);
	}

	IEnumerator StartGen(float speedModifier){
		generating = true;
		while (GameObject.FindGameObjectWithTag("Pathmaker") != null && globalTileCount < globalTileLimit){

			BroadcastMessage("DoYourJob", SendMessageOptions.RequireReceiver);
			CameraController.Instance.ExpandView();
			yield return new WaitForSeconds(1 / generationSpeed);
		}
		CameraController.Instance.cameraState = 0;
		GameObject[] pathmakerObjectsArray = GameObject.FindGameObjectsWithTag("Pathmaker");
		for (int i = 0; i < pathmakerObjectsArray.Length; i++){
			Destroy(pathmakerObjectsArray[i]);
		}
		pathmakersInScene = new List<Transform>();
		generating = false;
	}

	void ClearAndReset(){
		GameObject[] tileObjectsArray = GameObject.FindGameObjectsWithTag("TileObject");
		GameObject[] pathmakerObjectsArray = GameObject.FindGameObjectsWithTag("Pathmaker");
		for (int i = 0; i < tileObjectsArray.Length; i++){
			Destroy(tileObjectsArray[i]);
		}
		for (int i = 0; i < pathmakerObjectsArray.Length; i++){
			Destroy(pathmakerObjectsArray[i]);
		}
		CameraController.Instance.cameraState = 0;
		CameraController.Instance.ResetSize();
		pathmakersInScene = new List<Transform>();
	}
}
