using Asp.NETSchool.Models;
using Asp.NETSchool.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Reflection.Metadata.Ecma335;
using System.Xml;

namespace Asp.NETSchool.Controllers {
    public class FileUploadController : Controller {
        StudentService studentService;
        public FileUploadController(StudentService studentService) {
            this.studentService = studentService;
        }
        //pozor IFormFile se musi jmenovat stejne, jako v Create v inputu
        public async Task<IActionResult> UploadAsync(IFormFile file) {
            if (file.Length > 0) {
                string filePath = Path.GetFullPath(file.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create)) {
                    await file.CopyToAsync(stream);
                    stream.Close();
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(filePath);
                    XmlElement rootNode = xmlDocument.DocumentElement;
                    foreach (XmlNode node in rootNode.SelectNodes("/Students/Student")) {
                        Student newStudent = new Student() {
                            FirstName = node.ChildNodes[0].InnerText,
                            LastName = node.ChildNodes[1].InnerText,
                            DateOfBirth = DateTime.Parse(node.ChildNodes[2].InnerText, CultureInfo.CreateSpecificCulture("cs-CZ"))
                        };
                        await studentService.CreateStudentAsync(newStudent);
                    }
                }
                return RedirectToAction("Index", "Students");
            }
            return View("NotFound");
        }
    }
}
