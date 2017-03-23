using Apps.IDAL;
using Apps.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Apps.DAL
{
    public abstract class BaseRepository<T> : IBaseRepository<T> where T:class
    {
        DBContainer db;
        public BaseRepository(DBContainer context)
        {
            this.db = context;
        }

        public DBContainer Context
        {
            get { return db; }
        }

        public virtual bool Create(T model)
        {
            db.Set<T>().Add(model);
            return db.SaveChanges()>0;
        }

        public virtual bool Edit(T model)
        {
            if (db.Entry<T>(model).State == EntityState.Modified)
            {
                return db.SaveChanges() > 0;
            }
            else if (db.Entry<T>(model).State == EntityState.Detached)
            {
                try
                {
                    db.Set<T>().Attach(model);
                    db.Entry<T>(model).State = EntityState.Modified;
                }
                catch (InvalidOperationException)
                {
                    //T old = Find(model._ID);
                    //db.Entry<old>.CurrentValues.SetValues(model);
                    return false;
                }
                return db.SaveChanges()>0;
            }
            return false;
        }

        public virtual bool Delete(T model)
        {
            db.Set<T>().Remove(model);
            return db.SaveChanges()>0;
        }

        public virtual int Delete(params object[] keyValues)
        {
            T model = GetById(keyValues);
            if(model!=null)
            {
                db.Set<T>().Remove(model);
                return db.SaveChanges();
            }
            return -1;
        }
        public virtual T GetById(params object[] keyValues)
        {
            return db.Set<T>().Find(keyValues);
        }

        public virtual IQueryable<T> GetList()
        {
            return db.Set<T>();
        }

        public virtual IQueryable<T> GetList(Func<T, bool> whereLambda)
        {
            return db.Set<T>().Where(whereLambda).AsQueryable();
        }
        public virtual IQueryable<T> GetList<S>(int pageSize, int pageIndex, out int total
            ,Expression<Func<T,bool>> whereLambda,bool isAsc, Expression<Func<T,bool>> orderByLambda)
        {
            var queryable = db.Set<T>().Where(whereLambda);
            total = queryable.Count();
            if (isAsc)
            {
                queryable = queryable.OrderBy(orderByLambda).Skip<T>(pageSize * (pageIndex - 1)).Take<T>(pageSize);
            }
            else {
                queryable = queryable.OrderByDescending(orderByLambda).Skip<T>(pageSize * (pageIndex - 1)).Take<T>(pageSize);
            }
            return queryable;
        }

        public virtual bool IsExist(object id)
        {
            return GetById(id)!=null;
        }

        public int SaveChanges()
        {
           return db.SaveChanges();
        }


        //1、 Finalize只释放非托管资源；
        //2、 Dispose释放托管和非托管资源；
        //3、 重复调用Finalize和Dispose是没有问题的；
        //4、 Finalize和Dispose共享相同的资源释放策略，因此他们之间也是没有冲突的。
         public void Dispose()
         {
            Dispose(true);
            GC.SuppressFinalize(this);
         }

         public void Dispose(bool disposing)
         {
      
             if (disposing)
             {
                   Context.Dispose();
              }
        }
    }
}
