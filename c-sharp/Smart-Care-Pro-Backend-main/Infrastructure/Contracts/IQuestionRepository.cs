using Domain.Entities;

/*
 * Created by   : 
 * Date created : 
 * Modified by  : Biplob Roy
 * Last modified: 30.07.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Contracts
{
    public interface IQuestionRepository : IRepository<Question>
    {
        /// <summary>
        /// The method is used to get a question by key.
        /// </summary>
        /// <param name="key">Primary key of the table Questions.</param>
        /// <returns>Returns a question if the key is matched.</returns>
        public Task<Question> GetQuestionByKey(int key);

        /// <summary>
        /// The method is used to get the list of question.
        /// </summary>
        /// <returns>Returns a list of all question.</returns>
        public Task<IEnumerable<Question>> GetQuestions();

        /// <summary>
        /// The method is used to get an Question by Question Description.
        /// </summary>
        /// <param name="question">Description of an Question.</param>
        /// <returns>Returns an Question if the Question name is matched.</returns>
        public Task<Question> GetQuestionByName(string question);
    }
}