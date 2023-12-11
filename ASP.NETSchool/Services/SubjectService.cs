using ASP.NETSchool.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETSchool.Services
{
    public class SubjectService
    {
        private ApplicationDbContext _dbContext;
        
        public SubjectService(ApplicationDbContext context)
        {
            this._dbContext = context;
        }
        public async Task<IEnumerable<Subject>> GetAllAsync()   // IEnumerable rozhrani umoznuje pouzivat foreach
        {
            return await _dbContext.Subjects.ToListAsync();
        }
        public async Task CreateSubjectAsync(Subject subject)
        {
            await _dbContext.Subjects.AddAsync(subject);
            await _dbContext.SaveChangesAsync();
        }

        internal async Task<Subject> GetById(int id)
        {
            return await _dbContext.Subjects.FirstOrDefaultAsync(subject => subject.Id == id);
        }

        internal async Task UpdateAsync(Subject subject)
        {
            _dbContext.Subjects.Update(subject);
            await _dbContext.SaveChangesAsync();
        }

        internal async Task DeleteAsync(Subject subjectToDelete)
        {
            _dbContext.Subjects.Remove(subjectToDelete);
            await _dbContext.SaveChangesAsync();
        }
    }
}
