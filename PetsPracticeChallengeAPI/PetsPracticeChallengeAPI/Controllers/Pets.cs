using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using PetsPracticeChallengeAPI.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace PetsPracticeChallengeAPI.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class Pets : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public Pets(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }


        [HttpGet]
        [Route("get-all")]
        public JsonResult Get()
        {
            string query = @"
                            select petId, petName, species from
                            dbo.Pets
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("PetsDbConnectionString");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        [Route("add")]
        public JsonResult Post(Pet pet)
        {
            string query = @"
                            insert into dbo.Pets
                            (petName, species)
                            values (@PetName, @Species)
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("PetsDbConnectionString");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@PetName", pet.PetName);
                    myCommand.Parameters.AddWithValue("@Species", pet.Species);
                    
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }

        [HttpPut]
        [Route("update")]
        public JsonResult Put(Pet pet)
        {
            string query = @"
                            update dbo.Pets
                            set petName = @PetName, species = @Species
                            where petId = @PetId
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("PetsDbConnectionString");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@PetId", pet.PetId);
                    myCommand.Parameters.AddWithValue("@PetName", pet.PetName);
                    myCommand.Parameters.AddWithValue("@Species", pet.Species);                    
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Updated Successfully");
        }

        [HttpDelete("{id}")]
       
        public JsonResult Delete(int id)
        {
            string query = @"
                            delete from dbo.Pets                           
                            where petId = @PetId
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("PetsDbConnectionString");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@PetId", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Deleted Successfully");
        }




    }
}
