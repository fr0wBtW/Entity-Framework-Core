using _1.EntityFrameworkExervcise.Data.Models;
using System;
using System.Text;
using System.Linq;
using System.Globalization;


namespace SoftUniDatabase
{
    public class Startup
    {
        static void Main(string[] args)
        {
            var context = new SoftUniEFContext();
            // Console.WriteLine(GetEmployeesFullInformation(context));
            //Console.WriteLine(GetEmployeesWithSalaryOver50000(context));
            // Console.WriteLine(GetEmployeesFromResearchAndDevelopment(context));
            //Console.WriteLine(AddNewAddressToEmployees(context));
            //Console.WriteLine(GetEmployeesInPeriod(context));
            //Console.WriteLine(GetAddressesByTown(context));
            //Console.WriteLine(GetEmployee147(context));
            //Console.WriteLine(GetDepartmentsWithMoreThan5Employees(context));
            //Console.WriteLine(GetLatestProject(context));
            //Console.WriteLine(IncreaseSalaries(context));
            //Console.WriteLine(GetEmployeesByFirstNameStartingWithSa(context));
            //Console.WriteLine(DeleteProjectById(context));
            Console.WriteLine(RemoveTown(context));
        }

        //3.Employees Full Information
        public static string GetEmployeesFullInformation(SoftUniEFContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees.OrderBy(e => e.EmployeeId)
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    e.MiddleName,
                    e.JobTitle,
                    e.Salary
                }).ToList();

