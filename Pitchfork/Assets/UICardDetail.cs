using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UICardDetail : MonoBehaviour {

    public GameObject CardImage;
    public Button ButtonA;
    public Button ButtonB;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void DoShowWindow()
    {
        this.transform.gameObject.SetActive(true);
    }
    public void DoCloseWindow()
    {
        this.transform.gameObject.SetActive(false);
    }
}
