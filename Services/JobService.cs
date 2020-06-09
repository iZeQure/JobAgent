using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JobAgent.Data;

namespace JobAgent.Services
{
    public class JobService
    {
        public Task<List<Job>> GetJobMenuAsync()
        {
            List<Job> tempJobs = new List<Job>
            {
                new Job()
                {
                    JobTitle = "Data & Kommunikation",
                    JobCategories = new List<JobCategory>()
                {
                    new JobCategory()
                    {
                        Menus = new string[]
                        {
                            "Supporter",
                            "Infrastruktur",
                            "Programmering"
                        }
                    }
                }
                },

                new Job()
                {
                    JobTitle = "Bager & Konditor",
                    JobCategories = new List<JobCategory>()
                {
                    new JobCategory()
                    {
                        Menus = new string[]
                        {
                            "Bager",
                            "Konditor"
                        }
                    }
                }
                },

                new Job()
                {
                    JobTitle = "Detail",
                    JobCategories = new List<JobCategory>()
                {
                    new JobCategory()
                    {
                        Menus = null
                    }
                }
                }
            };

            return Task.FromResult(tempJobs);
        }

        public Task<List<JobPosting>> GetJobPostingsAsync()
        {
            List<JobPosting> temp = new List<JobPosting>
            {
                new JobPosting()
                {
                    Id = 1,
                    Title = "Test",
                    ImageURL = "https://www.zbc.dk/media/3529/050919_elektriker_slagelse_44.jpg",
                    Company = "Zealand Business College",
                    Catetory = "Programmering",
                    Information = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.",
                    StudentsNeeded = 2,
                    DateTimeForPost = DateTime.Now
                },

                new JobPosting()
                {
                    Id = 2,
                    Title = "Test",
                    ImageURL = "https://www.zbc.dk/media/3529/050919_elektriker_slagelse_44.jpg",
                    Company = "Zealand Business College",
                    Catetory = "Programmering",
                    Information = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Tincidunt praesent semper feugiat nibh.",
                    StudentsNeeded = 2,
                    DateTimeForPost = DateTime.Now
                },

                new JobPosting()
                {
                    Id = 3,
                    Title = "Test",
                    ImageURL = "https://www.zbc.dk/media/3529/050919_elektriker_slagelse_44.jpg",
                    Company = "Zealand Business College",
                    Catetory = "Programmering",
                    Information = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Sed nisi lacus sed viverra tellus in hac habitasse. Praesent semper feugiat nibh sed pulvinar proin gravida.",
                    StudentsNeeded = 2,
                    DateTimeForPost = DateTime.Now
                },

                new JobPosting()
                {
                    Id = 4,
                    Title = "Test",
                    ImageURL = "https://www.zbc.dk/media/3529/050919_elektriker_slagelse_44.jpg",
                    Company = "Zealand Business College",
                    Catetory = "Programmering",
                    Information = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Eu facilisis sed odio morbi quis. Pulvinar mattis nunc sed blandit libero volutpat sed cras ornare. Elementum tempus egestas sed sed risus pretium quam vulputate dignissim.",
                    StudentsNeeded = 2,
                    DateTimeForPost = DateTime.Now
                },

                new JobPosting()
                {
                    Id = 5,
                    Title = "Test",
                    ImageURL = "https://www.zbc.dk/media/3529/050919_elektriker_slagelse_44.jpg",
                    Company = "Zealand Business College",
                    Catetory = "Programmering",
                    Information = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Magna fringilla urna porttitor rhoncus dolor purus. Ornare massa eget egestas purus viverra accumsan in. Sit amet tellus cras adipiscing. Nisi scelerisque eu ultrices vitae auctor eu augue ut lectus.",
                    StudentsNeeded = 2,
                    DateTimeForPost = DateTime.Now
                },

                new JobPosting()
                {
                    Id = 6,
                    Title = "Debug",
                    ImageURL = "https://www.w3schools.com/w3css/img_lights.jpg",
                    Company = "W3Schools",
                    Catetory = "Supporter",
                    Information = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Magna fringilla urna porttitor rhoncus dolor purus. Ornare massa eget egestas purus viverra accumsan in. Sit amet tellus cras adipiscing. Nisi scelerisque eu ultrices vitae auctor eu augue ut lectus.",
                    StudentsNeeded = 0,
                    DateTimeForPost = DateTime.Now
                }
            };

            return Task.FromResult(temp);
        }
    }
}
