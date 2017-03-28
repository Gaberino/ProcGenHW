using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathmaker : MonoBehaviour {


	private int counter = 0;
	private int pathmakerSupply = 0;
	public Transform[] floorPrefabs;
	public Transform pathmakerSpherePrefab;


	private float p_tile1Ratio;
	private float p_tile2Ratio;
	private float p_tile3Ratio;



	// Use this for initialization
	void Start () {
		p_tile1Ratio = Overlord.Instance.tile1Ratio / 100f;
		p_tile2Ratio = Overlord.Instance.tile2Ratio / 100f;
		p_tile3Ratio = Overlord.Instance.tile3Ratio / 100f;
		pathmakerSupply = Random.Range(5, Overlord.Instance.pathmakerMaxSuppply);
	}
	
	// Update is called once per frame
	public void DoYourJob () {
		if (counter < pathmakerSupply){
			float randomNum = Random.Range(0f, 1f);

			if (randomNum < 0.25f){ //rotate 90
				this.transform.Rotate(Vector3.up * 90);
			}
			else if (randomNum > 0.25f && randomNum < 0.5f){//rotate -90
				this.transform.Rotate(Vector3.up * -90);
			}
			else if (randomNum > 0.95f && randomNum < 0.99f){//spawn new pathfinder
				SpawnNew();
			}
			else if (randomNum > 0.99f){
				SpawnNew();
				SpawnNew();
				SpawnNew();
				Destroy(this.gameObject);
			}
			TrySpawnFloor();
			this.transform.Translate(Vector3.forward * 5);
		}
		else {
			Overlord.Instance.pathmakersInScene.Remove(this.transform);
			Destroy(this.gameObject);
		}
	}

	void SpawnNew(){
		Transform newPathfinder = Instantiate(pathmakerSpherePrefab, this.transform.position, Quaternion.identity);
		newPathfinder.parent = Overlord.Instance.transform; //necessary for the broadcast to reach it
		Overlord.Instance.pathmakersInScene.Add(newPathfinder);
	}

	void TrySpawnFloor(){
		Ray checkRay = new Ray(this.transform.position + Vector3.up, Vector3.down);
		if (Physics.Raycast(checkRay)){ //there is already a tile here, do not build. OPTIONAL CHANGE do not update counter. Add as pathfinder life insurance
			//increment counter if the pathfinders are uninsured
			if (!Overlord.Instance.pathmakersInsured){
				counter++;
			}
		}
		else {
			//build
			float randomNum = Random.Range(0.0f, (p_tile1Ratio + p_tile2Ratio + p_tile3Ratio));
			if (randomNum < p_tile1Ratio){
				Transform newFloorTile = Instantiate(floorPrefabs[0], this.transform.position, Quaternion.identity);
			}
			else if (randomNum > p_tile1Ratio && randomNum < p_tile2Ratio + p_tile1Ratio){
				Transform newFloorTile = Instantiate(floorPrefabs[1], this.transform.position, Quaternion.identity);
			}
			else if (randomNum > p_tile2Ratio + p_tile1Ratio && randomNum < p_tile1Ratio + p_tile2Ratio + p_tile3Ratio){
				Transform newFloorTile = Instantiate(floorPrefabs[2], this.transform.position, Quaternion.identity);
		}
			counter++;
			Overlord.globalTileCount++;
		}
	}
}
