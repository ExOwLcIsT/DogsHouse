using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.UnitOfWork
{
    public interface IUnitOfWork
    {
        public DogsRepository Dogs { get; }
    }
}
