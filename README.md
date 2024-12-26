# GalaxyBMSConverter
Convert MIDI files into Galaxy BMS files!

![image](https://github.com/user-attachments/assets/f8e2b291-8be0-4c44-a30e-bfbb0177aad1)

> *Notice: The Hack.io libraries that are in this repository will eventually be moved to the main Hack.io repository*

## Requirements
- [.NET 8.0 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

## Usage
This program allows you to convert MIDI files into BMS files. Follow these simple steps:

- Fill the filepaths at the top of the program window, with your BMS Instrument List, MIDI Instrument Lookup, and MIDI
  - The BMS Instrument list is a list of availible instruments in the target game (Generally doesn't change between MIDIs)
  - The MIDI Instrument Lookup is a list of instruments to remap the MIDI instruments to (This can be specific to each song)
  - The MIDI file you want to convert must be a properly formatted MIDI.
- \[Optional\] Choose a BMS Track to put your Timing Track & Chords into (Usually Track 0)
- \[Optional\] Setup your Chords
- Set the Loop Points for your MIDI (in clock pulses. This is relative to whatever the MIDI file has for it's PPQN)
  - Set both to 0 to have your MIDI not loop at all
  - Set both to -1 to repeat the entire MIDI
  - Try to align your loop points to a full Measure
  - If your MIDI has notes which extend beyond the loop point, you may want to check the "Close Voices on Loop" checkbox. This is not always needed.
- \[Optional\] Select your optimizations. Each one has an explaination in the program itself.
- Remap the MIDI channels to BMS tracks.
  - The BMS Track with the Timing Track cannot have a MIDI channel assigned to it.
  - Choose \<Do Not Convert\> to ignore a MIDI Channel during conversion.
- Click Convert!

### Conversion Warnings
Once conversion is complete, it will give you a list of warnings, if any occured during conversion.

## Importing into the game
Once you have your working BMS file (it will be in .szs format if it's small enough to fit into the Sequence memory in-game), you can use [WiiExplorer](https://github.com/SuperHackio/WiiExplorer) to edit `AudioRes/Seqs/JaiSeq.arc`. Replace a song of your choosing (using ALT+R on the file you want to replace), then go to the File Properties of your file, and click the "Auto" button to set the compression flags correctly. Save the archive and assuming you did it correctly, it will work in-game.

## Limitations
- There is a maximum uncompressed sequence data size of 56kb in both Galaxy games. A BMS file cannot exceed this limit.
- The program only supports one set of percussion at this time. All channels that use a percussion instrument will use this one set.
- MIDI Gates are not supported. All midi notes must have both an ON and OFF.
- Decimal BPM (120.5 for example) is not supported by the BMS file format directly, as the BPM is stored as an integer value.
- The Maximum BPM is 255
- The Maximum MIDI channel count is 16 (15 if you plan on having a timing track in your BMS). Note that this does not mean you can't have more than 16 MIDI *tracks* (two midi tracks that use channel 8 will be merged into one BMS Track during conversion)
- BMS has a concept called Subroutines, which is not supported by the converter at this time.
- All MIDI Control Changes (CCs) are always written, so having a lot can result in larger files. An option for this to be compressed hasn't been found in the game code yet as far as I know.
- Advanced BMS Logic (such as register & variable management) are not supported.

## Advanced Features
- Timing Track Generation
  - This program allows the generation of Timing Tracks, which are BMS files that contain the following information:
    - Song BPM
    - Where the beats are in the song
    - Chord selection
  - If you want to generate a Timing Track without having a MIDI file, Simply type `_` as the MIDI filename, and a MIDI will not be used.
    - If you do this, you must have the Timing Track selected with the dropdown, and also have at least one Chord entry at MIDI Tick 0 that has the song's BPM set.
    - In this situation, all MIDI ticks are calculated with a PPQN of 120 (which is the standard for Galaxy)
  - If you are using this for an AST file, ensure your AST file's loop points line up with the loop points in the BMS file
- CIT Generation
  - TODO: Write how to use the Chords Editor
- Audio Effects (Pitch Bend & MIDI Control Changes)
  - Pitch bending is supported through the MIDI standard for pitch changes
  - The converter supports the following MIDI effects (All of which only apply to the BMS Track that they are actually in)
    - MIDI Control Change 1 (*CC1*) will set the **Vibrato Depth**
    - MIDI Control Change 2 (*CC2*) will set the **Vibrato Rate**
    - MIDI Control Change 7 (*CC7*) will set the **Master Volume**
    - MIDI Control Change 11 (*CC11*) will set the **Expression Volume**
    - MIDI Control Change 10 (*CC10*) will set the **Panning**
    - MIDI Control Change 91 (*CC91*) will set the **Reverb**
    - MIDI Control Change 92 (*CC92*) will set the **Volume Tremolo Strength**
    - MIDI Control Change 93 (*CC93*) will set the **Volume Tremolo Rate**
    - MIDI Control Change 94 (*CC94*) allows for **Transposition**
