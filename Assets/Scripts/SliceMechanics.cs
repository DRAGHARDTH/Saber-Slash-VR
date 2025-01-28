using UnityEngine;
using EzySlice;
using System.Collections;

public class SliceMechanics : MonoBehaviour
{
    [Header("Slicing Settings")]
    [Tooltip("Layer mask specifying which objects can be sliced.")]
    [SerializeField] private LayerMask sliceableLayer;

    [Tooltip("Starting point of the slicing line.")]
    [SerializeField] private Transform startSlicePoint;

    [Tooltip("Ending point of the slicing line.")]
    [SerializeField] private Transform endSlicePoint;

    [Header("Velocity Settings")]
    [Tooltip("Velocity estimator to calculate slicing velocity.")]
    [SerializeField] private VelocityEstimator velocityEstimator;

    [Header("Explosion Effect")]
    [Tooltip("Force applied to the sliced pieces to create an explosion effect.")]
    [SerializeField] private float explosionForce = 200f;

    private void Update()
    {
        // Check if the slicing line intersects with any object in the sliceable layer.
        if (Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out RaycastHit hit, sliceableLayer))
        {
            GameObject target = hit.transform.gameObject;
            Slice(target);
        }
    }

    /// <summary>
    /// Slices the specified target object along a plane defined by the slicing points and velocity.
    /// </summary>
    /// <param name="target">The object to be sliced.</param>
    private void Slice(GameObject target)
    {
        // Calculate the plane normal for slicing.
        Vector3 velocity = velocityEstimator.GetVelocityEstimate();
        Vector3 planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity).normalized;

        // Perform the slicing operation.
        SlicedHull hull = target.Slice(endSlicePoint.position, planeNormal);

        if (hull != null)
        {
            // Create and setup the upper and lower hulls.
            GameObject upperHull = hull.CreateUpperHull(target, target.GetComponent<Renderer>().material);
            SetupSlicedComponent(upperHull);

            GameObject lowerHull = hull.CreateLowerHull(target, target.GetComponent<Renderer>().material);
            SetupSlicedComponent(lowerHull);

            // Destroy the original object after slicing.
            target.SetActive(false);
        }
    }

    /// <summary>
    /// Sets up the sliced object with a Rigidbody and a convex MeshCollider.
    /// </summary>
    /// <param name="slicedObject">The sliced object to be configured.</param>
    private void SetupSlicedComponent(GameObject slicedObject)
    {         
        Rigidbody rigidBody = slicedObject.AddComponent<Rigidbody>();

        MeshCollider collider = slicedObject.AddComponent<MeshCollider>();
        collider.convex = true;

        slicedObject.layer = 3; 
       
        rigidBody.AddExplosionForce(explosionForce, slicedObject.transform.position, 1f);
        StartCoroutine(DelayedDestroy(2.5f, slicedObject));
    }

    IEnumerator DelayedDestroy(float delay, GameObject slicedObject)
    {
        yield return new WaitForSeconds(delay); // Wait for the specified delay
        Destroy(slicedObject);
    }
}
