namespace Platformer.GlobalData
{
    public class GlobalStrings
    {
        public AudioSources AudioSourceNames { get; }

        public Tags TagNames { get; set; }
    }

    public struct AudioSources
    {
        public const string CrystalMining1_wav = "Crystal Mining 1";
        public const string CrystalMining2_wav = "Crystal Mining 2";
        public const string CrystalMining3_wav = "Crystal Mining 3";
        public const string AIMinorLoop_wav_3d = "AI Minor Loop";
    }

    public struct Tags
    {

    }
}
