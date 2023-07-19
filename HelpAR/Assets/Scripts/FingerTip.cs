using Microsoft.MixedReality.Toolkit;
using Microsoft.MixedReality.Toolkit.Subsystems;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FingerTip: MonoBehaviour
{
    HandsAggregatorSubsystem aggregator;
    bool initialized = false;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(EnableWhenSubsystemAvailable());
    }

    // Update is called once per frame
    void Update()
    {
        if (!initialized) return;
        bool joinIsValid = aggregator.TryGetJoint(TrackedHandJoint.IndexTip, UnityEngine.XR.XRNode.RightHand, out HandJointPose jointPose);
        if (joinIsValid)
        {
            transform.position = jointPose.Position;
        }
    }

    private void Init()
    {
        aggregator = XRSubsystemHelpers.GetFirstRunningSubsystem<HandsAggregatorSubsystem>();
        initialized = true;
    }

    IEnumerator EnableWhenSubsystemAvailable()
    {
        yield return new WaitUntil(() =>
        {
            return XRSubsystemHelpers.GetFirstRunningSubsystem<HandsAggregatorSubsystem>() != null;
        });
        Init();
    }
}
