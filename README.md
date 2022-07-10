# Black Ops II Sound Studio Extended
Modified version of Black Ops II Sound Studio by master131. This version adds a new feature under the Tools tab: the Replace Manager.
The Replace Manager allows you to load and replace many sound files without having to choose them manually, adecuate for modding full Zombies maps to
other languages. Say, for example, you want to change Origins to German. You only need to extract all sound files from the German .sabs file, open the English
.sabs file and use the Replace Manager to tell it where all the German sound files are, then it will take care of the rest.

WARNING: The Replace Manager is not fully tested yet, which is why there is no warranty of it working correctly. Use it at your own risk.

This is all thanks to master131, who created the original program. Thank you!

Replace Manager:

![](https://github.com/tovaru/Black-Ops-II-Sound-Studio-Extended/blob/master/screenshots/replace_manager_v2_ss.jpg?raw=true)

## Replace Manager Features:
* Retrieves all files in a folder.
* Automatically matches sound files so you don't need to choose them one by one.
* Automatically converts and replaces every matched sound file.
* Allows to export progress output to a text file.

## New Features:
* Added the Replace Manager.
* Added support for Black Ops 3 SABS/SABL files.

## Limitations:
* Audio replacement of XMA audio entries are currently not supported as an encoder hasn't been added yet.
* Audio replacement for PS3 is not tested.
* Replacing headerless WAV audio with your own that is 1 second or more longer than the original will result in SP missions loading forever (same might happen with MP, not tested)
* Backups are not created, it's up to the user to do that themselves.
* Editing the asset references list actually does nothing when saved to disk.
* Saving is only supported for Black Ops 2 and Black Ops 3 SABS/SABL files.

## Legal:
This software uses a compiled/command-line version of [FFmpeg](http://ffmpeg.org/) licensed under GPLv3 built by Kyle Schwarz/Zeranoe which can be downloaded [here](https://ffmpeg.zeranoe.com/builds/) along with the original source code.

This tool is in no way affiliated with Activision Blizzard or Treyarch and does not contain any copyrighted files from the game, "Black Ops II" or "Black Ops III".

## Credits:
* master131
* kokole
* Red-EyeX32
* Jakes625
* Deadmau5
* Stanimir Stoyanov
* Xplorer
* AlphaTwentyThree
