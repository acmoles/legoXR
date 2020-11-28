﻿using Leap.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class LegoBrickSetup : MonoBehaviour
{
    public bool basePlate = false;
    public bool report = false;
    private bool setupComplete = false;

    public List<LocalPosition> knobs;
    public List<LocalPosition> slots;

    private void OnEnable()
    {
        Awake();
    }

    void Awake()
    {
        knobs = new List<LocalPosition>();
        slots = new List<LocalPosition>();
        PopulateConnections();
    }

    void PopulateConnections()
    {
        Transform connectivity = transform.GetChild(0).Find("Connectivity");

        if (connectivity)
        {
            string[] allowableKnob = { "knob", "hollowKnob", "hollowKnobFitInPegHole", "knobFitInPegHole" };
            string[] allowableSlot = { "antiKnob" };
            // Find knobs and slots
            foreach (Transform field in connectivity)
            {
                if (allowableKnob.Contains(field.GetChild(0).name))
                {
                    Transform knobsList = field;
                    foreach (Transform child in knobsList)
                    {
                        // TODO update LocalPosition to include a local rotation field
                        // Calculate it from knobsList rotation (will only have rotation if side-brick)
                        // This can then be used in Ghosting
                        LocalPosition knob = new LocalPosition();
                        knob.position = child.localPosition + knobsList.localPosition;
                        // Prebake realistic scale
                        knob.position = transform.GetChild(0).TransformVector(knob.position);
                        if (report) Debug.LogFormat("Knob at: {0}", knob.position);
                        if (allowableKnob.Contains(child.name))
                        {
                            knobs.Add(knob);
                        }
                        
                    }
                } else
                {
                    Transform slotsList = field;
                    foreach (Transform child in slotsList)
                    {
                        LocalPosition slot = new LocalPosition();
                        slot.position = child.localPosition + slotsList.localPosition;
                        // Prebake realistic scale
                        slot.position = transform.GetChild(0).TransformVector(slot.position);
                        if (report) Debug.LogFormat("Slot at: {0}", slot.position);
                        if (allowableSlot.Contains(child.name))
                        {
                            slots.Add(slot);
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log("missing connectivity");
            Debug.Log(connectivity);
        }

        if (report)
        {
            Debug.Log("Slot count: " + slots.Count);
            Debug.Log("Knobs count: " + knobs.Count);
        }
        setupComplete = true;
    }

    //private void OnDrawGizmos()
    //{
    //    //Collider col = GetComponent<Collider>();
    //    //Gizmos.DrawWireCube(col.bounds.center, col.bounds.extents);
    //    if (setupComplete && slots.Count > 0 && knobs.Count > 0)
    //    {
    //        Gizmos.color = Color.green;
    //        foreach (var item in slots)
    //        {
    //            var point = transform.TransformPoint(item.position);
    //            Gizmos.DrawLine(Vector3.zero, point);
    //        }
    //        Gizmos.color = Color.red;
    //        foreach (var item in knobs)
    //        {
    //            var point = transform.TransformPoint(item.position);
    //            Gizmos.DrawLine(Vector3.zero, point);
    //        }
    //    }

    //}


}
