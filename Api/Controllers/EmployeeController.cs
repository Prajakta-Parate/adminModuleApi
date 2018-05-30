using Entity;
using Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Services;
using reactjsAPI.Models;
using System.Web;
using Newtonsoft.Json;
using System.Web.Http.Cors;

namespace Api.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class EmployeeController : ApiController
    {
        
        private readonly IEmployeeService _employeeService;
        public EmployeeController(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }


        public IEnumerable<Employee> GetAllProducts([FromUri]PagingParameterModel pagingparametermodel)

        {

            // Return List of Customer  
            var source = (from prod in _employeeService.GetAll().
                            OrderBy(a => a.Id)
                          select prod).AsQueryable();




            // ------------------------------------ Search Parameter-------------------   

            if (!string.IsNullOrEmpty(pagingparametermodel.QuerySearch))
            {
                source = source.Where(a => a.Name.Contains(pagingparametermodel.QuerySearch));
            }

            // ------------------------------------ Search Parameter-------------------  



            // Get's No of Rows Count   
            int count = source.Count();

            // Parameter is passed from Query string if it is null then it default Value will be pageNumber:1  
            int CurrentPage = pagingparametermodel.pageNumber;

            // Parameter is passed from Query string if it is null then it default Value will be pageSize:20  
            int PageSize = pagingparametermodel.pageSize;

            // Display TotalCount to Records to User  
            int TotalCount = count;

            // Calculating Totalpage by Dividing (No of Records / Pagesize)  
            int TotalPages = (int)Math.Ceiling(count / (double)PageSize);

            // Returns List of Customer after applying Paging   
            var items = source.Skip((CurrentPage - 1) * PageSize).Take(PageSize).ToList();

            // if CurrentPage is greater than 1 means it has previousPage  
            var previousPage = CurrentPage > 1 ? "Yes" : "No";

            // if TotalPages is greater than CurrentPage means it has nextPage  
            var nextPage = CurrentPage < TotalPages ? "Yes" : "No";

            // Object which we are going to send in header   
            var paginationMetadata = new
            {
                totalCount = TotalCount,
                pageSize = PageSize,
                currentPage = CurrentPage,
                totalPages = TotalPages,
                previousPage,
                nextPage,

                QuerySearch = string.IsNullOrEmpty(pagingparametermodel.QuerySearch) ?
                      "No Parameter Passed" : pagingparametermodel.QuerySearch

            };

            // Setting Header  
            HttpContext.Current.Response.Headers.Add("Paging-Headers", JsonConvert.SerializeObject(paginationMetadata));
            // Returing List of Customers Collections  
            return items;

        }

        public IHttpActionResult Get(int id)
        {
            var data = _employeeService.GetById(id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);

        }
        public IHttpActionResult PostEmployee(Employee item/*, HttpPostedFileBase file*/)
        {
            //Dictionary<string, object> dict = new Dictionary<string, object>();
            //  string str = files(file);
            //  item.Img = str;
            try
            {
                var httpRequest = HttpContext.Current.Request;
                item = _employeeService.Insert(item);
                _employeeService.Save();
                var response = Request.CreateResponse<Employee>(HttpStatusCode.Created, item);

                string uri = Url.Link("DefaultApi", new { id = item.Id });
                response.Headers.Location = new Uri(uri);
                return Ok();
            }
            catch
            {
                return InternalServerError();
            }
        }
        public IHttpActionResult PutEmployee(Employee emp)
        {

             //emp.Id = id;
            if (!_employeeService.Update(emp ,emp.Id))
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }
            _employeeService.Save();
            return Ok("Successful");
        }
        public void DeleteEmployee(int id)
        {

            Employee item = _employeeService.GetById(id);
            if (item == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            _employeeService.Delete(item);
            _employeeService.Save();
        }

    }
}
