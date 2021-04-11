using HarmonyLib;
using System.Collections.Generic;
using Vintagestory.Server;
using Vintagestory.Common;
using System.Text;

namespace MooltiPackServerMod {

  public class ModReport {
    public string modId;
    public string version;
  }

  [HarmonyPatch(typeof(ServerMain))]
  [HarmonyPatch("HandleClientPacket")]
  public class Patch_ServerMain_HandleClientPacket {
    public static string ResponseString;
    static bool Prefix(
      ConnectedClient client,
      byte[] data,
      ServerMain __instance,
      ServerCoreAPI ___api,
      Dictionary<int, ConnectedClient> ___Clients
    ) {
      if (data.Length != 4) { return true; } // HARMONY CONTINUE
      if (data[0] != 255) { return true; } // HARMONY CONTINUE
      if (data[1] != 255) { return true; } // HARMONY CONTINUE
      if (data[2] != 255) { return true; } // HARMONY CONTINUE
      if (data[3] != 255) { return true; } // HARMONY CONTINUE

      ___api.Logger.Notification($"MooltiPackServerMod is repsonding to a query from client id {client.XXX_GetFieldValue<int>("Id")}!");

      var compressed = false;
      var enumSendResult = client.XXX_GetFieldValue<NetConnection>("Socket").Send(Encoding.ASCII.GetBytes(ResponseString), compressed);

      __instance.EnqueueMainThreadTask(delegate {
        ___Clients.Remove(client.XXX_GetFieldValue<int>("Id"));
        client.XXX_InvokeVoidMethod("CloseConnection");
      });

      return false; // HARMONY SKIP
    }
  }
}