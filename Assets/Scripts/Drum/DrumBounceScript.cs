using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrumBounceScript : MonoBehaviour
{
    public DrumBounce bounceEvent;
    public float bouncePower;

    [Header("Debug values")]
    [SerializeField]
    private float timeUntilNextBounce = 0.2f;
    [SerializeField]
    private bool bounceAvailable = true;

    // Start is called before the first frame update
    void Start()
    {
        bounceAvailable = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /*
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //if that something that hit it is the player, invoke the bounce
        //bounceEvent.Invoke(gameObject.transform.rotation.z);
        Debug.Log("Collided!");

        Debug.Log(collision.gameObject.layer);

        if (collision.gameObject.layer == LayerMask.NameToLayer("DrumBounceZone"))
        {
            Debug.Log("drum bounce zone entered");
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("DrumSolidZone"))
        {
            Debug.Log("drum solid zone entered");
        }
    }*/

    public void HandleChildCollision(int layer)
    {
        //Debug.Log("Received collision from child with layer: " + layer);

        if (bounceAvailable)
        {
            if (layer == LayerMask.NameToLayer("DrumBounceZone"))
            {
                Debug.Log("Handle a bounce");
                float rotation = this.gameObject.transform.rotation.z;

                //do stuff with the rotation.
                bounceEvent.Invoke(rotation, bouncePower);

                bounceAvailable = false;
                StartCoroutine(DisableBounceTemporarily());

            }
            else if (layer == LayerMask.NameToLayer("DrumSolidZone"))
            {
                Debug.Log("Handle a solid");
            }
        }
        
    }

    IEnumerator DisableBounceTemporarily()
    {
        yield return new WaitForSeconds(timeUntilNextBounce);
        bounceAvailable = true;
    }
}

[System.Serializable]
public class DrumBounce : UnityEngine.Events.UnityEvent<float, float>
{

}
