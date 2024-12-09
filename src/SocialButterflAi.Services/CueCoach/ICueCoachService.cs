using System;
using System.Threading.Tasks;

namespace SocialButterflAi.Services.CueCoach
{
    public interface ICueCoachService
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        /// <exception cref="Exception"></exception>
        public Task<object> ProcessMessageAsync(
        );
    }
}