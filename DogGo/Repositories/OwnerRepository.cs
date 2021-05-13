using DogGo.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace DogGo.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly IConfiguration _config;

        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.
        public OwnerRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }

        public List<Owner> GetAllOwners()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Name], Address, NeighborhoodId, Phone
                        FROM Owner
                    ";

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Owner> owners = new List<Owner>();
                    while (reader.Read())
                    {
                        Owner owner = new Owner
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name")),
                            Address = reader.GetString(reader.GetOrdinal("Address")),
                            NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"))
                        };

                        owners.Add(owner);
                    }

                    reader.Close();

                    return owners;
                }
            }
        }

        public Owner GetOwnerById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Owner.Id as OwnerId, 
                        Email, 
                        Owner.Name as OwnerName, 
                        Address, NeighborhoodId, 
                        Phone, 
                        Dog.Id as DogId, 
                        Dog.Name as DogName, 
                        OwnerId, 
                        Breed, 
                        Notes, 
                        ImageUrl
                        FROM Owner JOIN Dog
                        ON Owner.Id = Dog.OwnerId
                        WHERE Owner.Id = @id;
                    ";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();
                    Owner owner = new Owner();
                    while (reader.Read())
                    {
                        if (owner.Name is null)
                        {
                            owner.Id = id;
                            owner.Name = reader.GetString(reader.GetOrdinal("OwnerName"));
                            owner.Address = reader.GetString(reader.GetOrdinal("Address"));
                            owner.NeighborhoodId = reader.GetInt32(reader.GetOrdinal("NeighborhoodId"));
                            owner.Phone = reader.GetString(reader.GetOrdinal("Phone"));
                        }
                        Dog dog = new Dog()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("DogId")),
                            Name = reader.GetString(reader.GetOrdinal("DogName")),
                            OwnerId = id,
                            Breed = reader.GetString(reader.GetOrdinal("Breed"))
                        };
                        owner.Dogs.Add(dog);
                    }
                    reader.Close();
                    return owner;
                }
            }
        }

        
    }
}
