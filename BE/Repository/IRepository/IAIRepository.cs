using BE.Data;
using BE.DTOs;
using BE.models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BE.Repository.IRepository
{
    public interface IAIRepository
    {
        Task<List<ProductSearchResult>> SearchProductsAsync(string query);
        Task<List<string>> GetAllWithDetailsAsync();
    }
}