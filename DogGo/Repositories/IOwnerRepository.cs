using DogGo.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DogGo.Repositories
{
    public interface IOwnerRepository
    {
        List<Owner> GetAllOwners();
        Owner GetOwnerById(int id);
        public void AddOwner(Owner owner);
        public void UpdateOwner(Owner owner);
        public void DeleteOwner(int ownerId);
        public Owner GetOwnerByEmail(string email);
    }
}
