namespace BattleRpg.Character
{
    /// <inheritdoc/>
    internal sealed class CharacterAttributes : ICharacterAttributes
    {
        public CharacterAttributes()
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