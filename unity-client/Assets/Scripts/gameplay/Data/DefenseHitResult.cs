namespace PowerPrank3D.Gameplay
{
    public struct DefenseHitResult
    {
        public bool wasBlocked;
        public bool brokeDefense;
        public bool activatedDefense;
        public bool weaknessApplied;

        public float breakdownMultiplier;
        public float reactionMultiplier;

        public string popupText;

        public static DefenseHitResult Default()
        {
            return new DefenseHitResult
            {
                wasBlocked = false,
                brokeDefense = false,
                activatedDefense = false,
                weaknessApplied = false,
                breakdownMultiplier = 1f,
                reactionMultiplier = 1f,
                popupText = string.Empty
            };
        }
    }
}
