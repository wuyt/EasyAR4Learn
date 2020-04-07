using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using easyar;

public class ImageTargetsController : MonoBehaviour
{
    public ImageTrackerFrameFilter tracker;
    public ImageTargetController targetController;

    void Awake()
    {
        if (targetController)
        {
            targetController.TargetFound += () =>
            {
                Debug.LogFormat("Found target {{id = {0}, name = {1}}}", targetController.Target.runtimeID(), targetController.Target.name());
            };
            targetController.TargetLost += () =>
            {
                Debug.LogFormat("Lost target {{id = {0}, name = {1}}}", targetController.Target.runtimeID(), targetController.Target.name());
            };
            targetController.TargetLoad += (Target target, bool status) =>
            {
                Debug.LogFormat("Load target {{id = {0}, name = {1}, size = {2}}} into {3} => {4}", target.runtimeID(), target.name(), targetController.Size, targetController.Tracker.name, status);
            };
            targetController.TargetUnload += (Target target, bool status) =>
            {
                Debug.LogFormat("Unload target {{id = {0}, name = {1}}} => {2}", target.runtimeID(), target.name(), status);
            };
        }
    }
    public void SetTracker(bool status)
    {
        tracker.enabled = status;
    }
    public void SetTarget(bool status)
    {
        if (status)
        {
            targetController.Tracker = tracker;
        }
        else
        {
            targetController.Tracker = null;
        }
    }
}
