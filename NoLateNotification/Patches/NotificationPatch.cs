using BepInEx;
using HarmonyLib;
using SigurdLib.Util.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;


namespace NoLateNotification.Patches
{
    [HarmonyPatch(typeof(TimeOfDay))]

    internal class NotificationPatch
    {
        [HarmonyPatch("Update")]
        [HarmonyPostfix]
        static void disableNotificationPatch(ref bool ___shipLeavingAlertCalled)
        {
            if (NoLateNotification.Instance.isNoLateNotificationEnabledEntry.Value)
            {
                ___shipLeavingAlertCalled = true;
            }
        }

        [HarmonyPatch("TimeOfDayEvents")]
        [HarmonyTranspiler]
        static IEnumerable<CodeInstruction> randomNotificationPatch(IEnumerable<CodeInstruction> instructions)
        {
            var instructionList = instructions.ToList();
            if (NoLateNotification.Instance.isRandomNotificationEnabledEntry.Value)
            {
               
                for (int i = 0; i < instructionList.Count; i++)
                {
                    var instruction = instructionList[i];

                    if (instruction.opcode == OpCodes.Ldc_R4 && (float)instruction.operand == 0.9f)
                    {
                        Random random = new Random();
                        instruction.operand = (float)random.NextDouble();

                        var multiplierInstruction = new CodeInstruction(OpCodes.Ldc_R4, 0.35f);
                        var mulInstruction = new CodeInstruction(OpCodes.Mul);
                        var constantInstruction = new CodeInstruction(OpCodes.Ldc_R4, 0.5f);
                        var addInstruction = new CodeInstruction(OpCodes.Add);

                        instructionList.Insert(i + 1, multiplierInstruction);
                        instructionList.Insert(i + 2, mulInstruction);
                        instructionList.Insert(i + 3, constantInstruction);
                        instructionList.Insert(i + 4, addInstruction);

                        break;
                    }
                }
            }
            //CodeMatcher matcher = new CodeMatcher(instructionList);
            //NoLateNotification.logger.LogDebugInstructionsFrom(matcher);
            return instructionList;
        }
    }
}