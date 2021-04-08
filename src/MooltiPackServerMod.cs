using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.Server;
using Vintagestory.Server;

[assembly: ModInfo("MooltiPackServerMod")]

namespace MooltiPackServerMod {
  public class MooltiPackServerMod : ModSystem {

    ModConfig config;

    public override void StartServerSide(ICoreServerAPI sapi) {

      config = ModConfig.Load(sapi);
      Patch_ServerMain_HandleClientPacket.config = config;

      sapi.RegisterCommand("mooltipackreloadconfig", "Reload MooltiPackServerMod.json", "/mooltipackreloadconfig", (IServerPlayer player, int groupId, CmdArgs args) => {
        config = ModConfig.Load(sapi);
        Patch_ServerMain_HandleClientPacket.config = config;
      }, Privilege.root);

      System.Type[] parameterTypes = new System.Type[] { typeof(EnumServerRunPhase), typeof(Vintagestory.API.Common.Action) };
      (sapi.World as ServerMain).ModEventManager.XXX_InvokeVoidMethod("RegisterOnServerRunPhase", parameterTypes, new object[] {
        EnumServerRunPhase.RunGame, (Vintagestory.API.Common.Action)(() => {
          sapi.Logger.Notification("MooltiPackServerMod - RunGame!");

          sapi.Logger.Notification("MooltiPackServerMod patching now...");
          var harmony = new Harmony("goxmeor.MooltiPackServerMod");
          harmony.PatchAll();
          sapi.Logger.Notification("MooltiPackServerMod patched!");
        })
      });
    }
  }
}
