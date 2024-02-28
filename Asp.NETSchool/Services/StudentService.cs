using Asp.NETSchool.Models;
using Microsoft.EntityFrameworkCore;

namespace Asp.NETSchool.Services {
    public class StudentService {
        private ApplicationDbContext dbContext; //nebudeme resit new, protoze se o to postara dependency injection container
        public StudentService(ApplicationDbContext dbContext) {
            this.dbContext = dbContext;
        }
        public async Task<IEnumerable<Student>> GetAllAsync() { //IEnumerable je rozhranni, ktere nam umoznuje pracovat s foreach
            return await dbContext.Students.ToListAsync();
        }
        public async Task CreateStudentAsync(Student student) {
            await dbContext.Students.AddAsync(student);
            await dbContext.SaveChangesAsync();
        }
        internal async Task<Student> GetById(int id) {
            return await dbContext.Students.FirstOrDefaultAsync(st =>st.Id ==id);
        }
        internal async Task UpdateAsync(Student student) {
            dbContext.Students.Update(student);
            await dbContext.SaveChangesAsync();
        }
        internal async Task DeleteAsync(Student studentToDelete) {
            dbContext.Students.Remove(studentToDelete);
            await dbContext.SaveChangesAsync();
        }
    }
}
