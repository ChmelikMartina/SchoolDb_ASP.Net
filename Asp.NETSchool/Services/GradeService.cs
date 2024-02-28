using Asp.NETSchool.Models;
using Asp.NETSchool.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace Asp.NETSchool.Services {
    public class GradeService {
        private ApplicationDbContext dbContext;
        public GradeService(ApplicationDbContext dbContext) {
            this.dbContext = dbContext;
        }
        public async Task<GradesDropdownsVM> GetDropdownValuesAsync() {
            return new GradesDropdownsVM() {
                Students = await dbContext.Students.OrderBy(x => x.LastName).ToListAsync(),
                Subjects = await dbContext.Subjects.ToListAsync(),
            };
        }
        internal async Task CreateAsync(GradeVM gradeModel) {
            var gradeToInsert = new Grade() {
                Student = dbContext.Students.FirstOrDefault(s => s.Id == gradeModel.StudentId),
                Subject = dbContext.Subjects.FirstOrDefault(sub => sub.Id == gradeModel.SubjectId),
                Date = DateTime.Today,
                Topic = gradeModel.Topic,
                Mark = gradeModel.Mark
            };
            if (gradeToInsert.Student != null && gradeToInsert.Subject != null) {
                await dbContext.Grades.AddAsync(gradeToInsert);
                await dbContext.SaveChangesAsync();
            }
        }
        internal async Task<IEnumerable<Grade>> GetAllGrades() {
            return await dbContext.Grades.Include(gr=>gr.Student).Include(gr=>gr.Subject).ToListAsync();
        }
        internal GradeVM GetById(int id) {
            var gradeToEdit = dbContext.Grades.FirstOrDefault(gr=>gr.Id == id);
            GradeVM gradeViewModel = new GradeVM();
            if (gradeToEdit != null) {
                gradeViewModel.SubjectId = gradeToEdit.Subject.Id;
                gradeViewModel.StudentId = gradeToEdit.Student.Id;
                gradeViewModel.Id = gradeToEdit.Id;
                gradeViewModel.Mark = gradeToEdit.Mark;
                gradeViewModel.Date = gradeToEdit.Date;
                gradeViewModel.Topic = gradeToEdit.Topic;
            }
            return gradeViewModel;
        }
        internal async Task UpdateAsync(GradeVM updatedGrade) {
            var gradeToUpdate = dbContext.Grades.FirstOrDefault(gr=>gr.Id==updatedGrade.Id);
            if (gradeToUpdate != null) {
                gradeToUpdate.Subject = dbContext.Subjects.FirstOrDefault(sub => sub.Id == updatedGrade.SubjectId);
                gradeToUpdate.Student = dbContext.Students.FirstOrDefault(stu => stu.Id == updatedGrade.StudentId);
                gradeToUpdate.Topic = updatedGrade.Topic;
                gradeToUpdate.Mark = updatedGrade.Mark;
                //gradeToUpdate.Date = updatedGrade.Date;
            }
            dbContext.Update(gradeToUpdate);
            await dbContext.SaveChangesAsync();
        }
        internal async Task DeleteAsync (int id) {
            var gradeToDelete = dbContext.Grades.FirstOrDefault(gr => gr.Id == id);
            if (gradeToDelete != null) {
                dbContext.Remove(gradeToDelete);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
