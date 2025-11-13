using DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public UnitOfWork(DogsRepository dogsRepository)
        {
            Dogs = dogsRepository;
        }

        public DogsRepository Dogs { get; }

    }
}
