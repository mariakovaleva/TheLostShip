using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector= new Vector3(10f, 10f, 10f);
    [SerializeField] float oscillationPeriod = 2f; // it takes 2 seconds to complete a full cycle

    Vector3 startingPos;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // protection from period equalling 0
        if (oscillationPeriod <= Mathf.Epsilon) { return; }
        
        //Time.time -- game time; automatically frame rate independent
        float cycles = Time.time / oscillationPeriod; // grows continually from 0

        const float tau = Mathf.PI * 2f; // about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau);

        //half the amplitude and shift it by 0.5, so that the object doesn't go the same distance down
       float  movementFactor = rawSinWave / 2f + 0.5f; // goes from 0 to 1 and back

        Vector3 offset = movementVector * movementFactor;
        transform.position = startingPos + offset;
    }
}
