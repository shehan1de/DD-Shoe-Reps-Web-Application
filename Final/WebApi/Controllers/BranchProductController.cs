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
using WebApi.Models;

namespace WebApi.Controllers
{
    public class BranchProductController : ApiController
    {
        private DBModel db = new DBModel();

        // GET: api/BranchProduct
        public IQueryable<BranchProduct> GetBranchProducts()
        {
            return db.BranchProducts;
        }

        // GET: api/BranchProduct/5
        [ResponseType(typeof(BranchProduct))]
        public IHttpActionResult GetBranchProduct(int id)
        {
            BranchProduct branchProduct = db.BranchProducts.Find(id);
            if (branchProduct == null)
            {
                return NotFound();
            }

            return Ok(branchProduct);
        }

        // PUT: api/BranchProduct/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutBranchProduct(int id, BranchProduct branchProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != branchProduct.ProductID)
            {
                return BadRequest();
            }

            db.Entry(branchProduct).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BranchProductExists(id))
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

        // POST: api/BranchProduct
        [ResponseType(typeof(BranchProduct))]
        public IHttpActionResult PostBranchProduct(BranchProduct branchProduct)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.BranchProducts.Add(branchProduct);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = branchProduct.ProductID }, branchProduct);
        }

        // DELETE: api/BranchProduct/5
        [ResponseType(typeof(BranchProduct))]
        public IHttpActionResult DeleteBranchProduct(int id)
        {
            BranchProduct branchProduct = db.BranchProducts.Find(id);
            if (branchProduct == null)
            {
                return NotFound();
            }

            db.BranchProducts.Remove(branchProduct);
            db.SaveChanges();

            return Ok(branchProduct);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool BranchProductExists(int id)
        {
            return db.BranchProducts.Count(e => e.ProductID == id) > 0;
        }
    }
}