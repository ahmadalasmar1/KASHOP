using KASHOP.DAL.Dto.Request;
using KASHOP.DAL.Dto.Response;
using KASHOP.DAL.Models;
using KASHOP.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KASHOP.BLL.Services
{
    public class CategoryService : ICategoryService
    {   
        private readonly ICategoryRepository _categoryRepository;

        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryResponse> CreateCategory(CategoryRequest request)
        {
            var category = request.Adapt<Category>();
            await _categoryRepository.CreateAsync(category);
            var response = category.Adapt<CategoryResponse>();
            return response;
        }

        public async Task<List<CategoryResponse>> GetAll()
        {
            var categories = await _categoryRepository.GetAllAsync(
                new string[] {nameof(Category.Translations)});
            var response = categories.Adapt<List<CategoryResponse>>();

            return response;

        }

        public async Task<CategoryResponse> GetCategory(System.Linq.Expressions.Expression<Func<Category, bool>> filter)
        {
            var category = await _categoryRepository.GetByOneasync(filter, new string[] { nameof(Category.Translations) });
            return category.Adapt<CategoryResponse>();
        }

        public async Task<bool> DeleteCategory(int id)
        {
            var category = await _categoryRepository.GetByOneasync(c => c.Id == id);
            if (category == null)
            {
                return false;
            }
            return await _categoryRepository.DeleteAsync(category);
        }

        public async Task<CategoryResponse> UpdateCategory(int id, CategoryRequest request)
        {
            var category = await _categoryRepository.GetByOneasync(c => c.Id == id, new[] { nameof(Category.Translations) });
            if (category == null)
            {
                return null;
            }
            category.Translations = request.Translations.Adapt<List<CategoryTranslation>>();
            _categoryRepository.UpdateAsync(category);
            var response = category.Adapt<CategoryResponse>();
            return response;
        }
    }
}
