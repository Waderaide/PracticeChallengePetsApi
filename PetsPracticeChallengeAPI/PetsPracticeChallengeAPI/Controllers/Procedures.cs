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
    public class Procedures : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public Procedures(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }
        [HttpGet]
        [Route("get/{id:int}")]
        public JsonResult Get(int petid)
        {
            string query = @"
                            select p.petName, PO.procName, PO.procDesc from Treatments as T
                            inner join Procedures_ as PO on po.procId = T.procId_fk
                            inner join Pets as P on P.petId = T.petId_treat_fk
                            where p.petId = @PetId
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("PetsDbConnectionString");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@PetId", petid);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult(table);
        }

        [HttpPost]
        [Route("/add")]
        public JsonResult Post(Procedure procedures)
        {
            string query = @"
                            INSERT INTO dbo.Procedures_ (procName, procDesc) VALUES
                            (@ProcName, @ProcDesc)
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("PetsDbConnectionString");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    
                    myCommand.Parameters.AddWithValue("@ProcName", procedures.ProcName);
                    myCommand.Parameters.AddWithValue("@ProcDesc", procedures.ProcDesc);

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }

        [HttpDelete("{id:int}")]

        public JsonResult Delete(int procid)
        {
            string query = @"
                            delete from dbo.Procedures_                           
                            where procId = @ProcId
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("PetsDbConnectionString");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@ProcId", procid);
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
