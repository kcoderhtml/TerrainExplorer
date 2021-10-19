using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObjectFree : MonoBehaviour {

    public float time = 10;

	void Start () {
        Destroy(this.gameObject, time);
	}
}
