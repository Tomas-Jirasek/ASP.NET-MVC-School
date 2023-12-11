using ASP.NETSchool.Models;
using ASP.NETSchool.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace ASP.NETSchool.Services
{
    public class GradeService
    {
        private ApplicationDbContext _dbContext;

        public GradeService(ApplicationDbContext context)
        {
            this._dbContext = context;
        }

        public async Task<GradesDropdownsViewModel> GetDropdownValuesAsync()
        {
            return new GradesDropdownsViewModel()
            {
                Students = await _dbContext.Students.OrderBy(student => student.LastName).ToListAsync(),
                Subjects = await _dbContext.Subjects.OrderBy(subject => subject.Name).ToListAsync()
            };
        }

        internal async Task CreateAsync(GradeViewModel gradeViewModel)
        {
            var gradeToInsert = new Grade()
            {
                Student = _dbContext.Students.FirstOrDefault(student => student.Id == gradeViewModel.StudentId),
                Subject = _dbContext.Subjects.FirstOrDefault(subject => subject.Id == gradeViewModel.SubjectId),
                Date = DateTime.Today,
                Topic = gradeViewModel.Topic,
                Mark = gradeViewModel.Mark
            };
            if (gradeToInsert.Student != null && gradeToInsert.Subject != null)
            {
                await _dbContext.Grades.AddAsync(gradeToInsert);
                await _dbContext.SaveChangesAsync(); 
            }
        }
        internal async Task<IEnumerable<Grade>> GetAllGradesAsync()
        {
            return await _dbContext.Grades.Include(grade => grade.Student).Include(grade => grade.Subject).ToListAsync();
        }

        internal GradeViewModel GetById(int id)
        {
            var gradeToEdit = _dbContext.Grades.FirstOrDefault(grade => grade.Id == id);
            if (gradeToEdit != null)
            {
                return new GradeViewModel()
                {
                    SubjectId = gradeToEdit.Subject.Id,
                    StudentId = gradeToEdit.Student.Id,
                    Id = gradeToEdit.Id,
                    Mark = gradeToEdit.Mark,
                    Topic = gradeToEdit.Topic,
                    Date = gradeToEdit.Date
                };
            }
            return null;
        }

        internal async Task UpdateAsync(GradeViewModel updatedGrade)
        {
            var gradeToUpdate = _dbContext.Grades.FirstOrDefault(grade => grade.Id == updatedGrade.Id);
            if (gradeToUpdate != null)
            {
                gradeToUpdate.Subject = _dbContext.Subjects.FirstOrDefault(subject => subject.Id == updatedGrade.SubjectId);
                gradeToUpdate.Student = _dbContext.Students.FirstOrDefault(student => student.Id == updatedGrade.StudentId);
                gradeToUpdate.Topic = updatedGrade.Topic;
                gradeToUpdate.Mark = updatedGrade.Mark;
                //gradeToUpdate.Date = updatedGrade.Date;
            }
            _dbContext.Update(gradeToUpdate);
            await _dbContext.SaveChangesAsync();
        }
        internal async Task DeleteAsync(int id)
        {
            var gradeToDelete = _dbContext.Grades.FirstOrDefault(grade => grade.Id == id);
            if (gradeToDelete != null)
            {
                _dbContext.Remove(gradeToDelete);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
