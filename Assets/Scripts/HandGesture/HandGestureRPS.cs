using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static OVRSkeleton;

//public enum GestureType { None, Rock, Paper, Scissors }
public class HandGestureRPS : MonoBehaviour
{

    [Header("OVR References")]
    public OVRSkeleton skeleton;
    public OVRHand hand;

    [Header("Tuning")]
    [Range(0.05f, 0.5f)] public float holdTime = 0.15f;
    [Range(0.0f, 1.0f)] public float extendedThreshold = 0.30f;
    [Range(0.0f, 1.0f)] public float curledThreshold = 0.60f;

    [Header("Events")]
    public UnityEvent<GestureType> OnGestureChanged;
    
    public GestureType CurrentGesture { get; private set; } = GestureType.None;

    private readonly Dictionary<BoneId, Transform> _bones = new Dictionary<BoneId, Transform>();
    private GestureType _candidate = GestureType.Scissors;
    private float _candidateSince;

    void Awake()
    {
        if (!skeleton) skeleton = GetComponent<OVRSkeleton>();
        if (!hand) hand = GetComponent<OVRHand>();
    }

    void Update()
    {
        OnGestureChanged?.Invoke(CurrentGesture);
        
        if (skeleton == null || !skeleton.IsDataValid || !skeleton.IsDataHighConfidence) return;
        if (hand != null && hand.HandConfidence != OVRHand.TrackingConfidence.High) return;

        if (_bones.Count == 0) CacheBones();
        if (_bones.Count == 0) return;

        float thumbCurl = FingerCurl(BoneId.Hand_Thumb1, BoneId.Hand_Thumb2, BoneId.Hand_Thumb3, BoneId.Hand_ThumbTip);
        float indexCurl = FingerCurl(BoneId.Hand_Index1, BoneId.Hand_Index2, BoneId.Hand_Index3, BoneId.Hand_IndexTip);
        float middleCurl = FingerCurl(BoneId.Hand_Middle1, BoneId.Hand_Middle2, BoneId.Hand_Middle3, BoneId.Hand_MiddleTip);
        float ringCurl = FingerCurl(BoneId.Hand_Ring1, BoneId.Hand_Ring2, BoneId.Hand_Ring3, BoneId.Hand_RingTip);
        float pinkyCurl = FingerCurl(BoneId.Hand_Pinky1, BoneId.Hand_Pinky2, BoneId.Hand_Pinky3, BoneId.Hand_PinkyTip);
        
        bool thumbExtended = thumbCurl < extendedThreshold;
        bool indexExtended = indexCurl < extendedThreshold;
        bool middleExtended = middleCurl < extendedThreshold;
        bool ringExtended = ringCurl < extendedThreshold;
        bool pinkyExtended = pinkyCurl < extendedThreshold;

        bool thumbCurled = thumbCurl > curledThreshold;
        bool indexCurled = indexCurl > curledThreshold;
        bool middleCurled = middleCurl > curledThreshold;
        bool ringCurled = ringCurl > curledThreshold;
        bool pinkyCurled = pinkyCurl > curledThreshold;

        GestureType detected = Classify(
            thumbExtended, indexExtended, middleExtended, ringExtended, pinkyExtended,
            thumbCurled, indexCurled, middleCurled, ringCurled, pinkyCurled
        );

        if (detected != _candidate)
        {
            _candidate = detected;
            _candidateSince = Time.time;
        }

        if (Time.time - _candidateSince >= holdTime && _candidate != CurrentGesture)
        {
            CurrentGesture = _candidate;
            OnGestureChanged?.Invoke(CurrentGesture);
        }
    }

    private void CacheBones()
    {
        _bones.Clear();
        foreach (var bone in skeleton.Bones)
        {
            if (!_bones.ContainsKey(bone.Id))
                _bones.Add(bone.Id, bone.Transform);
        }
        EnsureBone(BoneId.Hand_Thumb1); EnsureBone(BoneId.Hand_Thumb2); EnsureBone(BoneId.Hand_Thumb3); EnsureBone(BoneId.Hand_ThumbTip);
        EnsureBone(BoneId.Hand_Index1); EnsureBone(BoneId.Hand_Index2); EnsureBone(BoneId.Hand_Index3); EnsureBone(BoneId.Hand_IndexTip);
        EnsureBone(BoneId.Hand_Middle1); EnsureBone(BoneId.Hand_Middle2); EnsureBone(BoneId.Hand_Middle3); EnsureBone(BoneId.Hand_MiddleTip);
        EnsureBone(BoneId.Hand_Ring1); EnsureBone(BoneId.Hand_Ring2); EnsureBone(BoneId.Hand_Ring3); EnsureBone(BoneId.Hand_RingTip);
        EnsureBone(BoneId.Hand_Pinky1); EnsureBone(BoneId.Hand_Pinky2); EnsureBone(BoneId.Hand_Pinky3); EnsureBone(BoneId.Hand_PinkyTip);
    }

    private void EnsureBone(BoneId id)
    {
        if (!_bones.ContainsKey(id))
        {
            var bone = FindBoneById(id);
            if (bone != null && !_bones.ContainsKey(id))
                _bones.Add(id, bone);
        }
    }

    private Transform FindBoneById(BoneId id)
    {
        foreach (var bone in skeleton.Bones)
            if (bone.Id == id) return bone.Transform;
        return null;
    }

    private float FingerCurl(BoneId knuckle, BoneId mid, BoneId distal, BoneId tip)
    {
        if (!_bones.ContainsKey(knuckle) || !_bones.ContainsKey(mid) || !_bones.ContainsKey(distal) || !_bones.ContainsKey(tip))
            return 1f;

        Vector3 p1 = _bones[knuckle].position;
        Vector3 p2 = _bones[mid].position;
        Vector3 p3 = _bones[distal].position;
        Vector3 p4 = _bones[tip].position;

        float seg1 = (p2 - p1).magnitude;
        float seg2 = (p3 - p2).magnitude;
        float seg3 = (p4 - p3).magnitude;
        float arcLen = Mathf.Max(1e-4f, seg1 + seg2 + seg3);
        float chord = (p4 - p1).magnitude;

        float straightness = Mathf.Clamp01(chord / arcLen);
        float curl = 1f - straightness;
        return Mathf.Clamp01(curl);
    }

    private GestureType Classify(
        bool tExt, bool iExt, bool mExt, bool rExt, bool pExt,
        bool tCurl, bool iCurl, bool mCurl, bool rCurl, bool pCurl)
    {
        if (iExt && mExt && rExt && pExt)
            return GestureType.Paper;

        if (iCurl && mCurl && rCurl && pCurl && (tCurl || !tExt))
            return GestureType.Rock;

        if (iExt && mExt && rCurl && pCurl)
            return GestureType.Scissors;

        return GestureType.None;
    }
}