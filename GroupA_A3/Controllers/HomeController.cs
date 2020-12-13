using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GroupA_A3.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

    

        public ActionResult AllEmployees()
        {
            List<Employee> employees = new List<Employee>();
            EmployeeEntities entities = new EmployeeEntities();
            employees = entities.Employees.OrderBy(k=>k.EmployeeID).ToList();
            return View(employees);
        }

        public ActionResult ViewEmployee(int employeeID)
        {
            EmployeeEntities entities = new EmployeeEntities();
            Employee e = entities.Employees.Where(k => k.EmployeeID == employeeID).FirstOrDefault();
            return View(e);
        }

        public ActionResult CreateEmployee()
        {
            using (var context = new EmployeeEntities())
            {
                List<Address> addresses = context.Addresses.Where(o => o.AddressType == 1).ToList();
                SelectList items = new SelectList(addresses, "AddressID", "Street");

                ViewBag.AllAddresses = items;

                List<Department> departments = context.Departments.ToList();
                SelectList itemsKP = new SelectList(departments, "DepartmentID", "DepartmentName");
                ViewBag.AllDepartments = itemsKP;

            }

            return View();
        }

        [HttpPost]
        public ActionResult CreateEmployee(Employee employee)
        {
            using (var context = new EmployeeEntities())
            {
                employee.HomeAddress.AddressName = "Home";
                employee.HomeAddress.AddressType = 2;
                context.Employees.Add(employee);
                context.SaveChanges();
            }
            
            return RedirectToAction("AllEmployees", new { employee.EmployeeID });
            
        }
        
        public ActionResult EditEmployee(int employeeId)
        {
            EmployeeEntities entities = new EmployeeEntities();
            Employee emp = entities.Employees.Where(k => k.EmployeeID == employeeId).FirstOrDefault();
            using (var context = new EmployeeEntities())
            {
                List<Address> addresses = context.Addresses.Where(k => k.AddressType == 1).ToList();
                SelectList items = new SelectList(addresses, "AddressID", "Street");
                ViewBag.AllAddresses = items;

                List<Department> departments = context.Departments.ToList();
                SelectList itemsKP = new SelectList(departments, "DepartmentID", "DepartmentName");
                ViewBag.AllDepartments = itemsKP;
            }
            return View(emp);
        }

        [HttpPost]
        public ActionResult EditEmployee(Employee employee)
        {
            using (var context = new EmployeeEntities())
            {
                Employee e1 = context.Employees.Where(x => x.EmployeeID == employee.EmployeeID).FirstOrDefault();
                e1.FirstName = employee.FirstName;
                e1.LastName = employee.LastName;
                e1.PhoneNumber = employee.PhoneNumber;
                e1.Salary = employee.Salary;
                e1.Title = employee.Title;
                e1.EmailAddress = employee.EmailAddress;
                e1.EmployeeDepartmentID = employee.EmployeeDepartmentID;
                e1.HomeAddress.Street = employee.HomeAddress.Street;
                e1.HomeAddress.PostalCode = employee.HomeAddress.PostalCode;
                e1.HomeAddress.City = employee.HomeAddress.City;
                e1.HomeAddress.Province = employee.HomeAddress.Province;
                e1.HomeAddress.AddressName = "Home";
                e1.HomeAddress.AddressType = 2;
                e1.LocationAddressID = employee.LocationAddressID;

                context.SaveChanges();
            }
            return RedirectToAction("ViewEmployee", new { employee.EmployeeID });
        }


        public ActionResult AddAddress()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddAddress(Address a1)
        {
            using (var context = new EmployeeEntities())
            {
                a1.AddressType = 1;
                a1.AddressName = "Office";
                context.Addresses.Add(a1);
                context.SaveChanges();

            }
            return RedirectToAction("AllEmployees", new { a1.AddressID });
        }

        public ActionResult DeleteEmployee(int employeeID)
        {
            //open new connection, update information and then close 
            using (var context = new EmployeeEntities())
            {
                // find the employee record in the database 
                Employee dbEmployee = context.Employees.Where(x => x.EmployeeID == employeeID).FirstOrDefault();

                // delete the record 
                context.Employees.Remove(dbEmployee);

                // save changes
                context.SaveChanges();
            }
            // redirect to all students
            return RedirectToAction("AllEmployees");
        }
    }
}