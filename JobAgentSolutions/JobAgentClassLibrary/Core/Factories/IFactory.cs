using JobAgentClassLibrary.Core.Repositories;

namespace JobAgentClassLibrary.Core.Factories
{
    public interface IFactory
    {
        /// <summary>
        /// Creates an instance of an entity.
        /// </summary>
        /// <param name="paramName">Uses the nameof keyword in order to swap between entities.</param>
        /// <param name="entityValues">A list of arguments providing the needed entity with data.</param>
        /// <exception cref="System.ArgumentException">Thrown when a param value couldn't be converted correctly.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">Thrown when a param name is out of range from the supported entities.</exception>
        /// <returns>An initialized entity.</returns>
        IAggregateRoot CreateEntity(string paramName, params object[] entityValues);
    }
}
