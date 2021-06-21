using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ObjectLibrary.Versioning
{
    /// <summary>
    /// Represents class handling the version control for the project.
    /// </summary>
    public class Version
    {
        private string _hashId;
        private ProjectSystem _system;
        private ReleaseType _releaseType;
        private int _major;
        private int _minor;
        private int _patch;
        private DateTime _releaseDateTime;

        public Version(string hashId, ProjectSystem system, ReleaseType releaseType, int major, int minor, int patch, DateTime releaseDateTime)
        {
            _hashId = hashId;
            _system = system;
            _releaseType = releaseType;
            _major = major;
            _minor = minor;
            _patch = patch;
            _releaseDateTime = releaseDateTime;
        }

        public string HashId { get => _hashId; set => _hashId = value; }
        public ProjectSystem System { get => _system; set => _system = value; }
        public ReleaseType ReleaseType { get => _releaseType; set => _releaseType = value; }
        public int Major { get => _major; set => _major = value; }
        public int Minor { get => _minor; set => _minor = value; }
        public int Patch { get => _patch; set => _patch = value; }
        public DateTime ReleaseDateTime { get => _releaseDateTime; set => _releaseDateTime = value; }
    }
}
