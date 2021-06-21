using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Versioning
{
    /// <summary>
    /// Represents a class containing the information about a project.
    /// </summary>
    public class ProjectSystem
    {
        private string _systemName;
        private DateTime _publishedDateTime;

        public ProjectSystem(string systemName, DateTime publishedDateTime)
        {
            _systemName = systemName;
            _publishedDateTime = publishedDateTime;
        }

        public string GetSystemName { get { return _systemName; } }

        public DateTime PublishedDateTime { get { return _publishedDateTime; } }
    }
}
