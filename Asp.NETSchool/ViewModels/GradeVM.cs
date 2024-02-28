using Asp.NETSchool.Models;

namespace Asp.NETSchool.ViewModels {
    public class GradeVM {
        public int Id { get; set; }
        public int SubjectId { get; set; }
        public int StudentId { get; set; }
        public string Topic { get; set; }
        public int Mark { get; set; }
        public DateTime Date { get; set; }
    }
}
