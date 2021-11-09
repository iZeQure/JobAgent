using JobAgentClassLibrary.Common.Locations.Entities;
using JobAgentClassLibrary.Common.Roles.Entities;
using JobAgentClassLibrary.Common.Users.Entities;
using NUnit.Framework;

namespace JobAgentClassLibraryTests
{
    public class UserObjectCastTests
    {
        [Test]
        public void IUserObject_NewObject_ShouldCastToAuthUser()
        {
            //Arrange
            IUser tempUser = new AuthUser
            {
                FirstName = "Jens",
                LastName = "Jensen",
                Email = "JensJensen@gmail.com",
                LocationId = 2,
                RoleId = 2,
                Password = "lololol123123"
            };

            //Act

            //Assert
            Assert.IsNotNull(tempUser as AuthUser);
            Assert.AreEqual(((AuthUser)tempUser).Password, "lololol123123");

        }
    }
}
