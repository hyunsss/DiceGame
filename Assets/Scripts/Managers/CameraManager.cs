using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform target;
	public float lerpValue;

	private void Start() {
		target = GameObject.Find("Player").transform;
	}

	public void Update()
	{
		if (target == null) { return; }
		Vector3 nextPos = Vector2.Lerp(transform.position, target.position, lerpValue * Time.deltaTime);
		nextPos.z = -10;
		transform.position = nextPos;
	}

}
