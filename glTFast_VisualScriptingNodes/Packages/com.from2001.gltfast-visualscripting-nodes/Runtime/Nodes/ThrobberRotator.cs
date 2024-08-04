using UnityEngine;
using System.Collections;

/// <summary>
/// Rotates the throbber to make it seem like a loading animation
/// </summary>
public class ThrobberRotator : MonoBehaviour
{
    private Transform iconTransform;
    private Coroutine rotationCoroutine;

    public void Initialize(Transform iconTransform)
    {
        this.iconTransform = iconTransform;
        rotationCoroutine = StartCoroutine(RotateThrobber());
    }

    private IEnumerator RotateThrobber()
    {
        while (true)
        {
            iconTransform.Rotate(Vector3.forward, -360 * Time.deltaTime);
            yield return null;
        }
    }

    public void StopRotation()
    {
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
        }
    }
}