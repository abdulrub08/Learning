using LearnStepByStep.Models;
using LearnStepByStep.Models.DOM;
using LearnStepByStep.Models.Repositry;
using LearnStepByStep.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LearnStepByStep.Controllers
{
    [Authorize]
    [Route("Home")]
    //[Route("[controller]")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IWebHostEnvironment hostingEnvironment;
        public HomeController(ILogger<HomeController> logger, IEmployeeRepository employeeRepository, IWebHostEnvironment hostingEnvironment)
        {
            _logger = logger;
            _employeeRepository = employeeRepository;
            this.hostingEnvironment = hostingEnvironment;
        }

        //[Route("")]
        //[Route("Home")]
        //[Route("Home/Index")]
        //[Route("")]
        //[Route("Index")]
        //[Route("/")]
        //[Route("")]
        [Route("Index")]
        [AllowAnonymous]
        public IActionResult Index()
        {

            var model = _employeeRepository.GetAllEmployee();
            return View(model);
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("[action]")]
        public IActionResult Create(CreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;

                // If the Photos property on the incoming model object is not null and if count > 0,
                // then the user has selected at least one file to upload

                if (model.Photos != null && model.Photos.Count > 0)
                {
                    // Loop thru each selected file
                    foreach (IFormFile photo in model.Photos)
                    {
                        // The file must be uploaded to the images folder in wwwroot
                        // To get the path of the wwwroot folder we are using the injected
                        // IHostingEnvironment service provided by ASP.NET Core
                        string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                        // To make sure the file name is unique we are appending a new
                        // GUID value and and an underscore to the file name
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        // Use CopyTo() method provided by IFormFile interface to
                        // copy the file to wwwroot/images folder
                        photo.CopyTo(new FileStream(filePath, FileMode.Create));
                    }
                }

                Employee newEmployee = new Employee
                {
                    Name = model.Name,
                    Email = model.Email,
                    Department = model.Department,
                    PhotoPath = uniqueFileName
                };

                _employeeRepository.Add(newEmployee);
                return RedirectToAction("details", new { id = newEmployee.Id });
            }
            return View();
        }

        [HttpPost]
        [Route("CreateBackup")]
        public RedirectToActionResult Create(Employee employee)
        {
            Employee newEmployee = _employeeRepository.Add(employee);
            return RedirectToAction("details", new { id = newEmployee.Id });
        }
        
        [HttpGet]
        public ViewResult Edit(int id)
        {
            Employee employee = _employeeRepository.GetEmployee(id);
            EditViewModel employeeEditViewModel = new EditViewModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Email = employee.Email,
                Department = employee.Department,
                ExistingPhotoPath = employee.PhotoPath
            };
            return View(employeeEditViewModel);
        }
        // Through model binding, the action method parameter
        // EmployeeEditViewModel receives the posted edit form data
        [HttpPost]
        public IActionResult Edit(EditViewModel model)
        {
            // Check if the provided data is valid, if not rerender the edit view
            // so the user can correct and resubmit the edit form
            if (ModelState.IsValid)
            {
                string uniqueFileName = null;
                // Retrieve the employee being edited from the database
                Employee employee = _employeeRepository.GetEmployee(model.Id);
                // Update the employee object with the data in the model object
                employee.Name = model.Name;
                employee.Email = model.Email;
                employee.Department = model.Department;
                // If the user wants to change the photo, a new photo will be
                // uploaded and the Photo property on the model object receives
                // the uploaded photo. If the Photo property is null, user did
                // not upload a new photo and keeps his existing photo
                if (model.Photos != null && model.Photos.Count > 0)
                {
                    // Loop thru each selected file
                    foreach (IFormFile photo in model.Photos)
                    { 
                        // The file must be uploaded to the images folder in wwwroot
                        // To get the path of the wwwroot folder we are using the injected
                        // IHostingEnvironment service provided by ASP.NET Core
                        string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                        // To make sure the file name is unique we are appending a new
                        // GUID value and and an underscore to the file name
                        uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                        string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                        // Use CopyTo() method provided by IFormFile interface to
                        // copy the file to wwwroot/images folder
                        photo.CopyTo(new FileStream(filePath, FileMode.Create));

                    }

                    // Save the new photo in wwwroot/images folder and update
                    // PhotoPath property of the employee object which will be
                    // eventually saved in the database
                    employee.PhotoPath = ProcessUploadedFile(model);
                }

                // Call update method on the repository service passing it the
                // employee object to update the data in the database table
                Employee updatedEmployee = _employeeRepository.Update(employee);

                return RedirectToAction("index");
            }
            return View(model);
        }

        #region
       // public IActionResult Edit(EditViewModel model)
        //{
        //    // Check if the provided data is valid, if not rerender the edit view
        //    // so the user can correct and resubmit the edit form
        //    if (ModelState.IsValid)
        //    {
        //        // Retrieve the employee being edited from the database
        //        Employee employee = _employeeRepository.GetEmployee(model.Id);
        //        // Update the employee object with the data in the model object
        //        employee.Name = model.Name;
        //        employee.Email = model.Email;
        //        employee.Department = model.Department;
        //        // If the user wants to change the photo, a new photo will be
        //        // uploaded and the Photo property on the model object receives
        //        // the uploaded photo. If the Photo property is null, user did
        //        // not upload a new photo and keeps his existing photo
        //        if (model.Photos != null && model.Photos.Count > 0)
        //        {
                    
        //           // If a new photo is uploaded, the existing photo must be
        //            // deleted. So check if there is an existing photo and delete
        //            if (model.ExistingPhotoPath != null)
        //            {
        //                string filePath = Path.Combine(hostingEnvironment.WebRootPath,
        //                    "images", model.ExistingPhotoPath);
        //                System.IO.File.Delete(filePath);
        //            }
        //            // Save the new photo in wwwroot/images folder and update
        //            // PhotoPath property of the employee object which will be
        //            // eventually saved in the database
        //            employee.PhotoPath = ProcessUploadedFile(model);
        //        }

        //        // Call update method on the repository service passing it the
        //        // employee object to update the data in the database table
        //        Employee updatedEmployee = _employeeRepository.Update(employee);

        //        return RedirectToAction("index");
        //    }
        //    return View(model);
        //}
        #endregion
        [AllowAnonymous]
        //[Route("Home/Details/{id}")]
        [Route("Details/{id?}")]
        //[Route("Home/Details/{id?}")]
        public ViewResult Details(int? id)
        {
            HomeDetailsViewModel homeDetailsViewModel = new HomeDetailsViewModel()
            {
                Employee = _employeeRepository.GetEmployee(id ?? 1),
                PageTitle = "Employee Details"
            };

            return View(homeDetailsViewModel);
        }

        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new Models.DOM.ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        #region ProcessUploadedFile
        private string ProcessUploadedFile(CreateViewModel model)
        {
            string uniqueFileName = null;

            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }
        #endregion
    }
}
