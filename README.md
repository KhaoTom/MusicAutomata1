# MusicAutomata1

work in progress

* left mouse button down -> draw
* space -> toggle play/pause
* enter -> step forward
* backspace -> reset

cellular automata rules:
* cell can have values [0, max] (max is currently 4)
* cell with less than two neighbors decrements by 1, clamped at 0
* cell with two neighbors stays at current value
* cell with three neighbors increments by 1
* cells that exceed max are set to 0
* cell with more than three neighbors set to 0

playback:
* during play mode, the highlighted row is evaluated on each tick.
* each column has a parameter string of 2 hex characters. the 1st  character is the audio channel, the 2nd character is the parameter which the cell value will affect.

current channels
* there is currently only 5 channels: [0, 4] which contain 5 notes of c major pentatonic scale.

current parameters
* 0: unused, the column does not affect audio
* 1: volume, this column affect volume of the assigned audio channel
* 2: clip position, this column affects the playback position within the audio clip, 0 means start, 1 is 20% of clip length from start, 2 is 40%, etc.
* 3 to F: unused, the column does not affect audio
