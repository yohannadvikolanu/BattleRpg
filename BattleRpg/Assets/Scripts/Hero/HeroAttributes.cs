namespace BattleRpg.Hero
{
    /// <inheritdoc/>
    internal sealed class HeroAttributes : IHeroAttributes
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Hero"/> class.
        /// </summary>
        /// <param name="heroType">The value for the heroType of the hero.</param>
        /// <param name="experiencePoints">The value for the total experience points of the hero.</param>
        /// <param name="health">The value for the total health of the hero.</param>
        /// <param name="attackPower">The value for the total attack power of the hero.</param>
        /// <param name="level">The value for the level of the hero.</param>
        /// <param name="unlocked">The value indicating whether the hero has been unlocked.</param>
        public HeroAttributes(HeroType heroType, float health, float attackPower, int experiencePoints, int level, bool unlocked)
        {
            HeroType = heroType;
            Health = health;
            AttackPower = attackPower;
            ExperiencePoints = experiencePoints;
            Level = level;
            Unlocked = unlocked;
        }

        /// <inheritdoc/>
        public HeroType HeroType { get; }

        /// <inheritdoc/>
        public int ExperiencePoints { get; set; }

        /// <inheritdoc/>
        public float Health { get; set; }

        /// <inheritdoc/>
        public float AttackPower { get; }

        /// <inheritdoc/>
        public int Level { get; }

        /// <inheritdoc/>
        public bool Unlocked { get; set; }
    }
}