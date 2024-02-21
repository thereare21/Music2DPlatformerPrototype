using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuitarLaunchScript : MonoBehaviour
{

    public GuitarLaunch guitarLaunchEvent;

    [Header("Lock-in delay values")]
    [SerializeField]
    private float lockInDelay = 0.1f;

    [Header("Launch recovery value (time until next launch available)")]
    [SerializeField]
    private float launchRecovery = 0.5f;
    [SerializeField]
    private bool launchAvailable = true;

    [Header("Guitar sound")]
    [SerializeField]
    private AudioSource guitarSound;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger entered");
        StartCoroutine(LockInWait());
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Trigger exited");
    }

    IEnumerator LockInWait()
    {
        if (launchAvailable)
        {
            Debug.Log(guitarLaunchEvent.GetPersistentEventCount());

            yield return new WaitForSeconds(lockInDelay);
            Debug.Log("locked in");

            StartCoroutine(PlaySound());

            guitarLaunchEvent.Invoke(gameObject.transform.rotation.z);
            launchAvailable = false;
            yield return new WaitForSeconds(launchRecovery);
            launchAvailable = true;
        }
    }

    IEnumerator PlaySound()
    {
        yield return new WaitForSeconds(0.2f);

        if (guitarSound != null && guitarSound.clip != null)
        {
            guitarSound.Play();

        }
        else
        {
            Debug.LogError("null audio source!");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        BoxCollider2D boxCollider = GetComponentInChildren<BoxCollider2D>();

        Matrix4x4 originalMatrix = Gizmos.matrix;
        Matrix4x4 rotationMatrix = Matrix4x4.TRS(transform.position, transform.rotation, Vector3.one);
        Gizmos.matrix = rotationMatrix;


        Gizmos.DrawWireCube(boxCollider.offset, boxCollider.size);

        Gizmos.matrix = originalMatrix;

    }
}

[System.Serializable]
public class GuitarLaunch : UnityEngine.Events.UnityEvent<float>
{
    
}


