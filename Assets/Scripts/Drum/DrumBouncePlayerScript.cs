using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumBouncePlayerScript : MonoBehaviour
{

    [Header("Drum bounce momentum")]
    [SerializeField]
    private float totalMomentumTime = 0f;
    [SerializeField]
    private float momentumPower = 0f;
    [SerializeField]
    public float bounceVelOffset = 0f;
    [SerializeField]
    private float playerMomentumWeight = 60f;

    [Header("Bounce status")]
    [SerializeField]
    public BounceStatus bounceStatus = BounceStatus.NORMAL;

    private Rigidbody2D rigidbody2d;

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] allDrums = GameObject.FindGameObjectsWithTag("Drum");
        foreach (GameObject drum in allDrums)
        {
            drum.GetComponent<DrumBounceScript>().bounceEvent.AddListener(Bounce);
        }

        rigidbody2d = GetComponent<Rigidbody2D>();
    }

    private void OnDisable()
    {
        GameObject[] allDrums = GameObject.FindGameObjectsWithTag("Drum");
        foreach (GameObject drum in allDrums)
        {
            drum.GetComponent<DrumBounceScript>().bounceEvent.RemoveListener(Bounce);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    /**
     * Handles a drum bounce. The player will bounce a certain amount from the 
     * drum going a certain way.
     */
    public void Bounce(float rotationDegrees, float bouncePower)
    {
        Debug.Log("Bounce event called with: " + rotationDegrees + "value.");

        Vector2 bounceImpulse = new Vector2(bouncePower * -Mathf.Sin(rotationDegrees), bouncePower * Mathf.Cos(rotationDegrees));

        //Debug.Log("Bounce Impulse: " + bounceImpulse.x + ", " + bounceImpulse.y);

        Vector2 playerMomentum = rigidbody2d.velocity;
        Debug.Log("Player momentum: " + rigidbody2d.velocity.x + ", " + rigidbody2d.velocity.y);

        Vector2 bounceImpulseWeighted = bounceImpulse - playerMomentum * playerMomentumWeight;

        Debug.Log("Impulse: " + bounceImpulseWeighted.x + ", " + bounceImpulseWeighted.y);

        rigidbody2d.AddForce(bounceImpulseWeighted);

        StartCoroutine(BounceTime());

    }

    IEnumerator BounceTime()
    {
        bounceStatus = BounceStatus.BOUNCE;

        yield return new WaitForSeconds(0.15f);

        //rigidbody2d.velocity = new Vector2(rigidbody2d.velocity.x, 0f);

        Vector2 carryOverMomentum = rigidbody2d.velocity;

        bounceStatus = BounceStatus.BACK_TO_NORMAL;

        //ADDS EXTRA ARTIFICIAL HORIZONTAL MOMENTUM AFTER THE BOUNCE
        float startTime = Time.time;
        while (Time.time - startTime < totalMomentumTime)
        {
            float forceMultiplier = 1f - ((Time.time - startTime) / totalMomentumTime);

            bounceVelOffset = carryOverMomentum.x * forceMultiplier * momentumPower;

            yield return null;
        }

        bounceStatus = BounceStatus.NORMAL;

        /*

        bounceStatus = BounceStatus.BACK_TO_NORMAL;
        isBounceTime = false;

        currentSpeed = rigidbody2d.velocity.x;

        yield return new WaitForSeconds(0.05f);

        bounceStatus = BounceStatus.NORMAL;*/
    }
}

public enum BounceStatus
{
    NORMAL,
    BOUNCE,
    BACK_TO_NORMAL
}
