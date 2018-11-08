using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace VNASTWebsite.Controllers
{
    public class AssignmentController : Controller
    {
        // GET: Assignment
        public ActionResult Assignments()
        {
            try
            {
                string get_assignments = AccountController.apiRequestController.RequestGet("tasks");
                var assignments = JsonConvert.DeserializeObject<List<Models.Assignment>>(get_assignments);

                return View(assignments);
            }
            catch { }

            return View();
        }

        public ActionResult AddAssignment()
        {
            return View();
        }

        // POST: add new assigment
        [HttpPost]
        public ActionResult AddAssignment(Models.Assignment assignment)
        {
            bool update_data = AccountController.apiRequestController.AddAssignmentRequest(assignment);
            if (update_data)
            {
                return RedirectToAction("Assignments");
            }
            else
            {
                return View("Error");
            }
        }

        // PUT: Assignment
        public ActionResult EditAssignment(string id)
        {
            string get_assignments = AccountController.apiRequestController.RequestGet("tasks");
            var assignments = JsonConvert.DeserializeObject<List<Models.Assignment>>(get_assignments);
            var assignmentToEdit = assignments.Where(s => s._id == id).FirstOrDefault();
            return View(assignmentToEdit);
        }

        [HttpPost]
        public ActionResult EditAssignment(Models.Assignment assignment)
        {
            bool update_data = AccountController.apiRequestController.EditAssignmentRequest(assignment);
            if (update_data)
            {
                return RedirectToAction("Assignments");
            }
            else
            {
                return View("Error");
            }
        }

        // DELETE
        public ActionResult DeleteAssignment(string id)
        {
            bool delete_assignment = AccountController.apiRequestController.RequestDelete("tasks/" + id);
            if (delete_assignment)
            {
                return RedirectToAction("Assignments");
            }
            else
            {
                return View("Error");
            }
        }
    }
}