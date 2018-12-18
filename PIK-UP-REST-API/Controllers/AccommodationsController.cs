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
    public class AccommodationsController : ApiController
    {
        private PIKUPEntities db = new PIKUPEntities();

        // GET: api/Accommodations
        [AllowAnonymous]
        public IQueryable<Object> GetAccommodations()
        {
            var Accommodations = db.Accommodations.Select(x => new {x.AccommodationID,x.UserID,x.TypeOfAccommodation,x.NumberofRooms,x.NumberofPeoplePerRoom,x.DistancetoCampus,x.WIFI,x.Price,x.Address,
                x.Suburb,x.City,x.Province, x.Country,x.PostalCode,x.MainImage,x.RoomImage,x.KitchenImage,x.BathroomImage,x
                .Comment
            });
            return Accommodations;
        }

        // GET: api/Accommodations/5
        /*[ResponseType(typeof(Accommodation))]
        public IHttpActionResult GetAccommodation(int id)
        {
            Accommodation accommodation = db.Accommodations.Find(id);
            if (accommodation == null)
            {
                return NotFound();
            }

            return Ok(accommodation);
        } */

        [HttpGet]
        [Route("api/GetAccommodation")]
        [AllowAnonymous]
        public IQueryable<Object> GetAccommodation(int id)
        {

            var list = db.UserTables.Join(db.Accommodations, trav => trav.UserID, ca => ca.UserID,
                (trav, ca) => new {
                    AccommodationID = ca.AccommodationID,
                    UserID = ca.UserID,
                    TypeOfAccommodation = ca.TypeOfAccommodation,
                    NumberofRooms = ca.NumberofRooms,
                    NumberofPeoplePerRoom = ca.NumberofPeoplePerRoom,
                    DistancetoCampus = ca.DistancetoCampus,
                    WIFI = ca.WIFI,
                    Price = ca.Price,
                    Comment = ca.Comment,
                    Address = ca.Address,
                    Suburb = ca.Suburb,
                    City = ca.City,
                    Province = ca.Province,
                    Country = ca.Country,
                    PostalCode = ca.PostalCode,
                    MainImage = ca.MainImage,
                    RoomImage = ca.RoomImage,
                    KitchenImage = ca.KitchenImage,
                    BathroomImage = ca.BathroomImage,
                    Name = trav.Name,
                    Image = trav.Image
    }
              );

            var details = list.Where(c => c.UserID.Equals(id));

            if (details == null)
            {
                return (null);
            }

            return details;

        }


        [HttpGet]
        [Route("GetAccommodationlist")]
        [AllowAnonymous]
        public IQueryable<Object> GetAccommodationlist()
        {

            var list = db.UserTables.Join(db.Accommodations, trav => trav.UserID, ca => ca.UserID,
                (trav, ca) => new {
                    AccommodationID = ca.AccommodationID,
                    UserID = ca.UserID,
                    TypeOfAccommodation = ca.TypeOfAccommodation,
                    NumberofRooms = ca.NumberofRooms,
                    NumberofPeoplePerRoom = ca.NumberofPeoplePerRoom,
                    DistancetoCampus = ca.DistancetoCampus,
                    WIFI = ca.WIFI,
                    Price = ca.Price,
                    Comment = ca.Comment,
                    Address = ca.Address,
                    Suburb = ca.Suburb,
                    City = ca.City,
                    Province = ca.Province,
                    Country = ca.Country,
                    PostalCode = ca.PostalCode,
                    MainImage = ca.MainImage,
                    RoomImage = ca.RoomImage,
                    KitchenImage = ca.KitchenImage,
                    BathroomImage = ca.BathroomImage,
                    Name = trav.Name,
                    Image = trav.Image,
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
        // PUT: api/Accommodations/5
        [AllowAnonymous]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAccommodation(int id, Accommodation accommodation)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != accommodation.AccommodationID)
            {
                return BadRequest();
            }

            db.Entry(accommodation).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AccommodationExists(id))
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

        // POST: api/Accommodations
        [AllowAnonymous]
        [ResponseType(typeof(Accommodation))]
        public IHttpActionResult PostAccommodation(Accommodation accommodation)
        {

            db.Accommodations.Add(accommodation);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = accommodation.AccommodationID }, accommodation);
        }

        // DELETE: api/Accommodations/5
        [AllowAnonymous]
        [ResponseType(typeof(Accommodation))]
        public IHttpActionResult DeleteAccommodation(int id)
        {
            Accommodation accommodation = db.Accommodations.Find(id);
            if (accommodation == null)
            {
                return NotFound();
            }

            db.Accommodations.Remove(accommodation);
            db.SaveChanges();

            return Ok(accommodation);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AccommodationExists(int id)
        {
            return db.Accommodations.Count(e => e.AccommodationID == id) > 0;
        }
    }
}