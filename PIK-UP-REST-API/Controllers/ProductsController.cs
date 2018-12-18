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
    public class ProductsController : ApiController
    {
        private PIKUPEntities db = new PIKUPEntities();

        // GET: api/Products
        [AllowAnonymous]
        public IQueryable<Object> GetProducts()
        {
            var Products = db.Products.Select(x => new { x.ProductID, x.UserID, x.Productname, x.Categoryname, x.ProductImage, x.NumberofItem,x.NeworUsed, x.ProductPrice,x.Comment });
            return Products;
        }

        //// GET: api/Products/5

        //[ResponseType(typeof(Product))]
        //public IHttpActionResult GetProduct(int id)
        //{
        //    Product product = db.Products.Find(id);
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(product);
        //}

        [HttpGet]
        [Route("api/Geteveyproduct")]
        [AllowAnonymous]
        public IQueryable<Object> Geteveyproduct()
        {

            var list = db.UserTables.Join(db.Products, trav => trav.UserID, ca => ca.UserID,
                (trav, ca) => new {
                    ProductID = ca.ProductID,
                    UserID = trav.UserID,
                    Productname = ca.Productname,
                    Categoryname = ca.Categoryname,
                    ProductImage = ca.ProductImage,
                    NumberofItem = ca.NumberofItem,
                    NeworUsed = ca.NeworUsed,
                    ProductPrice = ca.ProductPrice,
                    Comment = ca.Comment,
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

        [HttpGet]
        [Route("api/GetProduct")]
        [AllowAnonymous]
        public IQueryable<Object> GetProduct(int id)
        {

            var list = db.UserTables.Join(db.Products, trav => trav.UserID, ca => ca.UserID,
                (trav, ca) => new {
                    ProductID = ca.ProductID,
                    UserID = trav.UserID,
                    Productname = ca.Productname,
                    Categoryname = ca.Categoryname,
                    ProductImage = ca.ProductImage,
                    NumberofItem = ca.NumberofItem,
                    NeworUsed = ca.NeworUsed,
                    ProductPrice = ca.ProductPrice,
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



        // PUT: api/Products/5
        [AllowAnonymous]
        [ResponseType(typeof(void))]
        public IHttpActionResult PutProduct(int id, Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != product.ProductID)
            {
                return BadRequest();
            }

            db.Entry(product).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductExists(id))
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

        // POST: api/Products
        [AllowAnonymous]
        [ResponseType(typeof(Product))]
        public IHttpActionResult PostProduct(Product product)
        {
          

            db.Products.Add(product);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = product.ProductID }, product);
        }

        // DELETE: api/Products/5
        [AllowAnonymous]
        [ResponseType(typeof(Product))]
        public IHttpActionResult DeleteProduct(int id)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }

            db.Products.Remove(product);
            db.SaveChanges();

            return Ok(product);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ProductExists(int id)
        {
            return db.Products.Count(e => e.ProductID == id) > 0;
        }
    }
}