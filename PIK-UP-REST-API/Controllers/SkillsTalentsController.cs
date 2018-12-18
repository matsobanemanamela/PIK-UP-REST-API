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
    public class SkillsTalentsController : ApiController
    {
        private PIKUPEntities db = new PIKUPEntities();

        // GET: api/SkillsTalents
        [AllowAnonymous]
        public IQueryable<Object> GetSkillsTalents()
        {
            var SkillsTalents = db.SkillsTalents.Select(x => new {x.skillandtalentID,x.UserID,x.TypeofskillORtalent,x.Image,x.Video,x.Song,x.Comment });
            return SkillsTalents;
        }

        // GET: api/SkillsTalents/5
        /* [ResponseType(typeof(SkillsTalent))]
         public IHttpActionResult GetSkillsTalent(int id)
         {
             SkillsTalent skillsTalent = db.SkillsTalents.Find(id);
             if (skillsTalent == null)
             {
                 return NotFound();
             }

             return Ok(skillsTalent);
         }
         */

        [HttpGet]
        [Route("api/GeteverySkillsTalent")]
        [AllowAnonymous]
        public IQueryable<Object> GeteverySkillsTalent()
        {

            var list = db.UserTables.Join(db.SkillsTalents, trav => trav.UserID, ca => ca.UserID,
                (trav, ca) => new {
                    skillandtalentID = ca.skillandtalentID,
                    UserID = trav.UserID,
                    TypeofskillORtalent = ca.TypeofskillORtalent,
                    Image = ca.Image,
                    Video = ca.Video,
                    Song = ca.Song,
                    Comment = ca.Comment
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
        [Route("api/GetSkillsTalent")]
        [AllowAnonymous]
        public IQueryable<Object> GetSkillsTalent(int id)
        {

            var list = db.UserTables.Join(db.SkillsTalents, trav => trav.UserID, ca => ca.UserID,
                (trav, ca) => new {
                    skillandtalentID = ca.skillandtalentID,
                    UserID = trav.UserID,
                    TypeofskillORtalent = ca.TypeofskillORtalent,
                    Image = ca.Image,
                    Video = ca.Video,
                    Song = ca.Song,
                    Comment = ca.Comment,
                    Name = trav.Name,
                    Surname = trav.Surname
                }
              );

            var details = list.Where(c => c.UserID.Equals(id));

            if (details == null)
            {
                return (null);
            }

            return details;

        }

        // PUT: api/SkillsTalents/5
        [AllowAnonymous]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutSkillsTalent(int id, SkillsTalent skillsTalent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != skillsTalent.skillandtalentID)
            {
                return BadRequest();
            }

            db.Entry(skillsTalent).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SkillsTalentExists(id))
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

        // POST: api/SkillsTalents
        [AllowAnonymous]
        [ResponseType(typeof(SkillsTalent))]
        public IHttpActionResult PostSkillsTalent(SkillsTalent skillsTalent)
        {

            db.SkillsTalents.Add(skillsTalent);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = skillsTalent.skillandtalentID }, skillsTalent);
        }

        // DELETE: api/SkillsTalents/5
        [AllowAnonymous]
        [ResponseType(typeof(SkillsTalent))]
        public IHttpActionResult DeleteSkillsTalent(int id)
        {
            SkillsTalent skillsTalent = db.SkillsTalents.Find(id);
            if (skillsTalent == null)
            {
                return NotFound();
            }

            db.SkillsTalents.Remove(skillsTalent);
            db.SaveChanges();

            return Ok(skillsTalent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool SkillsTalentExists(int id)
        {
            return db.SkillsTalents.Count(e => e.skillandtalentID == id) > 0;
        }
    }
}