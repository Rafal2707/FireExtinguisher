using Unity.VisualScripting;
using UnityEngine;
public class Wire : MonoBehaviour
{
    [SerializeField] private Transform startTransform, endTransform;
    [SerializeField] private Transform segmentParent;

    private Transform[] segments;

    [SerializeField] int segmentCount = 10;
    [SerializeField] float totalLength = 10;
    [SerializeField] float radius = 0.5f;
    [SerializeField] float totalWeight = 10;
    [SerializeField] float drag = 1;
    [SerializeField] float angularDrag = 1;

    [SerializeField] bool usePhysics = false;

    private LineRenderer lineRenderer;

    void Start()
    {

        segments = new Transform[segmentCount];
        GenerateSegments();
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = segments.Length;
        DrawLines(segments, lineRenderer);

    }

    private void DrawLines(Transform[] segments, LineRenderer lineRenderer)
    {
        for (int i = 0; i < segments.Length;i++)
        {
            lineRenderer.positionCount = segments.Length;
            lineRenderer.SetPosition(i, segments[i].position);
        }
    }

    private void Update()
    {
        DrawLines(segments, lineRenderer);
    }

    private void GenerateSegments()
    {
        JoinSegment(startTransform, null, true);
        Transform previousTransform = startTransform;
        Vector3 direction = (endTransform.position - startTransform.position);

        for (int i = 0; i < segmentCount; i++)
        {
            GameObject segment = new GameObject($"segment{i}");
            segment.transform.SetParent(segmentParent);
            segments[i] = segment.transform;

            Vector3 pos = previousTransform.position + (direction / segmentCount);
            segment.transform.position = pos;
            JoinSegment(segment.transform, previousTransform);
            previousTransform = segment.transform; 
        }
        JoinSegment(endTransform, previousTransform, true, true);
    }

    private void JoinSegment(Transform current, Transform connectedTransform, bool isKinectic = false, bool isCloseConnected = false)
    {
        if(current.GetComponent<Rigidbody>() == null)
        {
            Rigidbody rigidbody = current.AddComponent<Rigidbody>();
            rigidbody.isKinematic = isKinectic;
            rigidbody.mass = totalWeight / segmentCount;
            rigidbody.drag = drag;
            rigidbody.angularDrag = angularDrag;
        }
        if(usePhysics) 
        { 
            SphereCollider sphereCollider = current.AddComponent<SphereCollider>();
            sphereCollider.radius = radius;
        }
        if (connectedTransform != null)
        {
            ConfigurableJoint joint = current.GetComponent<ConfigurableJoint>();
            if(joint == null)
            {
                joint = current.AddComponent<ConfigurableJoint>();
            }
            joint.connectedBody = connectedTransform.GetComponent<Rigidbody>();
            joint.autoConfigureConnectedAnchor = false;
            if (isCloseConnected)
            {
                joint.connectedAnchor = Vector3.forward * 0.1f;
            }
            else
            {
                joint.connectedAnchor = Vector3.forward * (totalLength / segmentCount);
            }
            joint.xMotion = ConfigurableJointMotion.Locked;
            joint.yMotion = ConfigurableJointMotion.Locked;
            joint.zMotion = ConfigurableJointMotion.Locked;

            joint.angularXMotion = ConfigurableJointMotion.Limited;
            joint.angularYMotion = ConfigurableJointMotion.Limited;
            joint.angularZMotion = ConfigurableJointMotion.Limited;

            SoftJointLimit softJointLimit = new SoftJointLimit();
            softJointLimit.limit = 0;
            joint.angularZLimit = softJointLimit;

            JointDrive jointDrive = new JointDrive();
            jointDrive.positionDamper = 0;
            jointDrive.positionSpring = 0;
            joint.angularXDrive = jointDrive;
            joint.angularYZDrive = jointDrive;
        }
    }
}
