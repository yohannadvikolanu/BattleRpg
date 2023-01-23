namespace BattleRpg.Enemy
{
    internal sealed class EnemyAttributes : IEnemyAttributes
    {
        public EnemyAttributes()
        {
        }

        /// <inheritdoc/>
        public float Health { get; set; }

        /// <inheritdoc/>
        public float AttackPower { get; set; }

        /// <inheritdoc/>
        public int Level { get; set; }
    }
}