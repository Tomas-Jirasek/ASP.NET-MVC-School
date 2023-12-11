using ASP.NETSchool.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETSchool.Services
{
    public class StudentService
    {
        private ApplicationDbContext _dbContext;

        public StudentService(ApplicationDbContext dbContext)
        {
            this._dbContext = dbContext;
        }

        public async Task<IEnumerable<Student>> GetAllAsync()   // IEnumerable rozhrani umoznuje pouzivat foreach
        {
            return await _dbContext.Students.ToListAsync();
        }
        public async Task CreateStudentAsync(Student student)
        {
            await _dbContext.Students.AddAsync(student);
            await _dbContext.SaveChangesAsync();
        }

        internal async Task<Student> GetById(int id)
        {
            return await _dbContext.Students.FirstOrDefaultAsync(student => student.Id == id);
        }

        internal async Task UpdateAsync(Student student)
        {
            _dbContext.Students.Update(student);
            await _dbContext.SaveChangesAsync();
        }

        internal async Task DeleteAsync(Student studentToDelete)
        {
            _dbContext.Students.Remove(studentToDelete);
            await _dbContext.SaveChangesAsync();
        }
    }
}
