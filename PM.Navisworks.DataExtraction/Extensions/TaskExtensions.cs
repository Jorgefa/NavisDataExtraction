using System;
using System.Threading.Tasks;

namespace PM.Navisworks.DataExtraction.Extensions
{
    public static class TaskExtensions
    {
        public static async void Await(this Task task)
        {
            await task;
        }
    }
}