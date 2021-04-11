# vsmod-MooltiPackServerMod

Provides server-side support for the MooltiPack Launcher.

## How it Works

You won't need to maintain a mod list or modpack for your players to download.

Run this on your server, then get your players to use the MooltiPack Launcher to connect to your server. Your server will send a list of mods and versions, which will be automatically downloaded and installed. When you add, remove, or update mods, your players' mods will be automatically added, removed, and updated when they connect using the Launcher next.

Make sure to use the MooltiPack Launcher to connect to your own server to verify that everything is working.

## Mods Not Found?

The MooltiPack Launcher will try to find your server's mods on the official VS ModDB. If your mods are available there, you're all set.

If one or more of your mods (or the specific versions you're using) can't be found on the official VS ModDB, you'll need to set `alternateDownloadUrls` in the config file at `%appdata%/VintagestoryData/ModConfig/MooltiPackServerMod.json`. 

Make sure the modId and version match exactly. If you're unsure what the modId and version are, try looking in the server console or try connecting to your own server with the MooltiPack Launcher and looking at the error message.

For example, to specify a download URL for the "books" mod, version 1.0.1:

```
  "alternateDownloadUrls": [
    { "modId": "books", "version": "1.0.1", "downloadUrl": "https://github.com/cloutech/modbooks/raw/main/Releases/V101/ModBooksV101.zip" }
  ]
```

## Client Mods

All mods found by the game (e.g. in the Mods/ directory) will be considered part of your pack, even if they are Client-only mods. The easiest way to add Client-only mods is to add them to your server's Mods/ directory. They won't run on your server, but they will be examined for ModId and ModVersion and reported to MooltiPack Launchers.

In addition to the Client-only mods provided by your server, if players want to add their own custom Client-only mods, they can add them to the `%appdata%/VintagestoryMooltiPack/YOUR_SERVER_NAME/Mods` directory, after connecting once.

## Reloading Config File

If you need to make changes to the config file, you can simply restart the server for your changes to take effect. If you are impatient, like me, you can also use the `/mooltipackreloadconfig` command.

## Technical Details

This mod adds a new network protocol to the server. When a client connects using this protocol, the server responds with a JSON blob describing all the mods you have, including their versions, and optionally downloadUrls, then disconnects.

The protocol is a 4-byte packet (0xFF, 0xFF, 0xFF, 0xFF). The response is uncompressed JSON.

The MooltiPack Launcher runs on your players' computers and asks your server for this JSON blob before launching the game. It uses the list of mods your server provides to synchronize a new Mods/ directory with what's running on your server, downloading the mods from the VS ModDB (or alternateDownloadUrls.) Once the directory is set up, the game is launched normally.
