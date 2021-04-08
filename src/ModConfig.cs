using Vintagestory.API.Common;
using System.Collections.Generic;

namespace MooltiPackServerMod {

  public class ModConfig {

    public List<ModIdVersionDownloadUrl> extraMods = new List<ModIdVersionDownloadUrl>();

    public List<ModIdVersionDownloadUrl> alternateDownloadUrls = new List<ModIdVersionDownloadUrl>();

    // static helper methods
    public static ModConfig Load(ICoreAPI api) {
      var config = api.LoadModConfig<ModConfig>("MooltiPackServerMod.json");
      if (config == null) {
        config = new ModConfig();
        api.StoreModConfig(config, "MooltiPackServerMod.json");
      }
      return config;
    }
    public static void Save(ICoreAPI api, ModConfig config) {
      api.StoreModConfig(config, "MooltiPackServerMod.json");
    }
  }
}
