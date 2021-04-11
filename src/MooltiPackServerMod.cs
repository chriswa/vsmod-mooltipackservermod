using System.Collections.Generic;
using System.Linq;
using HarmonyLib;
using Newtonsoft.Json;
using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.Common;
using Vintagestory.Server;

[assembly: ModInfo("MooltiPackServerMod")]

namespace MooltiPackServerMod {
  public class MooltiPackServerMod : ModSystem {

    ModConfig config;

    public override void StartServerSide(ICoreServerAPI sapi) {

      config = ModConfig.Load(sapi);

      sapi.RegisterCommand("mooltipackreloadconfig", "Reload MooltiPackServerMod.json", "/mooltipackreloadconfig", (IServerPlayer player, int groupId, CmdArgs args) => {
        config = ModConfig.Load(sapi);
        prepareResponseString(sapi);
      }, Privilege.root);

      System.Type[] parameterTypes = new System.Type[] { typeof(EnumServerRunPhase), typeof(Vintagestory.API.Common.Action) };
      (sapi.World as ServerMain).ModEventManager.XXX_InvokeVoidMethod("RegisterOnServerRunPhase", parameterTypes, new object[] {
        EnumServerRunPhase.RunGame, (Vintagestory.API.Common.Action)(() => {
          sapi.World.RegisterCallback((float dt) => {
            sapi.Logger.Notification("MooltiPackServerMod - RunGame!");

            prepareResponseString(sapi);

            sapi.Logger.Notification("MooltiPackServerMod patching now...");
            var harmony = new Harmony("goxmeor.MooltiPackServerMod");
            harmony.PatchAll();
            sapi.Logger.Notification("MooltiPackServerMod patched!");
          }, 1000);
        })
      });
    }

    private void prepareResponseString(ICoreServerAPI sapi) {
      var allModInfos = (sapi.ModLoader as ModLoader).LoadModInfos();

      List<ModIdVersionDownloadUrl> modList = (from mod in allModInfos
                                               where 
                                                ((mod.Info.Side.IsUniversal() && mod.Info.RequiredOnClient) || mod.Info.Side.IsClient())
                                                && mod.Info.ModID != "game" && mod.Info.ModID != "survival" && mod.Info.ModID != "creative"
                                               select new ModIdVersionDownloadUrl {
                                                 modId = mod.Info.ModID,
                                                 version = mod.Info.Version
                                               }).ToList();

      modList.AddRange(config.extraMods);

      foreach (var alternate in config.alternateDownloadUrls) {
        foreach (var mod in modList) {
          if (mod.modId == alternate.modId && mod.version == alternate.version) {
            mod.downloadUrl = alternate.downloadUrl;
          }
        }
      }

      var json = JsonConvert.SerializeObject(modList);

      sapi.Logger.Notification("MooltiPackServerMod prepared response string: {0}", json);

      Patch_ServerMain_HandleClientPacket.ResponseString = json;
    }
  }
}
