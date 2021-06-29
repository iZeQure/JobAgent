using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.Providers
{
    public class TaskExtProvider
    {
        public static async Task WhenAll(params Task[] tasks)
        {
            var allTasks = Task.WhenAll(tasks);

            try
            {
                await allTasks;
            }
            catch (Exception)
            {
                // Ignore
            }

            throw allTasks.Exception ?? throw new Exception("This can't possibly happen.");
        }
    }
}
