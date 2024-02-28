using System.ComponentModel;

namespace Asp.NETSchool.Models {
    public class Student {
        public int Id { get; set; }
        [DisplayName("First name")]
        public string? FirstName { get; set; } //otaznik, znamena,ze ho muzu ulozit bez krestniho jmena
        public string? LastName { get; set;}
        public DateTime DateOfBirth { get; set; }
    }
}
