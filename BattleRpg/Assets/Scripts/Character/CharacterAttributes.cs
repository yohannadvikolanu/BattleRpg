namespace BattleRpg.Character
{
    /// <inheritdoc/>
    internal sealed class CharacterAttributes : ICharacterAttributes
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CharacterAttributes"/> class.
        /// </summary>
        /// <param name="health">The value for the total health of the character.</param>
        /// <param name="attackPower">The value for the total attack power of the character.</param>
        /// <param name="level">The value for the level of the character.</param>
        public CharacterAttributes(float health, float attackPower, int level)
        {
            Health = health;
            AttackPower = attackPower;
            Level = level;
        }

        /// <inheritdoc/>
        public float Health { get; set; }

        /// <inheritdoc/>
        public float AttackPower { get; }

        /// <inheritdoc/>
        public int Level { get; }
    }
}