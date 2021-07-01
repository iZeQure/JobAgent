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

            Console.WriteLine( $"Processing Task : {allTasks}");

            try
            {
                await allTasks;
                Console.WriteLine("Exception Happened.");
            }
            catch (Exception)
            {
                // Ignore
            }

            throw allTasks.Exception ?? throw new Exception("This can't possibly happen.");
        }
    }
}
