using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class Checkpoints : MonoBehaviour
{
    public Checkpoint_Manager checkpoint_Manager;

    public GameObject checkpointPainted;
    public GameObject checkpointNotPainted;
    public GameObject spray;

    [SerializeField]
    GameObject _vfxCheckpoint;

    private void Start()
    {
        checkpointPainted.SetActive(false);
        spray.SetActive(false);
        checkpointNotPainted.SetActive(true);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            checkpoint_Manager.ChangeCheckpointIndex(transform);

            if (!checkpointPainted.activeSelf)
            {
                spray.SetActive(true);
                FMODAudioManager.instance.PlayOneShot(FMODEvents.instance.checkPoint, this.gameObject.transform.position);
                _vfxCheckpoint.SetActive(true);


                Animator sprayAnimator = spray.GetComponent<Animator>();
                if (sprayAnimator != null)
                {
                    StartCoroutine(WaitForAnimationToEnd(sprayAnimator, spray));
                }

                checkpointPainted.SetActive(true);
                checkpointNotPainted.SetActive(false);
            }
        }
    }

    private IEnumerator WaitForAnimationToEnd(Animator animator, GameObject sprayObject)
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        while (stateInfo.normalizedTime < 1.0f || !stateInfo.IsName("Spray"))
        {
            stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            yield return null;
        }

        sprayObject.SetActive(false);
        _vfxCheckpoint.SetActive(false);
    }
}