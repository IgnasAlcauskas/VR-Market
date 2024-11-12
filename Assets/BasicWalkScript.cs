using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWalkScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetLocalPositionAndRotation(transform.localPosition + new Vector3(speed, 0, 0) * Time.deltaTime, transform.rotation);
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("AAA");
        if (other.gameObject.CompareTag("DeleteNpc"))
        {
            Destroy(gameObject);
        }
    }
}
