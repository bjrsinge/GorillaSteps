using GorillaSteps.Patches;

namespace GorillaSteps.Scripts
{
    [System.Serializable]
    internal class PlayerData
    {
        public int steps;

        internal PlayerData()
        {
            steps = HandTapPatch.steps_count;
        }
    }
}
