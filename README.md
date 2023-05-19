# ComplexGameSystems

Notes:
Unloading + Preloading audio for distance based environmental sounds (memory management)

https://gamedevbeginner.com/unity-audio-optimisation-tips/#preload_audio_data


Updating Waveform in real time for fades / loading waveform texture
Use burst jobs

AudioClip.SetData is non destructive and only relevant per play session
Any Fades / audio clip changes will have to be Set on load 

Project Settings > Audio > Max Voices

Manually Pausing / Playing continuous audio sources

Compatibility with exposed mixergroup parameters?