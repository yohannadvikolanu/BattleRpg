using BattleRpg.Character;

namespace BattleRpg.Enemy
{
    internal sealed class EnemyAttributes : IEnemyAttributes, ICharacterAttributes
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Enemy"/> class.
        /// </summary>
        /// <param name="health">The value for the total health of the enemy.</param>
        /// <param name="attackPower">The value for the total attack power of the enemy.</param>
        /// <param name="level">The value for the level of the enemy.</param>
        public EnemyAttributes(float health, float attackPower, int level)
        {
            Health = health;
            AttackPower = attackPower;
            Level = level;
        }

        /// <inheritdoc/>
        public float Health { get; }

        /// <inheritdoc/>
        public float AttackPower { get; }

        /// <inheritdoc/>
        public int Level { get; }
    }
}