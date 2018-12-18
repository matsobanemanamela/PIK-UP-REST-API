using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using PIK_UP_REST_API.Models;

namespace PIK_UP_REST_API.Controllers
{
    public class TransportsController : ApiController
    {
        private PIKUPEntities db = new PIKUPEntities();

        // GET: api/Transports
        [AllowAnonymous]
        public IQueryable<Object> GetTransports()
        {
            var transport = db.Transports.Select(x => new { x.TransportationID, x.UserID, x.DepartureDestination, x.ArrivalDestination, x.DepartureDate, x.Image, x.NumberOfPassengers, x.Price,x.Comment });
            return transport;
        }

        // GET: api/Transports/5
        /*  [AllowAnonymous]
          [ResponseType(typeof(Transport))]
          public IHttpActionResult GetTransport(int id)
          {
              Transport transport = db.Transports.Find(id);
              if (transport == null)
              {
                  return NotFound();
              }

              return Ok(transport);
          } */

        [HttpGet]
        [Route("api/GeteveryTransportationdetails")]
        [AllowAnonymous]
        public IQueryable<Object> GeteveryTransportationdetails()
        {

            var list = db.UserTables.Join(db.Transports, trav => trav.UserID, ca => ca.UserID,
                (trav, ca) => new {
                    TransportationID = ca.TransportationID,
                    UserID = trav.UserID,
                    DepartureDestination = ca.DepartureDestination,
                    ArrivalDestination = ca.ArrivalDestination,
                    DepartureDate = ca.DepartureDate,
                    Image = ca.Image,
                    NumberOfPassengers = ca.NumberOfPassengers,
                    Price = ca.Price,
                    Comment = ca.Comment,
                    Name = trav.Name,
                    Surname = trav.Surname
                }
              );

            //var details = list.Where(c => c.UserID.Equals(id));

            if (list == null)
            {
                return (null);
            }

            return list;

        }

        [HttpGet]
        [Route("api/GetTransportationdetails")]
        [AllowAnonymous]
        public IQueryable<Object> GetTransportationdetails(int id)
        {

            var list = db.UserTables.Join(db.Transports, trav => trav.UserID, ca => ca.UserID,
                (trav, ca) => new {
                    TransportationID = ca.TransportationID,
                    UserID = trav.UserID,
                    DepartureDestination = ca.DepartureDestination,
                    ArrivalDestination = ca.ArrivalDestination,
                    DepartureDate = ca.DepartureDate,
                    Image = ca.Image,
                    NumberOfPassengers = ca.NumberOfPassengers,
                    Price = ca.Price,
                    Comment = ca.Comment
                }
              );

            var details = list.Where(c => c.UserID.Equals(id));

            if (details == null)
            {
                return (null);
            }

            return details;

        }




        // PUT: api/Transports/5
        [AllowAnonymous]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTransport(int id, Transport transport)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != transport.TransportationID)
            {
                return BadRequest();
            }

            db.Entry(transport).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TransportExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Transports
        [AllowAnonymous]
        [ResponseType(typeof(Transport))]
        public IHttpActionResult PostTransport(Transport transport)
        {
           /* if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            } */

            db.Transports.Add(transport);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = transport.TransportationID }, transport);
        }

        // DELETE: api/Transports/5
        [AllowAnonymous]
        [ResponseType(typeof(Transport))]
        public IHttpActionResult DeleteTransport(int id)
        {
            Transport transport = db.Transports.Find(id);
            if (transport == null)
            {
                return NotFound();
            }

            db.Transports.Remove(transport);
            db.SaveChanges();

            return Ok(transport);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool TransportExists(int id)
        {
            return db.Transports.Count(e => e.TransportationID == id) > 0;
        }
    }
}