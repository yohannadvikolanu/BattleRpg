namespace BattleRpg.Hero
{
    /// <inheritdoc/>
    internal sealed class HeroAttributes : IHeroAttributes
    {
        /// <inheritdoc/>
        public int ExperiencePoints { get; set; }

        /// <inheritdoc/>
        public float Health { get; set; }

        /// <inheritdoc/>
        public float AttackPower { get; set; }

        /// <inheritdoc/>
        public int Level { get; set; }
    }
}