using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Linq.Expressions;

namespace PdfConversion.Server.DataService
{
    public interface IRepository<T> : IDisposable  where T:class 
    {
        void Insert(T entity);
        void Delete(T entity);
        IQueryable<T> SearchFor(Expression<Func<T, bool>> predicate);
        IQueryable<T> GetAll();        
        void SaveChanges();
    }

    public class FileStatusRepositoryFactory
    {
        public static IRepository<FileStatusEntity> GetRepository()
        {
            return new FileStatusRepository();
        }

    }

    class FileStatusRepository : IRepository<FileStatusEntity>
    {
        private FileStatusContext dbContext;
        private DbSet<FileStatusEntity> entities;
        public FileStatusRepository()
        {
            this.dbContext = new FileStatusContext();
            this.entities = dbContext.Set<FileStatusEntity>();
        }

        public void Insert(FileStatusEntity entity)
        {
            this.entities.Add(entity);
        }

        public void Delete(FileStatusEntity entity)
        {
            this.entities.Remove(entity);
        }

        public IQueryable<FileStatusEntity> SearchFor(Expression<Func<FileStatusEntity, bool>> predicate)
        {
            return entities.Where(predicate);
        }

        public IQueryable<FileStatusEntity> GetAll()
        {
            return entities.AsNoTracking();
        }
        
    
        public void SaveChanges()
        {
            dbContext.SaveChanges();
        }

        public void Dispose()
        {
            this.dbContext.Dispose();   
        }
    }
}
