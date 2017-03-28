using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	public static CameraController Instance;

	public float cameraSpeed = 5f;

	private Camera myCamera;
	private Vector3 originalAnchorPos;
	Transform AnchorTransform;

	private float startingCameraSize;
	public float cameraExpansionRate = 1; //how much to expand each call

	public int cameraState = 0; //0 is at first position, 1 is move while generating, 2 is manual control

	// Use this for initialization
	void Start () {
		Instance = this;
		myCamera = this.GetComponent<Camera>();
		AnchorTransform = this.transform.parent;
		originalAnchorPos = this.transform.position;
		startingCameraSize = this.GetComponent<Camera>().orthographicSize;
	}
	
	// Update is called once per frame
	void Update () {
		if (cameraState == 0){
			if (AnchorTransform.position != originalAnchorPos){
				AnchorTransform.position = Vector3.Lerp(AnchorTransform.position, originalAnchorPos, Time.deltaTime * cameraSpeed);
			}
		}
		else if (cameraState == 1){
			Vector3 pathmakerAveragePos = Vector3.zero;
			if (Overlord.Instance.pathmakersInScene.Count > 0){
				foreach (Transform pathmaker in Overlord.Instance.pathmakersInScene) {
					pathmakerAveragePos += pathmaker.position;
				}
			pathmakerAveragePos /= Overlord.Instance.pathmakersInScene.Count;
			}
			AnchorTransform.position = Vector3.Lerp(AnchorTransform.position, pathmakerAveragePos, cameraSpeed * Time.deltaTime);
		}
		else if (cameraState == 2){
			float horizontal = Input.GetAxis("Horizontal") * cameraSpeed * 10;
			float vertical = Input.GetAxis("Vertical") * cameraSpeed * 10;

			AnchorTransform.position += new Vector3 (horizontal * Time.deltaTime, 0, vertical * Time.deltaTime);
		}

		if (Input.GetAxis("Horizontal") > 0f || Input.GetAxis("Vertical") > 0f){
			cameraState = 2;
		}
		if (Input.GetKeyDown(KeyCode.Space)){
			cameraState = 1;
		}
	}

	public void ExpandView(){
		myCamera.orthographicSize += cameraExpansionRate;
	}

	public void ResetSize(){
		myCamera.orthographicSize = startingCameraSize;

	}
}
