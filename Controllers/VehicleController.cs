using Assignment3.Context;
using Assignment3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Assignment3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VehicleController : ControllerBase
    {
        private readonly IMapperSessionVehicle session;
        private readonly IMapperSessionContainer containerSession;
        public VehicleController(IMapperSessionVehicle session, IMapperSessionContainer containerSession)
        {
            this.session = session;
            this.containerSession = containerSession;
        }
        //vehicle add method
        [HttpGet]
        public List<Vehicle> Get() 
        {
            List<Vehicle> result = session.vehicle.ToList();
            return result;
        }


        [HttpGet("{id}")]
        public Vehicle Get(long id)
        {
            Vehicle result = session.vehicle.Where(x => x.id == id).FirstOrDefault();
            return result;
        }
        //vehicle add method
        [HttpPost]
        public void Post([FromBody] Vehicle vehicle) 
        {
            try
            {
                session.BeginTransaction();
                session.Save(vehicle);
                session.Commit();
            }
            catch (Exception ex)
            {
                session.Rollback();
                Log.Error(ex, "Insert Error");
            }
            finally
            {
                session.CloseTransaction();
            }
        }
        //container update method
        [HttpPut]
        public ActionResult<Vehicle> Put(long id,[FromBody] UpdateVehicleDto request) 
        {
            Vehicle vehicle = session.vehicle.Where(x => x.id == id).FirstOrDefault();
            if (vehicle == null)
            {
                return NotFound();
            }

            try
            {
                session.BeginTransaction();

                vehicle.vehiclename = request.vehiclename;
                vehicle.vehicleplate = request.vehicleplate;
              

                session.Update(vehicle);

                session.Commit();
            }
            catch (Exception ex)
            {
                session.Rollback();
                Log.Error(ex, " Insert Error");
            }
            finally
            {
                session.CloseTransaction();
            }


            return Ok();
        }

        //container delete method. When a vehicle is deleted, all its containers are deleted.
        [HttpDelete("{id}")]
        public ActionResult<Vehicle> Delete(long id) 
        {
            Vehicle vehicle = session.vehicle.Where(x => x.id == id).FirstOrDefault();
            List<Container> container = containerSession.container.Where(x => x.vehicleid == id).ToList();
            if (vehicle == null)
            {
                return NotFound();
            }

            try
            {
                session.BeginTransaction();
                session.Delete(vehicle);
                session.Commit();

                containerSession.BeginTransaction();
                foreach (var item in container)
                {
                    containerSession.Delete(item);
                }
               
                containerSession.Commit();
            }
            catch (Exception ex)
            {
                session.Rollback();
                Log.Error(ex, "Insert Error");
            }
            finally
            {
                session.CloseTransaction();
                containerSession.CloseTransaction();
            }

            return Ok();
        }


    }
}
