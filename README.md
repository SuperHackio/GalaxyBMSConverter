# GalaxyBMSConverter
Convert MIDI files into Galaxy BMS files!



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