using System.Collections; // Needed for Coroutines
using UnityEngine;

public class DoorQuizManager : MonoBehaviour
{
    [Header("References")]
    public GameObject ghost;
    public Animator ghostAnimator;
    public AudioSource scareSound;
    public Transform playerCamera;

    [Header("Door Settings")]
    public Animator doorAnimator;

    private bool isFollowing = false;

    public void SelectAnswer(bool isCorrect)
    {
        if (isCorrect)
        {
            doorAnimator.SetTrigger("openDoor");
            this.gameObject.SetActive(false);
        }
        else
        {
            // Start the special timer-based scare
            StartCoroutine(ScareAndFollowRoutine());
        }
    }

    IEnumerator ScareAndFollowRoutine()
    {
        // 1. Teleport in front of player
        Vector3 scarePos = playerCamera.position + (playerCamera.forward * 1.5f);
        // scarePos.y = playerCamera.position.y - 1.0f; // Kept at floor level
        scarePos.y = playerCamera.position.y; // Kept at floor level
        ghost.transform.position = scarePos;
        
        // 2. Start effects
        ghostAnimator.SetTrigger("doAttack");
        scareSound.Play();
        isFollowing = true;

        // 3. Follow the player for 5 seconds
        float timer = 0;
        float stopDistance = 1.2f; // Adjust this: higher = further away, lower = closer

        while (timer < 5f)
        {
            // Calculate where the player is on the ground
            Vector3 targetPos = new Vector3(playerCamera.position.x, ghost.transform.position.y, playerCamera.position.z);
            
            // Calculate how far the ghost is from the player
            float distanceToPlayer = Vector3.Distance(ghost.transform.position, targetPos);

            // ONLY move if the ghost is further away than the stopDistance
            if (distanceToPlayer > stopDistance)
            {
                ghost.transform.position = Vector3.MoveTowards(ghost.transform.position, targetPos, Time.deltaTime * 2.5f);
            }
            
            // Still always look at the player
            ghost.transform.LookAt(targetPos);
            
            timer += Time.deltaTime;
            yield return null;
        }

        // 4. Hide under the map
        isFollowing = false;
        ghost.transform.position = new Vector3(0, -50, 0); 
        Debug.Log("Ghost has retreated.");
    }
}