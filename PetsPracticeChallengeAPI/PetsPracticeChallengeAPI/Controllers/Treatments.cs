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
    public class Treatments : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IWebHostEnvironment _env;
        public Treatments(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
            _env = env;
        }

        [HttpGet]
        [Route("get/{id}")]
        public JsonResult Get(int procid)
        {
            string query = @"
                            SELECT treatmentId,petName, treatmentName, treatmentDesc 
                            FROM dbo.Procedures_ INNER JOIN dbo.Treatments ON @ProcId = procId_fk 
                            FULL JOIN dbo.Pets ON petId = petId_fk
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
            return new JsonResult(table);
        }

        //Get all of a pets treatments

        //[HttpGet]
        //[Route("get/{id}")]
        //public JsonResult Get()
        //{
        //    string query = @"
        //                    SELECT treatmentId, treatmentName, treatmentDesc 
        //                    FROM dbo.Procedures_, dbo.Treatments WHERE treatmentId = 
        //                    ";
        //    DataTable table = new DataTable();
        //    string sqlDataSource = _configuration.GetConnectionString("PetsDbConnectionString");
        //    SqlDataReader myReader;
        //    using (SqlConnection myCon = new SqlConnection(sqlDataSource))
        //    {
        //        myCon.Open();
        //        using (SqlCommand myCommand = new SqlCommand(query, myCon))
        //        {
        //            myReader = myCommand.ExecuteReader();
        //            table.Load(myReader);
        //            myReader.Close();
        //            myCon.Close();
        //        }
        //    }
        //    return new JsonResult(table);
        //}

        [HttpPost]
        [Route("{id}")]
        public JsonResult Post(int procid, int petid)
        {
            string query = @"
                            INSERT INTO dbo.Treatments (procId_fk, petId_treat_fk) VALUES
                            (@ProcId, @PetId)
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
                    myCommand.Parameters.AddWithValue("@PetId", petid);
                    

                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }
            return new JsonResult("Added Successfully");
        }

        [HttpDelete("{id}")]

        public JsonResult Delete(int treatmentid)
        {
            string query = @"
                            delete from dbo.Treatments                           
                            where treatmentId = @TreatmentId
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("PetsDbConnectionString");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@TreatmentId", treatmentid);
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
