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
    public class CommentSkillTalentsController : ApiController
    {
        private PIKUPEntities db = new PIKUPEntities();

        // GET: api/CommentSkillTalents
        public IQueryable<Object> GetCommentSkillTalents()
        {
            var CommentSkillTalents = db.CommentSkillTalents.Select(x => new {x.CommentID,x.skillandtalentID,x.UserID,x.DateandTime,x.Comments });
            return CommentSkillTalents;
        }

        // GET: api/CommentSkillTalents/5
        /*  [ResponseType(typeof(CommentSkillTalent))]
          public IHttpActionResult GetCommentSkillTalent(int id)
          {
              CommentSkillTalent commentSkillTalent = db.CommentSkillTalents.Find(id);
              if (commentSkillTalent == null)
              {
                  return NotFound();
              }

              return Ok(commentSkillTalent);
          } */

        [HttpGet]
        [Route("api/GetCommentSkillTalents")]
        [AllowAnonymous]
        public IQueryable<Object> GetCommentSkillTalents(int id)
        {

            var list = db.SkillsTalents.Join(db.CommentSkillTalents, trav => trav.skillandtalentID, ca => ca.skillandtalentID,
                (trav, ca) => new {
                    CommentID = ca.CommentID,
                    skillandtalentID = ca.skillandtalentID,
                    UserID = ca.UserID,
                    DateandTime = ca.DateandTime,
                    Comments = ca.Comments

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
        [Route("api/GetCommentSkillTalentsCount")]
        [AllowAnonymous]
        public int GetCommentSkillTalentsCount(int id)
        {

            var list = db.SkillsTalents.Join(db.CommentSkillTalents, trav => trav.skillandtalentID, ca => ca.skillandtalentID,
                           (trav, ca) => new {
                               CommentID = ca.CommentID,
                               skillandtalentID = trav.skillandtalentID,
                               UserID = ca.UserID,
                               DateandTime = ca.DateandTime,
                               Comments = ca.Comments

                           }
                         );

            var details = list.Where(c => c.skillandtalentID.Equals(id)).Count();

            return details;

        }

        // PUT: api/CommentSkillTalents/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutCommentSkillTalent(int id, CommentSkillTalent commentSkillTalent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != commentSkillTalent.CommentID)
            {
                return BadRequest();
            }

            db.Entry(commentSkillTalent).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentSkillTalentExists(id))
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

        // POST: api/CommentSkillTalents
        [ResponseType(typeof(CommentSkillTalent))]
        public IHttpActionResult PostCommentSkillTalent(CommentSkillTalent commentSkillTalent)
        {
        
            db.CommentSkillTalents.Add(commentSkillTalent);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = commentSkillTalent.CommentID }, commentSkillTalent);
        }

        // DELETE: api/CommentSkillTalents/5
        [ResponseType(typeof(CommentSkillTalent))]
        public IHttpActionResult DeleteCommentSkillTalent(int id)
        {
            CommentSkillTalent commentSkillTalent = db.CommentSkillTalents.Find(id);
            if (commentSkillTalent == null)
            {
                return NotFound();
            }

            db.CommentSkillTalents.Remove(commentSkillTalent);
            db.SaveChanges();

            return Ok(commentSkillTalent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool CommentSkillTalentExists(int id)
        {
            return db.CommentSkillTalents.Count(e => e.CommentID == id) > 0;
        }
    }
}