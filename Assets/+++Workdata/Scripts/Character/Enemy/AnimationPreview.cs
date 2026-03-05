using System.Collections;
using UnityEngine;

public class AnimationPreview : MonoBehaviour
{
    [SerializeField] bool idle = true;
    [SerializeField] bool walk = false;
    [SerializeField] bool die = false;
    [SerializeField] bool attack = false;

    Animator animator;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (idle)
        {
            print("Idle");
            animator.SetBool("Idle", true);
        }
        if (walk)
        {
            print("Walk");
            animator.SetBool("Walk", true);
        }
        if (die)
        {
            print("Die");
            animator.SetBool("Die", true);
        }
        if (attack)
        {
            print("Attack");
            animator.SetBool("Attack", true);
        }

        StartCoroutine(First());
    }


    IEnumerator First()
    {
        float elapsed = -1f;
        float shakeDuration = 1;

        while (elapsed < shakeDuration)
        {
            animator.SetFloat("Y", -elapsed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(Second());
    }

    IEnumerator Second()
    {
        float elapsed = -1f;
        float shakeDuration = 1f;

        while (elapsed < shakeDuration)
        {
            animator.SetFloat("X", -elapsed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(Third());
    }
    IEnumerator Third()
    {
        float elapsed = -1f;
        float shakeDuration = 1f;

        while (elapsed < shakeDuration)
        {
            animator.SetFloat("Y", elapsed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(Fourth());
    }

    IEnumerator Fourth()
    {
        float elapsed = -1f;
        float shakeDuration = 1f;

        while (elapsed < shakeDuration)
        {
            animator.SetFloat("X", elapsed);
            elapsed += Time.deltaTime;
            yield return null;
        }

        StartCoroutine(First());
    }
}
