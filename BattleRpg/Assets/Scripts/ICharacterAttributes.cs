namespace BattleRpg.Character
{
    /// <summary>
    /// Class that represents the base attributes of any character entity.
    /// </summary>
    public interface ICharacterAttributes
    {
        /// <summary>
        /// The health attribute of the character.
        /// </summary>
        float Health { get; }
        
        /// <summary>
        /// The current attack power attribute of the character.
        /// </summary>
        float AttackPower { get; }

        /// <summary>
        /// The current level attribute of the character.
        /// </summary>
        int Level { get; }
    }
}