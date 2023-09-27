# Audio Class Documentation

The `Audio` class in Easy14 provides functions for playing and managing audio files.

## Table of Contents

- Audio.Start(x);
- Audio.Stop();

## Audio.Start(x)

The `Audio.Start(x)` function allows you to start playing an audio file. It takes the file path as a parameter.
### Parameters

`x`: The file path of the audio file to play.

### Example

```easy14
Audio.Start("path/to/audio/file.wav");
```

## Audio.Stop()

The `Audio.Stop()` function stops the audio player with the specified index.

### Parameters

- `indexOfAudioPlayer`: The index of the audio player to stop. Each audio player is assigned an index when started, and you can use this index to stop a specific player.

### Example

```easy14
// Stop the audio player with index 1 (assuming it was started first)
Audio.Stop(1);
```

That's a brief overview of the `Audio` class in Easy14, including how to stop a specific audio player using its index. These functions provide basic audio playback functionality for your Easy14 programs.