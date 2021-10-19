using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn : MonoBehaviour
{
    public GameObject myPrefab;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(spawnprefab());
    }
    IEnumerator spawnprefab()
    {
        while(true) {
            yield return new WaitForSeconds(7);
            Instantiate(myPrefab, new Vector3(0, 0, 0), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
