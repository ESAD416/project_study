using System.Collections;
using System.Collections.Generic;
using UnityEditor.Events;
using UnityEngine;
using UnityEngine.Events;

public static class UnityEventUtil
{
   public static void RemoveAllListenersWithPersistent(UnityEventBase unityEvent)
    {
        var eventInvocationList = unityEvent.GetPersistentEventCount();
        for (int i = 0; i < eventInvocationList; i++)
        {
            UnityEventTools.RemovePersistentListener(unityEvent, i);
            // unityEvent.SetPersistentListenerState(i, UnityEventCallState.Off);
        }

        unityEvent.RemoveAllListeners();
    }
}
