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
    public class LikeSkillsTalentsController : ApiController
    {
        private PIKUPEntities db = new PIKUPEntities();

        // GET: api/LikeSkillsTalents
        [AllowAnonymous]
        public IQueryable<Object> GetLikeSkillsTalents()
        {
            var LikeSkillsTalents = db.LikeSkillsTalents.Select(x => new {x.likeId,x.skillandtalentID,x.UserID,x.DateTime,x.Condition });
            return LikeSkillsTalents;
        }

        // GET: api/LikeSkillsTalents/5
        /*  [ResponseType(typeof(LikeSkillsTalent))]
          public IHttpActionResult GetLikeSkillsTalent(int id)
          {
              LikeSkillsTalent likeSkillsTalent = db.LikeSkillsTalents.Find(id);
              if (likeSkillsTalent == null)
              {
                  return NotFound();
              }

              return Ok(likeSkillsTalent);
          }  */

        [HttpGet]
        [Route("api/GetLikeSkillsTalents")]
        [AllowAnonymous]
        public IQueryable<Object> GetLikeSkillsTalents(int id)
        {

            var list = db.SkillsTalents.Join(db.LikeSkillsTalents, trav => trav.skillandtalentID, ca => ca.skillandtalentID,
                (trav, ca) => new {
                    likeId = ca.likeId,
                    skillandtalentID = ca.skillandtalentID,
                    UserID = ca.UserID,
                    DateTime = ca.DateTime,
                    Condition = ca.Condition

                }
              );

            var details = list.Where(c => c.skillandtalentID.Equals(id));

            if (details == null)
            {
                return (null);
            }

            return details;

        }

        [HttpGet]
        [Route("api/GetLikeSkillsTalentsCount")]
        [AllowAnonymous]
        public int GetLikeSkillsTalentsCount(int id)
        {

            var list = db.SkillsTalents.Join(db.LikeSkillsTalents, trav => trav.skillandtalentID, ca => ca.skillandtalentID,
                (trav, ca) => new {
                    likeId = ca.likeId,
                    skillandtalentID = trav.skillandtalentID,
                    UserID = ca.UserID,
                    DateTime = ca.DateTime,
                    Condition = ca.Condition

                }
              );

            var details = list.Where(c => c.skillandtalentID.Equals(id)).Count();

            return details;

        }

        // PUT: api/LikeSkillsTalents/5
        [AllowAnonymous]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutLikeSkillsTalent(int id, LikeSkillsTalent likeSkillsTalent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != likeSkillsTalent.likeId)
            {
                return BadRequest();
            }

            db.Entry(likeSkillsTalent).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LikeSkillsTalentExists(id))
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

        // POST: api/LikeSkillsTalents
        [AllowAnonymous]
        [ResponseType(typeof(LikeSkillsTalent))]
        public IHttpActionResult PostLikeSkillsTalent(LikeSkillsTalent likeSkillsTalent)
        {
        
            db.LikeSkillsTalents.Add(likeSkillsTalent);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = likeSkillsTalent.likeId }, likeSkillsTalent);
        }

        // DELETE: api/LikeSkillsTalents/5
        [AllowAnonymous]
        [ResponseType(typeof(LikeSkillsTalent))]
        public IHttpActionResult DeleteLikeSkillsTalent(int id)
        {
            LikeSkillsTalent likeSkillsTalent = db.LikeSkillsTalents.Find(id);
            if (likeSkillsTalent == null)
            {
                return NotFound();
            }

            db.LikeSkillsTalents.Remove(likeSkillsTalent);
            db.SaveChanges();

            return Ok(likeSkillsTalent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool LikeSkillsTalentExists(int id)
        {
            return db.LikeSkillsTalents.Count(e => e.likeId == id) > 0;
        }
    }
}