using System;
using System.Threading.Tasks;

namespace JobAgentClassLibrary.Security.Providers
{
    public class TaskExtProvider
    {
        public static async Task WhenAll(params Task[] tasks)
        {
            var allTasks = Task.WhenAll(tasks);

            try
            {
                await allTasks;
                return;
            }
            catch (Exception)
            {
                // Ignore
            }

            throw allTasks.Exception ?? throw new Exception("This can't possibly happen.");
        }
    }
}
