using ObjectLibrary.Common;
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
    public class Version : BaseEntity
    {
        private string _commitId;
        private ProjectInformation _projectInformation;
        private ReleaseType _releaseType;
        private int _major;
        private int _minor;
        private int _patch;
        private DateTime _releaseDateTime;

        public Version(int id, string commitId, ProjectInformation system, ReleaseType releaseType, int major, int minor, int patch, DateTime releaseDateTime) : base(id)
        {
            _commitId = commitId;
            _projectInformation = system;
            _releaseType = releaseType;
            _major = major;
            _minor = minor;
            _patch = patch;
            _releaseDateTime = releaseDateTime;
        }

        public string CommitId { get => _commitId; set => _commitId = value; }
        public ProjectInformation System { get => _projectInformation; set => _projectInformation = value; }
        public ReleaseType ReleaseType { get => _releaseType; set => _releaseType = value; }
        public int Major { get => _major; set => _major = value; }
        public int Minor { get => _minor; set => _minor = value; }
        public int Patch { get => _patch; set => _patch = value; }
        public DateTime ReleaseDateTime { get => _releaseDateTime; set => _releaseDateTime = value; }
    }
}
