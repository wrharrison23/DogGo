using DogGo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Repositories
{
    public interface INeighborhoodRepository
    {
       public List<Neighborhood> GetAll();
        public Neighborhood GetNeighborhoodById(int id);
    }
}
