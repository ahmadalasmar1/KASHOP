using KASHOP.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.DAL.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
            
        }

        public async Task<T> CreateAsync(T entity)
        {
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        

        public async Task<List<T>> GetAllAsync(string[] includes= null)
        {

            IQueryable<T> query = _context.Set<T>();
            //خليناها queryable عشان نقدر نضيف includes لو في
            //queryable هو نوع من أنواع البيانات في C# يسمح لنا ببناء استعلامات ديناميكية على البيانات. يعني نقدر نضيف شروط أو عمليات تصفية أو ترتيب على البيانات قبل ما نسترجعها من قاعدة البيانات.
            //في هذا الكود، لو كان includes مش فارغ، بنضيف كل include على الاستعلام query باستخدام Include. Include ده بيخلي EF Core يجيب البيانات المرتبطة بالكيان الرئيسي اللي احنا بنسترجعه.
           // البيانات تخزن في الذاكرة مؤقتًا لحد ما نخلص كل العمليات ونرجع النتيجة النهائية باستخدام ToListAsync.ده بيخلي الاستعلام يكون أكثر كفاءة ويقلل عدد الاستعلامات اللي بتروح لقاعدة البيانات.

            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return await query.ToListAsync();
            //return await _context.Set<T>().ToListAsync();// القديمة

        }
        public async Task<T> GetByOneasync(Expression<Func<T,bool>> filter, string[]? includes = null)
        {
            IQueryable<T> query = _context.Set<T>();
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.FirstOrDefaultAsync(filter);
        }
        public async Task<bool> DeleteAsync(T entity)
        {
            _context.Remove(entity);
            var effected = await _context.SaveChangesAsync();
            return effected > 0;
        }

        public async Task<bool> UpdateAsync(T entity)
        {
            _context.Update(entity);
            var effected = await _context.SaveChangesAsync();
            return effected > 0;    
        }
    }
}
