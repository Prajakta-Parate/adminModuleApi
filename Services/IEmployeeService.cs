using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entity;

namespace Services
{
    public interface IEmployeeService
    {
        List<Employee> GetAll();
        Employee GetById(Int64 id);
        Employee Insert(Employee model);
        bool Update(Employee model,Int64 id);
        void Delete(Employee model);
        void Save();

    }
}
