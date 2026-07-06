using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    [Header("Optional Footstep Audio")]
    public AudioSource audioSource;
    public AudioClip footstepClip;
    public AudioClip landingClip;

    [Range(0f, 1f)]
    public float footstepVolume = 0.5f;

    [Range(0f, 1f)]
    public float landingVolume = 0.5f;

    public void OnFootstep(AnimationEvent animationEvent)
    {
        if (audioSource != null && footstepClip != null)
        {
            audioSource.PlayOneShot(footstepClip, footstepVolume);
        }
    }

    public void OnLand(AnimationEvent animationEvent)
    {
        if (audioSource != null && landingClip != null)
        {
            audioSource.PlayOneShot(landingClip, landingVolume);
        }
    }
}