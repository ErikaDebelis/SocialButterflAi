using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

using SocialButterflAi.Models;

namespace SocialButterflAi.Api.Helpers
{
    public class ApiHelpers
    {
        private Serilog.ILogger SeriLogger => Serilog.Log.Logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="modelProvider"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="Exception"></exception>
        public async Task<BaseResponse<string>> GenerateBase64Async(
            IFormFile file
        )
        {
            var response = new BaseResponse<string>();
            try
            {
                var base64 = string.Empty;

                using (var memoryStream = new MemoryStream())
                {
                    await file.OpenReadStream().CopyToAsync(memoryStream);
                    var fileBytes = memoryStream.ToArray();
                    base64 = Convert.ToBase64String(fileBytes);
                }

                SeriLogger.Information("Base64 string generated successfully");

                response.Success = true;
                response.Data = base64;

                return response;
            }
            catch (Exception ex)
            {
                SeriLogger.Fatal(ex, "Error");
                throw new Exception("Error", ex);
            }
        }
    }
}