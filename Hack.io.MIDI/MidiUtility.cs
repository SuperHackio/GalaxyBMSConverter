namespace Hack.io.MIDI;

public static class MidiUtility
{
    const string _Notes = "C C#D D#E F F#G G#A A#B ";
    /// <summary>
    /// Converts a MIDI note id into a string note representation
    /// </summary>
    /// <param name="noteId">The note id (0-127)</param>
    /// <param name="withOctave">Indicates whether or not the octave should be returned</param>
    /// <returns>The string note</returns>
    public static string NoteIdToNote(byte noteId, bool withOctave = true)
    {
        noteId = unchecked((byte)(noteId & 0x7F));
        if (withOctave)
            return _Notes.Substring((noteId % 12) * 2, 2).TrimEnd() + ((int)(noteId / 12)).ToString();
        return _Notes.Substring((noteId % 12) * 2, 2).TrimEnd();
    }
    /// <summary>
    /// Converts a string representation of a note to a MIDI note id
    /// </summary>
    /// <param name="note">The note</param>
    /// <returns>A MIDI note id</returns>
    public static byte NoteToNoteId(string note)
    {
        ArgumentNullException.ThrowIfNull(note);
        if (0 == note.Length)
            throw new ArgumentException("The note must not be empty", "note");
        var bn = "";
        for (var i = 0; i < note.Length; ++i)
        {
            var ch = note[i];
            if (!char.IsLetter(ch) && '#' != ch)
                break;
            bn += ch.ToString().ToUpperInvariant();
        }
        if (0 == bn.Length || 2 < bn.Length || '#' == bn[0])
            throw new ArgumentException("Not a valid note", "note");
        var j = _Notes.IndexOf(bn);
        if (0 > j)
            throw new ArgumentException("Note a valid note", "note");
        var oct = 5;
        if (note.Length > bn.Length)
        {
            var num = note.Substring(bn.Length);
            if (!int.TryParse(num, out oct))
                throw new ArgumentException("Note a valid note", "note");
            if (10 < oct)
                throw new ArgumentException("Note a valid note", "note");
        }
        return unchecked((byte)(12 * oct + (j / 2)));
    }
    /// <summary>
    /// Converts a microtempo to a tempo
    /// </summary>
    /// <param name="microTempo">The microtempo</param>
    /// <returns>The tempo</returns>
    public static double MicroTempoToTempo(int microTempo)
    {
        return 60000000 / ((double)microTempo);
    }
    /// <summary>
    /// Converts a tempo to a microtempo
    /// </summary>
    /// <param name="tempo">The tempo</param>
    /// <returns>The microtempo</returns>
    public static int TempoToMicroTempo(double tempo)
    {
        return (int)(500000 * (120d / tempo));
    }
}
