namespace BattleRpg.Hero
{
    /// <summary>
    /// Class that represents the hero entity's attributes.
    /// </summary>
    public interface IHeroAttributes
    {
        /// <summary>
        /// The Name of the hero.
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// The accumulated experience points for the hero.
        /// </summary>
        int ExperiencePoints { get; }
    }
}