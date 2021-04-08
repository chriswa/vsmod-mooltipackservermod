# vsmod-MooltiPackServerMod

Provides server-side support for the MooltiPack Launcher.

Adds support for MooltiPack Launcher's mod query request.

After updating mods, make sure you try connecting to your own server with the MooltiPack Launcher to ensure that your players will be able to as well.

## Mods Not Found

If one or more of your mods (or the specific version you're using) can't be found on the official VS ModDB, you can specify `alternateDownloadUrls` in the config file at `%appdata%/VintagestoryData/ModConfig/MooltiPackServerMod.json`. Make sure the modId and version match exactly. If you're unsure what the modId and version are, try connecting with the MooltiPack Launcher and looking at the error message.

For example:

```
  "alternateDownloadUrls": [
    { "modId": "extrachests", "version": "1.3.1", "downloadUrl": "http://mods.vintagestory.at/files/asset/217/ExtraChestsCC_v1.3.1.zip" },
    { "modId": "books", "version": "1.0.1", "downloadUrl": "https://github.com/cloutech/modbooks/raw/main/Releases/V101/ModBooksV101.zip" }
  ]
```

## Adding Client Mods

By default, only "Universal" mods which are "RequiredOnClient" are included. To add "Client-only" mods (or Universal mods which are not RequiredOnClient) to the list of mods which your players will automatically download, you can specify `extraMods` in the config file at `%appdata%/VintagestoryData/ModConfig/MooltiPackServerMod.json`.

For example:

```
  "extraMods": [
    { "modId": "quickstep", "version": "1.0.1" },
    { "modId": "survivalcats", "version": "1.2.7" },
    { "modId": "zoombutton", "version": "1.1.0" }
  ],
```

If you need to specify an alternate download url for a client mod, you can add a `"downloadUrl"` key to the extra mod item.

## Reloading Config File

If you need to make changes to the config file, you can simply restart the server for your changes to take effect. If you are impatient, you can also use the `/mooltipackreloadconfig` command.


## Technical Info

The MooltiPack Launcher connects to your server and sends a custom 4-byte packet (0xFF, 0xFF, 0xFF, 0xFF). This mod adds a listener for that packet and responds with a JSON blob describing your required mods, which is generated automatically from running mods and customized by the ModConfig file as described above.
