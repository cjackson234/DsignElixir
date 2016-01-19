using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using RozichMurals.Web.Models;

namespace dsignelixir.Models
{ 
    public class ImageRepository : IImageRepository
    {
        dsignelixirContext context = new dsignelixirContext();

        public IQueryable<Image> All
        {
            get { return context.Images; }
        }

        public IQueryable<Image> AllIncluding(params Expression<Func<Image, object>>[] includeProperties)
        {
            IQueryable<Image> query = context.Images;
            foreach (var includeProperty in includeProperties) {
                query = query.Include(includeProperty);
            }
            return query;
        }

        public Image Find(int id)
        {
            return context.Images.Find(id);
        }

        public void InsertOrUpdate(Image image)
        {
            if (image.id == default(int)) {
                // New entity
                context.Images.Add(image);
            } else {
                // Existing entity
                context.Entry(image).State = EntityState.Modified;
            }
        }

        public void Delete(int id)
        {
            var image = context.Images.Find(id);
            context.Images.Remove(image);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose() 
        {
            context.Dispose();
        }
    }

    public interface IImageRepository : IDisposable
    {
        IQueryable<Image> All { get; }
        IQueryable<Image> AllIncluding(params Expression<Func<Image, object>>[] includeProperties);
        Image Find(int id);
        void InsertOrUpdate(Image image);
        void Delete(int id);
        void Save();
    }
}