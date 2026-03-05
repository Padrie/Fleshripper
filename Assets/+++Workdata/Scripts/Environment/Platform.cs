using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public GameObject firstWaypoint;
    public GameObject secondWaypoint;

    public float speed = 1f;
    public float waitTime = 0.5f;
    public float timeToGetToNextPlatform;
    public float currentTime;

    [HideInInspector] public bool oneTime = false;
    bool toggle = true;
    Vector3 target;


    private void Start()
    {
        gameObject.transform.position = firstWaypoint.transform.position;
        target = firstWaypoint.transform.position;
        StartCoroutine(UpdateThing());
        //waa
    }

    private void OnValidate()
    {
        if (!oneTime)
        {
            firstWaypoint = new GameObject($"[{name}] First Waypoint {Random.Range(0, 1000)}");
            secondWaypoint = new GameObject($"[{name}] Second Waypoint {Random.Range(0, 1000)}");

            firstWaypoint.transform.position = transform.position;
            secondWaypoint.transform.position = transform.position;

            oneTime = true;
        }

        timeToGetToNextPlatform = Vector3.Distance(firstWaypoint.transform.position, secondWaypoint.transform.position) / speed;
    }


    private IEnumerator UpdateThing()
    {
        while (true)
        {
            if (Vector3.Distance(gameObject.transform.position, target) < 0.01f)
            {
                target = NextTarget();
                yield return new WaitForSeconds(waitTime);
                currentTime = 0;
            }

            currentTime += Time.deltaTime;
            gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, target, Time.deltaTime * speed);

            yield return null;
        }
    }

    Vector3 NextTarget()
    {
        toggle = !toggle;

        return toggle ? firstWaypoint.transform.position : secondWaypoint.transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(firstWaypoint.transform.position, .25f);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(secondWaypoint.transform.position, .25f);
        Gizmos.DrawLine(firstWaypoint.transform.position, secondWaypoint.transform.position);
    }
}
