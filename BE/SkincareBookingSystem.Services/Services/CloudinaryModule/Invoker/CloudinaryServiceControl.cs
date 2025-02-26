using Microsoft.AspNetCore.Http;
using SkincareBookingSystem.Services.IServices;
using SkincareBookingSystem.Services.Services.CloudinaryModule.Commands;

namespace SkincareBookingSystem.Services.Services.CloudinaryModule.Invoker
{
    public class CloudinaryServiceControl
    {
        private ICommand _command;

        public void SetCommand(ICommand command)
        {
            _command = command;
        }

        /// <summary>
        /// Async method to run commands related to Cloudinary services.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task RunAsync()
        {
            if (_command is null)
            {
                throw new InvalidOperationException("Command not set.");
            }

            await _command.ExecuteAsync();
        }
    }
}
