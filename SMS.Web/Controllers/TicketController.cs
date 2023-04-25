using Microsoft.AspNetCore.Mvc;
using SMS.Data.Entities;
using SMS.Web.Models;
using SMS.Data.Services;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SMS.Web.Controllers
{
    public class TicketController : BaseController
    {
        private readonly IStudentService svc;
        public TicketController()
        {
            svc = new StudentServiceDb();
        }

        // GET /ticket/index
        public IActionResult Index()
        {
            var tickets = svc.GetOpenTickets();
            return View(tickets);
        }
       
        //  POST /ticket/close/{id}
        [HttpPost]
        public IActionResult Close(int id)
        {
            // TBC - Q5 close ticket via service then check that ticket was closed
            var closed=svc.CloseTicket(id);
            // if not display a warning/error alert otherwise a success alert
            if (closed is not null)
            {
            Alert("Ticket closed successfuly", AlertType.success);
            return RedirectToAction(nameof(Index));
            }
            Alert("Ticket could not be closed", AlertType.warning);
            return RedirectToAction(nameof(Index));
        }       
        
        // GET /ticket/create
        public IActionResult Create()
        {
            // TBC Q5 - get list of students using service
            var tStudents=svc.GetStudents();
                       
            var tvm = new TicketViewModel { Students= new SelectList(tStudents,"Id", "Name")
                // TBC Q5 - populate select list property using list of students
               
            };

            // render blank form passing view model as a a parameter
            return View(tvm);
        }
       
        // POST /ticket/create
        [HttpPost]
        public IActionResult Create(TicketViewModel tvm)
        {
            // TBC - Q5 check if modelstate is valid and create ticket, display success alert and redirect to index
      
            if (ModelState.IsValid) 
            {
               var ticket=svc.CreateTicket(tvm.StudentId,tvm.Issue);
             Alert("Ticket opened succesfully", AlertType.success);
            return RedirectToAction(nameof(Index));     
            }           

            // TBC - Q6 before sending viewmodel back (due to validation issues)
            //       repopulate the select list
            var tStudents=svc.GetStudents();
            var select = new TicketViewModel { Students= new SelectList(tStudents,"Id", "Name")};
            return View(tvm);
            
        }
    }
}
