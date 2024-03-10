using BepInEx;
using BepInEx.Configuration;
using BepInEx.Logging;
using HarmonyLib;
using LethalConfig;
using LethalConfig.ConfigItems;
using NoLateNotification.Patches;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoLateNotification
{
    [BepInPlugin(modGUID, modName, modVersion)]
    [BepInDependency("ainavt.lc.lethalconfig")]
    public class NoLateNotification : BaseUnityPlugin
    {
        private const string modGUID = "Jacky.NoLateNotification";
        private const string modName = "NolateNotification";
        private const string modVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);

        public static NoLateNotification Instance;

        public static ManualLogSource logger;

        public ConfigEntry<bool> isNoLateNotificationEnabledEntry;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }

            logger = base.Logger;

            logger.LogInfo("NoLateNotification mod has awaken");

            isNoLateNotificationEnabledEntry = Config.Bind("NoLateNotification Config", "Disable Time Late Notification", true, "This disables the ship is leaving soon notification!");
    
            BaseConfigItem isNoLateNotificationEnabled = new BoolCheckBoxConfigItem(isNoLateNotificationEnabledEntry, requiresRestart: false);

            LethalConfigManager.AddConfigItem(isNoLateNotificationEnabled);


            harmony.PatchAll(typeof(NotificationPatch));
        }
    }
}