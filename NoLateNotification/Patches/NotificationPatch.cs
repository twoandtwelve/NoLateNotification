using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace NoLateNotification.Patches
{
    [HarmonyPatch(typeof(TimeOfDay))]

    internal class NotificationPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void notificationPatch(ref bool ___shipLeavingAlertCalled)
        {
            if (NoLateNotification.Instance.isNoLateNotificationEnabledEntry.Value)
            {
                ___shipLeavingAlertCalled = true;
            }
        }
    }
}