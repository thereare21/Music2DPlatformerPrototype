using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuitarLaunchPlayerScript : MonoBehaviour
{
    [Header("Launch time values")]
    [SerializeField]
    private float launchDelay;
    [SerializeField]
    private float noControlTime;
    [SerializeField]
    private float backToNormalTime;

    [Header("Launch power values")]
    [SerializeField]
    private float launchPower;

    [Header("Back to normal momentum")]
    public float launchVelOffset;
    [SerializeField]
    private float backToNormalMomentum;

    [Header("Launch status")]
    [SerializeField]
    public LaunchStatus launchStatus = LaunchStatus.NORMAL;

    private Rigidbody2D rb;
    

    // Start is called before the first frame update
    void Start()
    {
        GameObject[] allGuitars = GameObject.FindGameObjectsWithTag("Guitar");
        foreach (GameObject guitar in allGuitars)
        {
            guitar.GetComponentInChildren<GuitarLaunchScript>().guitarLaunchEvent.AddListener(Launch);
        }

        rb = GetComponent<Rigidbody2D>();
    }

    private void OnDisable()
    {
        GameObject[] allGuitars = GameObject.FindGameObjectsWithTag("Guitar");
        foreach (GameObject guitar in allGuitars)
        {
            guitar.GetComponentInChildren<GuitarLaunchScript>().guitarLaunchEvent.RemoveListener(Launch);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Launch(float guitarRotation)
    {
        Debug.Log("Launch invoked");
        StartCoroutine(LaunchTime(guitarRotation));
    }

    IEnumerator LaunchTime(float guitarRotation)
    {
        launchStatus = LaunchStatus.LOCK_IN;
        rb.velocity = new Vector2(0f, 0f);
        rb.gravityScale = 0f;
        yield return new WaitForSeconds(launchDelay);
        launchStatus = LaunchStatus.LAUNCH;

        rb.AddForce(new Vector2(launchPower * -Mathf.Sin(guitarRotation), launchPower * Mathf.Cos(guitarRotation)));



        yield return new WaitForSeconds(noControlTime);
        launchStatus = LaunchStatus.BACK_TO_NORMAL;

        Vector2 carryOverMomentum = rb.velocity;

        
        float startTime = Time.time;
        while (Time.time - startTime < backToNormalTime)
        {
            

            float forceMultiplier = 1f - ((Time.time - startTime) / backToNormalTime);

            launchVelOffset = carryOverMomentum.x * forceMultiplier * backToNormalMomentum;


            yield return null;
        }
        launchStatus = LaunchStatus.NORMAL;

    }

    
}

public enum LaunchStatus
{
    NORMAL,
    LOCK_IN,
    LAUNCH,
    BACK_TO_NORMAL
}