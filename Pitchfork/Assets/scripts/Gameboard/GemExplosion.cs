using UnityEngine;
using System.Collections;

public class GemExplosion : MonoBehaviour {

    public float _lifeTime = 0.3f;
    private float _time;
	
	// Update is called once per frame
	void Update () {
        _time += Time.deltaTime;
        if (_time > _lifeTime) { Destroy(this.gameObject); }
	}
}
