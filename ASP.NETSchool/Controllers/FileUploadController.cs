using ASP.NETSchool.Models;
using ASP.NETSchool.Services;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Xml;

namespace ASP.NETSchool.Controllers
{
    public class FileUploadController : Controller
    {
        private StudentService _studentService;

        public FileUploadController(StudentService studentService)
        {
            _studentService = studentService;
        }

        public async Task<IActionResult> UploadAsync(IFormFile file) // musi byt stejny nazev jako ve formu v create formu name="file"
        {
            if(file.Length > 0)
            {
                string filePath = Path.GetFullPath(file.FileName);
                using(var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                    stream.Close();
                    XmlDocument xmlDocument = new XmlDocument();
                    xmlDocument.Load(filePath);
                    XmlElement rootNode = xmlDocument.DocumentElement;
                    foreach(XmlNode node in rootNode.SelectNodes("/Students/Student"))
                    {
                        Student newStudent = new Student()
                        {
                            FirstName = node.ChildNodes[0].InnerText,
                            LastName = node.ChildNodes[1].InnerText,
                            DateOfBirth = DateTime.Parse(node.ChildNodes[2].InnerText, CultureInfo.CreateSpecificCulture("cs-cz"))  // specifikace jazyka kvuli Azure
                        };
                        await _studentService.CreateStudentAsync(newStudent);
                    }
                }
                return RedirectToAction("Index", "Students");
            }
            return View("NotFound");
        }
    }
}
