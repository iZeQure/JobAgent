using JobAgentClassLibrary.Core.Entities;
using System;

namespace JobAgentClassLibrary.Extensions.SmartUsing
{
    internal class EndOfProcessOperation : IDisposable
    {
        private BaseModel _model;

        public EndOfProcessOperation(BaseModel model)
        {
            _model = model;
        }

        public void Dispose()
        {
            _model.IsProcessing = !_model.IsProcessing;
            Console.WriteLine(_model.IsProcessing);
        }
    }
}
