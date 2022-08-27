using Assignment3.Context;
using Assignment3.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContainerController : ControllerBase
    {

        private readonly IMapperSessionContainer session;
        private readonly IMapperSessionVehicle sessionVehicle;
        public ContainerController(IMapperSessionContainer session, IMapperSessionVehicle sessionVehicle)
        {
            this.session = session;
            this.sessionVehicle = sessionVehicle;
        }

        [HttpGet]
        public List<Container> Get()      // container get method
        {
            List<Container> result = session.container.ToList();
            return result;
        }


        [HttpGet("{id}")]
        public Container Get(long id) 
        {
            Container result = session.container.Where(x => x.id == id).FirstOrDefault();
            return result;
        }
        [HttpGet("id/{vehicleId}")]
        public List<Container> GetContainerByVehicleId(long vehicleId) //get method containers by vehicleId
        {
            List<Container> result = session.container.Where(x => x.vehicleid == vehicleId).ToList();
            return result;
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] Container container) //container add method
        {

                try
                {
                    if (sessionVehicle.vehicle.Any(x => x.id == container.vehicleid))
                    {
                        session.BeginTransaction();
                        session.Save(container);
                        session.Commit();

                    }
                    else
                    {
                        return BadRequest("There is no such truck");
                    }
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
            return Ok();
        }

        [HttpPut]
        public ActionResult<Container> Put(long id, [FromBody] UpdateContainerDto request) //container update method
        {
            Container container = session.container.Where(x => x.id == id).FirstOrDefault();
            if (container == null)
            {
                return NotFound();
            }

            try
            {
                session.BeginTransaction();

                container.containername = request.containername;
                container.latitude = request.latitude;
                container.longitude = request.longitude;


                session.Update(container);

                session.Commit();
            }
            catch (Exception ex)
            {
                session.Rollback();
                Log.Error(ex, " Update Error");
            }
            finally
            {
                session.CloseTransaction();
            }


            return Ok();
        }


        [HttpDelete("{id}")]
        public ActionResult<Container> Delete(long id) //container delete method
        {
            Container container = session.container.Where(x => x.id == id).FirstOrDefault();
            if (container == null)
            {
                return NotFound();
            }

            try
            {
                session.BeginTransaction();
                session.Delete(container);
                session.Commit();
            }
            catch (Exception ex)
            {
                session.Rollback();
                Log.Error(ex, "Delete Error");
            }
            finally
            {
                session.CloseTransaction();
            }

            return Ok();
        }


        [HttpGet("{vehicleId}/numberOfClusters")] //method that groups containers by number of clusters
        public List<List<Container>> ContainerGrouping(int vehicleId, int numberOfClusters)
        {
            var i = 0;

            var container = session.container.Where(x => x.vehicleid == vehicleId).ToList();
            var result =
                   container.GroupBy(s => i++ / numberOfClusters).Select(g => g.ToList()).ToList();

            return result;

        }
      
        }
    }


