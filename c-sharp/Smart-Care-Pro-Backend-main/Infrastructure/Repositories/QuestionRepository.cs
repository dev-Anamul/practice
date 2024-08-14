using Domain.Entities;
using Infrastructure.Contracts;

/*
 * Created by   : 
 * Date created : 
 * Modified by  : Biplob Roy
 * Last modified: 30.07.2023
 * Reviewed by  :
 * Date reviewed:
 */
namespace Infrastructure.Repositories
{
    public class QuestionRepository : Repository<Question>, IQuestionRepository
    {
        public QuestionRepository(DataContext context) : base(context)
        {

        }

        /// <summary>
        /// The method is used to a get question by key.
        /// </summary>
        /// <param name="key">Primary key of the table BirthHistories.</param>
        /// <returns>Returns a question if the key is matched.</returns>
        public async Task<Question> GetQuestionByKey(int key)
        {
            try
            {
                return await FirstOrDefaultAsync(q => q.Oid == key && q.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get the list of questions.
        /// </summary>
        /// <returns>Returns a list of all questions.</returns>
        public async Task<IEnumerable<Question>> GetQuestions()
        {
            try
            {
                return await QueryAsync(q => q.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// The method is used to get an Question by Question Description.
        /// </summary>
        /// <param name="description">Name of an Question.</param>
        /// <returns>Returns an Question if the Question description is matched.</returns>
        public async Task<Question> GetQuestionByName(string question)
        {
            try
            {
                return await FirstOrDefaultAsync(a => a.Description.ToLower().Trim() == question.ToLower().Trim() && a.IsDeleted == false);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}