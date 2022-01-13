using JobAgentClassLibrary.Core.Entities;
using JobAgentClassLibrary.Extensions.SmartUsing;
using System;

namespace JobAgentClassLibrary.Extensions
{
    public static class OperationExtensions
    {
        /// <summary>
        /// Timed end of process operation.
        /// </summary>
        /// <remarks>
        /// <see cref="BaseModel.IsProcessing"/> is set to true when called.
        /// </remarks>
        /// <param name="model"></param>
        /// <returns></returns>
        public static IDisposable TimedEndOfOperation(this BaseModel model)
        {
            model.IsProcessing = !model.IsProcessing;
            return new EndOfProcessOperation(model);
        }
    }
}
