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
    public class OrderItemsController : ApiController
    {
        private DBModel db = new DBModel();

        // GET: api/OrderItems
        public IQueryable<OrderItems> GetOrderItems1()
        {
            return db.OrderItems1;
        }

        // GET: api/OrderItems/5
        [ResponseType(typeof(OrderItems))]
        public IHttpActionResult GetOrderItems(int id)
        {
            OrderItems orderItems = db.OrderItems1.Find(id);
            if (orderItems == null)
            {
                return NotFound();
            }

            return Ok(orderItems);
        }

        // PUT: api/OrderItems/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOrderItems(int id, OrderItems orderItems)
        {
           

            if (id != orderItems.OrderItemID)
            {
                return BadRequest();
            }

            db.Entry(orderItems).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderItemsExists(id))
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

        // POST: api/OrderItems
        [ResponseType(typeof(OrderItems))]
        public IHttpActionResult PostOrderItems(OrderItems orderItems)
        {
          

            db.OrderItems1.Add(orderItems);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = orderItems.OrderItemID }, orderItems);
        }

        // DELETE: api/OrderItems/5
        [ResponseType(typeof(OrderItems))]
        public IHttpActionResult DeleteOrderItems(int id)
        {
            OrderItems orderItems = db.OrderItems1.Find(id);
            if (orderItems == null)
            {
                return NotFound();
            }

            db.OrderItems1.Remove(orderItems);
            db.SaveChanges();

            return Ok(orderItems);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderItemsExists(int id)
        {
            return db.OrderItems1.Count(e => e.OrderItemID == id) > 0;
        }
    }
}