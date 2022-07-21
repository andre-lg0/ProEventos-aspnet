using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace ProEventos.Api.Extensions
{
    public static class Pagination {

        public static void AddPagination(this HttpResponse response,
            int currentPage,
            int itemsPerPage,
            int totalItems,
            int totalPages)
        {
            var options  = new JsonSerializerOptions
            {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            response.Headers.Add("Pagination",JsonSerializer.Serialize( new
                {
                    CurrentPage = currentPage,
                    ItemsPerPage = itemsPerPage,
                    TotalItems = totalItems,
                    TotalPages = totalPages
                },
                options));
            
            response.Headers.Add("Access-Control-Expose-Headers","Pagination");
            
            
        }



    }    
}