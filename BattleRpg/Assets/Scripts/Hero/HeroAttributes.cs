namespace BattleRpg.Hero
{
    /// <inheritdoc/>
    internal sealed class HeroAttributes : IHeroAttributes
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Hero"/> class.
        /// </summary>
        /// <param name="name">The value for the name of the hero.</param>
        /// <param name="experiencePoints">The value for the total experience points of the hero.</param>
        /// <param name="health">The value for the total health of the hero.</param>
        /// <param name="attackPower">The value for the total attack power of the hero.</param>
        /// <param name="level">The value for the level of the hero.</param>
        public HeroAttributes(HeroType heroType, float health, float attackPower, int experiencePoints, int level)
        {
            HeroType = heroType;
            Health = health;
            AttackPower = attackPower;
            ExperiencePoints = experiencePoints;
            Level = level;
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
    }
}