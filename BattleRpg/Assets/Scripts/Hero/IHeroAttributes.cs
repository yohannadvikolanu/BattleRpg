using BattleRpg.Character;

namespace BattleRpg.Hero
{
    /// <summary>
    /// Class that represents the hero entity's attributes.
    /// </summary>
    public interface IHeroAttributes : ICharacterAttributes
    {        
        /// <summary>
        /// The accumulated experience points for the hero.
        /// </summary>
        int ExperiencePoints { get; set; }
    }
}