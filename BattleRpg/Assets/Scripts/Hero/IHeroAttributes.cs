using BattleRpg.Character;

namespace BattleRpg.Hero
{
    /// <summary>
    /// Class that represents the hero entity's attributes.
    /// </summary>
    public interface IHeroAttributes : ICharacterAttributes
    {
        /// <summary>
        /// The HeroType of the hero.
        /// </summary>
        HeroType HeroType { get; }
        
        /// <summary>
        /// The accumulated experience points for the hero.
        /// </summary>
        int ExperiencePoints { get; set; }

        /// <summary>
        /// Defines whether the hero has been unlocked for the player.
        /// </summary>
        bool Unlocked { get; set; }
    }
}