

using System.Diagnostics;

namespace Gameplay.RhythmSystem
{
    public struct Signature
    {
        public uint top;
        public uint bottom;
        public NoteDuration bottomInDuration;
        public Signature(uint top, uint bottom)
        {
            this.top = top;
            this.bottom = bottom;
            switch(bottom)
            {
                case 1:
                    this.bottomInDuration = NoteDuration.Whole;
                    break;
                case 2:
                    this.bottomInDuration = NoteDuration.Half;
                    break;
                case 4:
                    this.bottomInDuration = NoteDuration.Quarter;
                    break;
                case 8:
                    this.bottomInDuration = NoteDuration.Eighth;
                    break;
                case 16:
                    this.bottomInDuration = NoteDuration.Sixteenth;
                    break;
                default:
                    UnityEngine.Debug.LogError($"The Bottom value is not valid: {bottom}");
                    this.bottomInDuration = NoteDuration.None;
                    break;
            }
        }
    }

    public enum NoteDuration
    {
        None = 0,
        Whole,
        Half,
        Quarter,
        Eighth,
        Sixteenth
    }

    public struct Note
    {
        public NoteDuration duration;
        public bool isDottedNote;
        public bool isSilence;
        public Note(NoteDuration duration, bool isDottedNote = false, bool isSilence = false)
        {
            this.duration = duration;
            this.isDottedNote = isDottedNote;
            this.isSilence = isSilence;
        }
    }

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
        }
    }
}