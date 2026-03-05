using System.Collections;
using UnityEngine;

public class StickyBomb : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(DeleteObj());
    }

    IEnumerator DeleteObj()
    {
        yield return new WaitForSeconds(1f);

        Destroy(gameObject);
    }
}