            foreach (var e in employees)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.MiddleName} {e.JobTitle} {e.Salary:F2}");
            }
            return sb.ToString().TrimEnd();
        }

        //4.Employees with Salary Over 50 000
        public static string GetEmployeesWithSalaryOver50000(SoftUniEFContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employeesWithSalaryOver = context.Employees.OrderBy(e => e.FirstName).Where(e => e.Salary > 50000).Select(e => new
            {
                e.FirstName,
                e.Salary
            }).ToList();

            foreach (var e in employeesWithSalaryOver)
            {
                sb.AppendLine($"{e.FirstName} - {e.Salary:F2}");
            }
            return sb.ToString();
        }

        //5.Employees from Research and Development
        public static string GetEmployeesFromResearchAndDevelopment(SoftUniEFContext context)
        {
            StringBuilder result = new StringBuilder();

            var researchAndDevelopmentDepartmentEmployees = context.Employees.Where(e => e.Department.Name == "Research And Development")
                .Select(e => new
                {
                    e.FirstName,
                    e.LastName,
                    DepartmentName = e.Department.Name,
                    e.Salary
                }).OrderBy(e => e.Salary).ThenByDescending(e => e.FirstName).ToList();

            foreach (var e in researchAndDevelopmentDepartmentEmployees)
            {
                result.AppendLine($"{e.FirstName} {e.LastName} from {e.DepartmentName} - {e.Salary:f2}");
            }
            return result.ToString();
        }

        //6.Adding a New Address and Updating Employee
        public static string AddNewAddressToEmployees(SoftUniEFContext context)
        {
            StringBuilder sb = new StringBuilder();

            Address newAddress = new Address()
            {
                AddressText = "Vitoshka 15",
                TownId = 4,
            };

            Employee employeeNakov = context.Employees.First(e => e.LastName == "Nakov");

            context.Addresses.Add(newAddress);
            employeeNakov.Address = newAddress;
            context.SaveChanges();

            var addresses = context.Employees.OrderByDescending(e => e.AddressId).Take(10).Select(e => e.Address.AddressText).ToList();

            foreach (var address in addresses)
            {
                sb.AppendLine(address);
            }
            return sb.ToString().TrimEnd();
        }

        //7.Employees and Projects
        public static string GetEmployeesInPeriod(SoftUniEFContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employees = context.Employees.Where(e => e.EmployeesProjects.Any(ep => ep.Project.StartDate.Year > 2001 &&
            ep.Project.StartDate.Year <= 2003)).Take(10).Select(e => new
            {
                e.FirstName,
                e.LastName,
                ManagerFirstName = e.Manager.FirstName,
                ManagerLastName = e.Manager.LastName,
                Projects = e.EmployeesProjects.Select(ep => new
                {
                    ProjectName = ep.Project.Name,
                    StartDate = ep.Project.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture),
                    EndDate = ep.Project.EndDate.HasValue ? ep.Project.EndDate.Value.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture) : "not finished"
                }).ToList()
            }).ToList();

            foreach (var employee in employees)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - Manager: {employee.ManagerFirstName} {employee.ManagerLastName}");

                foreach (var project in employee.Projects)
                {
                    sb.AppendLine($"--{project.ProjectName} - {project.StartDate} - {project.EndDate}");
                }
            }
            return sb.ToString().TrimEnd();
        }

        //8. Addresses by Town
        public static string GetAddressesByTown(SoftUniEFContext context)
        {
            StringBuilder sb = new StringBuilder();

            var addresses = context.Addresses.OrderByDescending(e => e.Employees.Count()).ThenBy(t => t.Town.Name)
                .ThenBy(a => a.AddressText)
                .Take(10).Select(a => new
                {
                    a.AddressText,
                    TownName = a.Town.Name,
                    EmployeeCount = a.Employees.Count,
                }).ToList();

            foreach (var address in addresses)
            {
                sb.AppendLine($"{address.AddressText}, {address.TownName} - {address.EmployeeCount} employees");
            }
            return sb.ToString();
        }

        //9.Employee 147
        public static string GetEmployee147(SoftUniEFContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employee147 = context.Employees.Where(e => e.EmployeeId == 147).Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.JobTitle,
                Projects = e.EmployeesProjects.Select(ep => ep.Project.Name).OrderBy(pn => pn).ToList()
            }).Single();

            sb.AppendLine($"{employee147.FirstName} {employee147.LastName} - {employee147.JobTitle}");

            foreach (var projects in employee147.Projects)
            {
                sb.AppendLine(projects);
            }
            return sb.ToString();
        }

        //10.Departments with More Than 5 Employees
        public static string GetDepartmentsWithMoreThan5Employees(SoftUniEFContext context)
        {
            StringBuilder sb = new StringBuilder();

            var departmentsWithMoreThan5Employees = context.Departments.Where(e => e.Employees.Count > 5).OrderBy(e => e.Employees.Count)
                .ThenBy(d => d.Name).Select(d => new
                {
                    DepartmentName = d.Name,
                    ManagerFirstName = d.Manager.FirstName,
                    ManagerLastName = d.Manager.LastName,
                    EmployeeInfo = d.Employees.Select(e => new
                    {
                        e.FirstName,
                        e.LastName,
                        e.JobTitle
                    }).OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList()
                }).ToList();

            foreach (var d in departmentsWithMoreThan5Employees)
            {
                sb.AppendLine($"{d.DepartmentName} - {d.ManagerFirstName} {d.ManagerLastName}");

                foreach (var e in d.EmployeeInfo)
                {
                    sb.AppendLine($"{e.FirstName} {e.LastName} - {e.JobTitle}");
                }
            }
            return sb.ToString();
        }

        //11.Find Latest 10 Projects
        public static string GetLatestProject(SoftUniEFContext context)
        {
            StringBuilder sb = new StringBuilder();

            var latest10Projects = context.Projects.Take(10).OrderBy(p => p.StartDate.Date).OrderBy(p => p.Name).Select(p => new
            {
                p.Name,
                p.Description,
                StartDate = p.StartDate.ToString("M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture)
            }).ToList();

            foreach (var p in latest10Projects)
            {
                sb.AppendLine(p.Name);
                sb.AppendLine(p.Description);
                sb.AppendLine(p.StartDate);
            }
            return sb.ToString();
        }

        //12.Increase Salaries
        public static string IncreaseSalaries(SoftUniEFContext context)
        {
            StringBuilder sb = new StringBuilder();

            IQueryable<Employee> employeesToIncrease = context.Employees.Where(d => d.Department.Name == "Engineering" || d.Department.Name == "Tool Design" ||
            d.Department.Name == "Marketing" || d.Department.Name == "Information Services");

            foreach (var employee in employeesToIncrease)
            {
                employee.Salary += employee.Salary * 0.12m;
            }
            context.SaveChanges();

            var employeeInfo = employeesToIncrease.Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.Salary
            }).OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList();

            foreach (var e in employeeInfo)
            {
                sb.AppendLine($"{e.FirstName} {e.LastName} {e.Salary:f2}");
            }
            return sb.ToString();
        }

        //13.Find Employees by First Name Starting with "Sa"
        public static string GetEmployeesByFirstNameStartingWithSa(SoftUniEFContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employeesWithSA = context.Employees.Where(e => e.FirstName.StartsWith("SA")).Select(e => new
            {
                e.FirstName,
                e.LastName,
                e.JobTitle,
                e.Salary
            }).OrderBy(e => e.FirstName).ThenBy(e => e.LastName).ToList();

            foreach (var employee in employeesWithSA)
            {
                sb.AppendLine($"{employee.FirstName} {employee.LastName} - {employee.JobTitle} - ({employee.Salary})");
            }
            return sb.ToString().TrimEnd();
        }

        //14.Delete Project by Id
        public static string DeleteProjectById(SoftUniEFContext context)
        {
            StringBuilder sb = new StringBuilder();

            var employeeeProject = context.EmployeesProjects.Where(ep => ep.ProjectId == 2);

            var deleteProject = context.Projects.Find(2);
            context.Projects.Remove(deleteProject);

            foreach (var ep in employeeeProject)
            {
                context.EmployeesProjects.Remove(ep);
            }
            context.SaveChanges();

            var projects = context.Projects.Take(10).Select(p => p.Name).ToList();

            foreach (var p in projects)
            {
                sb.AppendLine($"{p}");
            }

            return sb.ToString();
        }

        //15.Remove Town
        public static string RemoveTown(SoftUniEFContext context)
        {
            Town townToDelete = context.Towns.First(t => t.Name == "Seattle");

            IQueryable<Address> addressToDelete = context.Addresses.Where(t => t.TownId == townToDelete.TownId);

            int addressCount = addressToDelete.Count();

            IQueryable<Employee> employeeAddressToNull = context.Employees.Where(e => addressToDelete.Any(a => a.AddressId == e.AddressId));

            foreach (var e in employeeAddressToNull)
            {
                e.AddressId = null;
            }
            foreach (Address a in addressToDelete)
            {
                context.Addresses.Remove(a);
            }
            context.Towns.Remove(townToDelete);
            context.SaveChanges();

            return $"{addressCount} addresses in {townToDelete.Name} were deleted.";
        }
    }
}
  