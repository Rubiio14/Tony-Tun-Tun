using System.Collections;
using UnityEngine;

public class skullbunnycesta : MonoBehaviour
{
    public GameObject Skullbunny;
    public GameObject Waypoint;
    public float duration;

    private void Start()
    {
        StartCoroutine(MoveTowards(Skullbunny.transform, Waypoint.transform.position, duration));
    }

    private IEnumerator MoveTowards(Transform objectToMove, Vector3 toPosition, float duration)
    {
        float counter = 0;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            Vector3 currentPos = objectToMove.position;

            float time = Vector3.Distance(currentPos, toPosition) / (duration - counter) * Time.deltaTime;

            objectToMove.position = Vector3.MoveTowards(currentPos, toPosition, time);

            yield return null;
        }
        Destroy(gameObject);
    }

}


