using UnityEngine;


namespace Audio.SpecificImplementations
{
    public class CitadelMarchController : AModularMusicController
    {
        private int _part = -1;


        // TODO: Do the logic of the song changes
        protected override int[] InitClips()
        {
            return new int[] { 0 };
        }

        protected override int[] SelectNextClips(int[] songPartThatWillEnd)
        {
            _part = (_part + 1) % (_clipsRhythms.Count - 3);
            return new int[] { _part, (_part % 2 == 0)? _clipsRhythms.Count - 1 : _clipsRhythms.Count - 3 };
        }
    }
}