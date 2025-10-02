using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Oculus.Interaction.Input;


public class HandGestureXR_RPS : MonoBehaviour
{
    [Header("References (Interaction SDK)")]
    [Tooltip("Oculus.Interaction.Input.Hand (IHand implementation)")]
    public Hand hand; // Drag & drop the Hand component here

    [Header("Tuning")]
    [Range(0.05f, 0.5f)] public float holdTime = 0.15f;
    [Range(0.0f, 1.0f)] public float extendedThreshold = 0.30f;
    [Range(0.0f, 1.0f)] public float curledThreshold = 0.60f;
    
    [Tooltip("Normalize distance thresholds by hand scale")]
    public bool normalizeByScale = true;

    [Header("Events")]
    public UnityEvent<GestureType> OnGestureChanged;
    public UnityEvent<GestureType> GestureAction;


    public GestureType CurrentGesture { get; private set; } = GestureType.None;

    private GestureType _candidate = GestureType.None;
    private float _candidateSince = 0f;
    
    
    // Update is called once per frame
    void Update()
    {
        if (hand == null || !hand.IsTrackedDataValid) return;

        // Finger extended/curl detection
        bool idxExt  = IsFingerExtended(HandJointId.HandIndex1,  HandJointId.HandIndex2,  HandJointId.HandIndex3,  HandJointId.HandIndexTip);
        bool midExt  = IsFingerExtended(HandJointId.HandMiddle1, HandJointId.HandMiddle2, HandJointId.HandMiddle3, HandJointId.HandMiddleTip);
        bool ringExt = IsFingerExtended(HandJointId.HandRing1,   HandJointId.HandRing2,   HandJointId.HandRing3,   HandJointId.HandRingTip);
        bool litExt  = IsFingerExtended(HandJointId.HandPinky1,  HandJointId.HandPinky2,  HandJointId.HandPinky3,  HandJointId.HandPinkyTip);

        // (optional) improve Paper accuracy with thumb
        bool thmExt  = IsFingerExtended(HandJointId.HandThumb1,  HandJointId.HandThumb2,  HandJointId.HandThumb3,  HandJointId.HandThumbTip);

        // Simple rules:
        // Rock: all fingers curled
        // Paper: all fingers extended (thumb optional)
        // Scissors: index + middle extended, others curled
        GestureType detected = GestureType.None;

        if (!idxExt && !midExt && !ringExt && !litExt)
            detected = GestureType.Rock;
        else if (idxExt && midExt && ringExt && litExt) // add && thmExt if you want
            detected = GestureType.Paper;
        else if (idxExt && midExt && !ringExt && !litExt)
            detected = GestureType.Scissors;
        // Stabilize with holdTime
        if (detected != _candidate)
        {
            _candidate = detected;
            _candidateSince = Time.time;
        }
        else
        {
            if (Time.time - _candidateSince >= holdTime && CurrentGesture != detected)
            {
                CurrentGesture = detected;
                OnGestureChanged?.Invoke(CurrentGesture);
            }
        }

        GestureAction?.Invoke(CurrentGesture);
    }

    /// <summary>
    /// Check if a finger is extended by comparing the distance
    /// between its proximal and tip joints.
    /// </summary>
    bool IsFingerExtended(HandJointId prox, HandJointId inter, HandJointId dist, HandJointId tip)
    {
        if (!TryGet(prox, out Pose p) ||!TryGet(inter, out Pose i) || !TryGet(dist, out Pose d) ||!TryGet(tip, out Pose t))
            return false; // no data → treat as not extended

        // Straightness: if finger is extended, vectors are close to colinear (dot ~ 1)
        Vector3 v1 = (i.position - p.position).normalized;
        Vector3 v2 = (d.position - i.position).normalized;
        Vector3 v3 = (t.position - d.position).normalized;
        float straightness = (Vector3.Dot(v1, v2) + Vector3.Dot(v2, v3)) * 0.5f; // [-1..1]

        // Length ratio: span / (sum of segments)
        float l1 = Vector3.Distance(p.position, i.position);
        float l2 = Vector3.Distance(i.position, d.position);
        float l3 = Vector3.Distance(d.position, t.position);
        float span = Vector3.Distance(p.position, t.position);
        float ratio = span / Mathf.Max(l1 + l2 + l3, 1e-4f);

        // Thresholds (adjust if needed)
        return (straightness > 0.85f) && (ratio > 0.75f);
    }

    bool TryGet(HandJointId id, out Pose pose)
    {
        return hand.GetJointPose(id, out pose); // with Oculus.Interaction.Input.Hand
    }
    
}
