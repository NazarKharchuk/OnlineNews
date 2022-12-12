using System;
using System.Collections.Generic;
using System.Text;
using OnlineNews.DAL.Entities;
using OnlineNews.DAL.Interfaces;
using OnlineNews.DAL.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace OnlineNews.DAL.Repositories
{
    public class RubricRepository : IRepository<Rubric>
    {
        private NewsContext db;

        public RubricRepository(NewsContext context)
        {
            db = context;
        }

        public IEnumerable<Rubric> GetAll()
        {
            return db.Rubrics;
        }

        public Rubric Get(int id)
        {
            return db.Rubrics.Find(id);
        }

        public void Create(Rubric rubric)
        {
            db.Rubrics.Add(rubric);
        }

        public void Update(Rubric rubric)
        {
            db.Entry(rubric).State = EntityState.Modified;
        }

        public IEnumerable<Rubric> Find(Func<Rubric, Boolean> predicate)
        {
            return db.Rubrics.Where(predicate).ToList();
        }

        public void Delete(int id)
        {
            Rubric rubric = db.Rubrics.Find(id);
            if (rubric != null)
                db.Rubrics.Remove(rubric);
        }
    }
}
