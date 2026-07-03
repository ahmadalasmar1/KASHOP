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
            await _categoryRepository.Create(category);
            var response = category.Adapt<CategoryResponse>();
            return response;
        }

        public async Task<List<CategoryResponse>> GetAll()
        {
            var categories = await _categoryRepository.GetAll();
            var response = categories.Adapt<List<CategoryResponse>>();

            return response;

        }
}
}
