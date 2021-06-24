using ObjectLibrary.Common;
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
    public class ProjectInformation : BaseEntity
    {
        private readonly string _systemName;
        private DateTime _publishedDateTime;

        public ProjectInformation(int id, string systemName, DateTime publishedDateTime) : base(id)
        {
            _systemName = systemName;
            _publishedDateTime = publishedDateTime;
        }

        public string GetSystemName { get { return _systemName; } }

        public DateTime PublishedDateTime { get { return _publishedDateTime; } }
    }
}
