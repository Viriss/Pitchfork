using UnityEngine;
using System.Collections;

public class RotateCircle : MonoBehaviour {

    private int _rotateAngle = 60;
    private float _expandRate = 0.4f;
    private float _lifeTime = 0f;
    private float _maxLifeTime = 1.0f;
    private float _fadeTime = 0.8f;
    private SpriteRenderer _sprite;

    void Start()
    {
        _sprite = this.GetComponent<SpriteRenderer>();
    }

	// Update is called once per frame
	void Update () {
        _lifeTime += Time.deltaTime;
        
        this.transform.Rotate(new Vector3(0, 0, 90), _rotateAngle * Time.deltaTime);
        this.transform.localScale = new Vector3(this.transform.localScale.x + (_expandRate * Time.deltaTime), this.transform.localScale.y + (_expandRate * Time.deltaTime), this.transform.localScale.z);
        _sprite.color = new Color(_sprite.color.r, _sprite.color.g, _sprite.color.b, _sprite.color.a - (_fadeTime * Time.deltaTime));

        if (_lifeTime >= _maxLifeTime)
        {
            Destroy(this.gameObject);
        }
    }
}
