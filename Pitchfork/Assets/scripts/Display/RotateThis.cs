using UnityEngine;
using System.Collections;

public class RotateThis : MonoBehaviour {

    public float Speed;

	void Update () {
        this.transform.Rotate(Vector3.forward * (Speed * Time.deltaTime));
    }
}
