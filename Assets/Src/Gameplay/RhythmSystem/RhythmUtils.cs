using System.Collections.Generic;
using System.Linq;

namespace Gameplay.RhythmSystem
{
    [System.Serializable]
    public struct Signature
    {
        public uint top;
        public uint bottom;
        public NoteDuration beatNoteDuration;
        public uint maxSixteenthsOnOneMeasure;
        public Signature(uint top, uint bottom)
        {
            this.top = top;
            this.bottom = bottom;
            this.beatNoteDuration = (NoteDuration)bottom;
            if (bottom != 1 && bottom != 2 && bottom != 4 && bottom != 8 && bottom != 16)
            {
                UnityEngine.Debug.LogError($"Signature constructor can not create the signature, not valid bottom {bottom}");
            }
            uint SixteenthsOnOneBeat = 16 / bottom;
            this.maxSixteenthsOnOneMeasure = SixteenthsOnOneBeat * top; // Top corresponds to the number of beats in one measure
        }
    }

    // Really important this values for have a direct cast,
    // also the most fast note can not be dotted because we have no way to measure unless we add to the measures and callbacks other layer more
    // and doing that we are in the same state but one layer more deep
    public enum NoteDuration : uint
    {
        None = 0,
        Whole = 1,
        Half = 2,
        Quarter = 4,
        Eighth = 8,
        Sixteenth = 16
    }

    [System.Serializable]
    public struct Note
    {
        public NoteDuration duration;
        public bool isDottedNote;
        public bool isSilence;
        public uint durationInSixteenths;
        public Note(NoteDuration duration, bool isDottedNote = false, bool isSilence = false)
        {
            this.duration = duration;
            this.isDottedNote = isDottedNote;
            this.isSilence = isSilence;
            this.durationInSixteenths = (uint)duration;
        }
    }

    [System.Serializable]
    public struct TimesForBPMAndSignature
    {
        // All the times are in ms
        public double Whole;
        public double Half;
        public double Quarter;
        public double Eighth;
        public double Sixteenth;

        public double beat; // Pulso

        public TimesForBPMAndSignature(Signature signature, int bpm)
        {
            double BPM = bpm;

            beat = 60000 / BPM;

            Whole = beat * signature.bottom;
            Half = Whole / 2.0;
            Quarter = Whole / 4.0;
            Eighth = Whole / 8.0;
            Sixteenth = Whole / 16.0;

            UnityEngine.Debug.Log($"TIMES: {beat}, {Whole}, {Half}, {Quarter}, {Eighth}, {Sixteenth}");
        }
    }

    [System.Serializable]
    public struct RhythmPattern
    {
        public Signature signature;
        public List<Note> patternNotes;
        public int spaceInSixteenths;

        public RhythmPattern(Signature signature)
        {
            this.signature = signature;
            this.spaceInSixteenths = (int)signature.maxSixteenthsOnOneMeasure;
            this.patternNotes = new List<Note>();
        }

        public RhythmPattern(ref RhythmPattern copy)
        {
            signature = copy.signature; // Is a struct so in theory he is the value not the ref

            Note[] notes = new Note[copy.patternNotes.Count];
            copy.patternNotes.CopyTo(notes);
            patternNotes = notes.ToList<Note>();

            spaceInSixteenths = copy.spaceInSixteenths;
        }

        public uint GetIndexOfSixteenthOnMeasure(int noteIndex)
        {
            // Assume we pass a valid noteIndex
            uint numberOfSixteenths = 0;
            for (int i = 0; i < noteIndex; i++)
            {
                numberOfSixteenths += patternNotes[i].durationInSixteenths;
            }
            return numberOfSixteenths;
        }

        // Add one note to the pattern at the final returns if the note can be added (if the index is -1 (default) the note is added to the final
        public bool AddNote(Note note, int index = -1)
        {
            if (spaceInSixteenths - note.durationInSixteenths < 0 && index < -1)
            {
                return false;
            }

            spaceInSixteenths -= (int)note.durationInSixteenths;
            if (index == -1)
            {
                patternNotes.Add(note);
                return true;
            }

            patternNotes.Insert(index, note);
            return true;
        }

        public void RemoveNote(int index)
        {
            if (patternNotes.Count < index && index >= 0)
            {
                spaceInSixteenths += (int)patternNotes[index].durationInSixteenths;
                patternNotes.RemoveAt(index);
            }
        }
    }
}