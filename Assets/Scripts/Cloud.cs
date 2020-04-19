using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloud : MonoBehaviour
{

    public Vector3 Direction = Vector3.zero;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Direction * Time.deltaTime);
        if(transform.position.x > 500)
        {
            Destroy(gameObject);
        }
    }
}
