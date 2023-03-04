using UnityEngine;

public class CameraController : MonoBehaviour
{
	Transform target;

	float smoothSpeed = 0.2f;
	Vector3 offset = new Vector3(0, 0, -45);
	Vector3 velocity = Vector3.zero;

	void Start()
	{
		target = GameObject.FindGameObjectsWithTag("Player")[0].transform;
		transform.position = target.position + offset;
	}

	void Update()
	{
		Vector3 desiredPosition = target.position + offset;
		Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothSpeed);
		transform.position = smoothedPosition;
	}

}