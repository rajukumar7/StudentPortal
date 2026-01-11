using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentPortal.Web.Data;
using StudentPortal.Web.Models;
using StudentPortal.Web.Models.Entities;

namespace StudentPortal.Web.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext dbContext;

        // Create constructor to inject ApplicationDbContext
        public StudentsController(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddStudentViewModel viewModel)
         {
            var student = new Student
            {
                Name = viewModel.Name,
                Email = viewModel.Email,
                Phone = viewModel.Phone,
                EnrollmentDate = viewModel.EnrollmentDate,
                IsActive = viewModel.IsActive
            };
            await dbContext.Tbl_Students.AddAsync(student);
            await dbContext.SaveChangesAsync();

            //return View();
            return RedirectToAction("List", "Students");
        }

        [HttpGet]
        public async Task<IActionResult> List()
        {
            var students = await dbContext.Tbl_Students.ToListAsync();
            return View(students);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id)
        {
            var student = await dbContext.Tbl_Students.FindAsync(id);
           
            return View(student);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Student viewModel)
        {
            var student = await dbContext.Tbl_Students.FindAsync(viewModel.Id);

            if(student != null)
            {
                student.Name = viewModel.Name;
                student.Email = viewModel.Email;
                student.Phone = viewModel.Phone;
                student.EnrollmentDate = viewModel.EnrollmentDate;
                student.IsActive = viewModel.IsActive;
                
                await dbContext.SaveChangesAsync();
                return RedirectToAction("List");
            }

            return RedirectToAction("List", "Students");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(Student viewModel)
        {
            var student = await dbContext.Tbl_Students
                .AsNoTracking() // It will not do any tracking of the entity
                .FirstOrDefaultAsync(x => x.Id == viewModel.Id);
            if(student != null)
            {
                dbContext.Tbl_Students.Remove(viewModel);
                await dbContext.SaveChangesAsync();
            }

            return RedirectToAction("List", "Students");

        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            var student = await dbContext.Tbl_Students.FindAsync(id);
            return View(student);
        }
    }
}
