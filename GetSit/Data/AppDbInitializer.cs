using GetSit.Common;
using GetSit.Models;

namespace GetSit.Data
{
    public class AppDbInitializer
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDBcontext>();
                context.Database.EnsureCreated();

                //Space 
                if (!context.Space.Any())
                {
                    context.Space.AddRange(new List<Space>()
                    {
                        new Space
                        {
                            Name = "Spaceko",
                            Bio = "Book and Work .. fast and easy",
                            Country="Egypt",
                            City="Qena",
                            Street="Al Tagned",
                            GPSLocation="26.172879278940762, 32.730838529460435",
                            IsFast=true,
                            BankAccount="1111222233334444"
                        },
                        new Space
                        {
                            Name = "Meto",
                            Bio = "Come and study or work",
                            Country="Egypt",
                            City="Qena",
                            Street="Omar Effendi",
                            GPSLocation="26.164065, 32.719766",
                            IsFast=true,
                            BankAccount="1111222233334444"
                        }
                    });
                    context.SaveChanges();
                }
                //Space Employee
                context.SpaceEmployee.AddRange(new List<SpaceEmployee>()
                {
                    new SpaceEmployee
                    {
                        FirstName="Ahmed",
                        LastName="Mahmoud",
                        Email="Provider@meto.com",
                        Password= PasswordHashing.Encode("Provider1234"),
                        Birthdate= new DateTime(1980,7,7),
                        PhoneNumber= "01013205017",
                        SpaceId=2
                    }
                });
                context.SaveChanges();
                //Space Phone
                if (!context.SpacePhone.Any())
                {
                    context.SpacePhone.AddRange(new List<SpacePhone>()
                    {
                        new SpacePhone
                        {
                            SpaceId=1,
                            PhoneNumber="01011223333"
                        },
                        new SpacePhone
                        {
                            SpaceId=1,
                            PhoneNumber="01345678911"
                        },
                        new SpacePhone
                        {
                            SpaceId=2,
                            PhoneNumber="01010101011"
                        },
                    });
                    context.SaveChanges();
                }
                //Space WorkingDay
                if (!context.SpaceWorkingDay.Any())
                {
                    context.SpaceWorkingDay.AddRange(new List<SpaceWorkingDay>()
                    {
                        //space 1
                        new SpaceWorkingDay
                        {
                            SpaceId=1,
                            Day=DayOfWeek.Saturday,
                            OpeningTime=new TimeSpan(8,0,0),
                            ClosingTime=new TimeSpan(22,0,0),
                        },
                        new SpaceWorkingDay
                        {
                            SpaceId=1,
                            Day=DayOfWeek.Sunday,
                             OpeningTime=new TimeSpan(10,0,0),
                            ClosingTime=new TimeSpan(22,0,0),
                        },
                        new SpaceWorkingDay
                        {
                            SpaceId=1,
                            Day=DayOfWeek.Tuesday,
                             OpeningTime=new TimeSpan(12,0,0),
                            ClosingTime=new TimeSpan(20,0,0),
                        },
                        new SpaceWorkingDay
                        {
                            SpaceId=1,
                            Day=DayOfWeek.Monday,
                             OpeningTime=new TimeSpan(8,0,0),
                            ClosingTime=new TimeSpan(20,0,0),
                        },
                        new SpaceWorkingDay
                        {
                            SpaceId=1,
                            Day=DayOfWeek.Thursday,
                             OpeningTime=new TimeSpan(8,0,0),
                            ClosingTime=new TimeSpan(22,0,0),
                        },
                        new SpaceWorkingDay
                        {
                            SpaceId=1,
                            Day=DayOfWeek.Wednesday,
                             OpeningTime=new TimeSpan(8,0,0),
                            ClosingTime=new TimeSpan(22,0,0),
                        },
                        //space 2
                        new SpaceWorkingDay
                        {
                            SpaceId=2,
                            Day=DayOfWeek.Saturday,
                            OpeningTime=new TimeSpan(8,0,0),
                            ClosingTime=new TimeSpan(22,0,0),
                        },
                        new SpaceWorkingDay
                        {
                            SpaceId=2,
                            Day=DayOfWeek.Sunday,
                             OpeningTime=new TimeSpan(8,0,0),
                            ClosingTime=new TimeSpan(20,0,0),
                        },
                        new SpaceWorkingDay
                        {
                            SpaceId=2,
                            Day=DayOfWeek.Tuesday,
                             OpeningTime=new TimeSpan(8,0,0),
                            ClosingTime=new TimeSpan(20,0,0),
                        },
                        new SpaceWorkingDay
                        {
                            SpaceId=2,
                            Day=DayOfWeek.Monday,
                            OpeningTime=new TimeSpan(8,0,0),
                            ClosingTime=new TimeSpan(20,0,0),
                        },
                        new SpaceWorkingDay
                        {
                            SpaceId=2,
                            Day=DayOfWeek.Thursday,
                             OpeningTime=new TimeSpan(8,0,0),
                            ClosingTime=new TimeSpan(22,0,0),
                        },
                        new SpaceWorkingDay
                        {
                            SpaceId=2,
                            Day=DayOfWeek.Wednesday,
                             OpeningTime=new TimeSpan(8,0,0),
                            ClosingTime=new TimeSpan(22,0,0),
                        },
                    });
                    context.SaveChanges();
                }
                //Space Photos
                if (!context.SpacePhoto.Any())
                {
                    context.SpacePhoto.AddRange(new List<SpacePhoto>()
                    {
                        new SpacePhoto
                        {
                            SpaceId=1,
                            Url="./resources/space.webp"
                        },
                        new SpacePhoto
                        {
                            SpaceId=2,
                            Url="./resources/space1.webp"
                        },
                        new SpacePhoto
                        {
                            SpaceId=2,
                            Url = "./resources/space.webp"
                        }
                    });
                    context.SaveChanges();
                }

                //Hall
                if (!context.SpaceHall.Any())
                {
                    context.SpaceHall.AddRange(new List<SpaceHall>()
                    {
                        //space 1
                        new SpaceHall
                        {
                            SpaceId=1,
                            Capacity=30,
                            CostPerHour=300,
                            Description="Make your mentor sessions preparation easy",
                            Status=enums.HallStatus.Accepted,
                            Type=enums.HallType.ClassRoom
                        },
                        new SpaceHall
                        {
                            SpaceId = 1,
                            Capacity = 30,
                            CostPerHour = 300,
                            Description = "Make your mentor sessions preparation easy",
                            Status = enums.HallStatus.Accepted,
                            Type = enums.HallType.ClassRoom
                        },
                        new SpaceHall
                        {
                            SpaceId = 1,
                            Capacity = 15,
                            CostPerHour = 200,
                            Description = "Make your Meetings preparation easy",
                            Status = enums.HallStatus.Accepted,
                            Type = enums.HallType.MeetingRoom
                        },
                        new SpaceHall
                        {
                            SpaceId = 1,
                            Capacity = 15,
                            CostPerHour = 20,
                            Description = "Study in relief!",
                            Status = enums.HallStatus.Accepted,
                            Type = enums.HallType.Desk
                        },
                        
                        //space 2
                        new SpaceHall
                        {
                            SpaceId=2,
                            Capacity=30,
                            CostPerHour=250,
                            Description="Make your mentor sessions preparation easy",
                            Status=enums.HallStatus.Accepted,
                            Type=enums.HallType.ClassRoom
                        },
                        new SpaceHall
                        {
                            SpaceId = 2,
                            Capacity = 20,
                            CostPerHour = 150,
                            Description = "Make your mentor sessions preparation easy",
                            Status = enums.HallStatus.Accepted,
                            Type = enums.HallType.ClassRoom
                        },
                        new SpaceHall
                        {
                            SpaceId = 2,
                            Capacity = 10,
                            CostPerHour = 200,
                            Description = "Make your Meetings preparation easy",
                            Status = enums.HallStatus.Accepted,
                            Type = enums.HallType.MeetingRoom
                        },
                        new SpaceHall
                        {
                            SpaceId = 2,
                            Capacity = 30,
                            CostPerHour = 15,
                            Description = "Study in relief!",
                            Status = enums.HallStatus.Accepted,
                            Type = enums.HallType.Desk
                        },
                    });
                    context.SaveChanges();
                }
                //Hall Facility
                if (!context.HallFacility.Any())
                {
                    context.HallFacility.AddRange(new List<HallFacility>()
                    {
                        new HallFacility
                        {
                            HallId=1,
                            Facility=enums.Facility.FreeWifi
                        },
                        new HallFacility
                        {
                            HallId=1,
                            Facility=enums.Facility.UnlimitedInternet
                        },
                        new HallFacility
                        {
                            HallId=2,
                            Facility=enums.Facility.OpenAirView
                        },
                        new HallFacility
                        {
                            HallId=3,
                            Facility=enums.Facility.UnlimitedCoffee
                        },
                        new HallFacility
                        {
                            HallId=4,
                            Facility=enums.Facility.FreeWifi
                        },
                    });
                }
                //Hall photo
                if (!context.HallPhoto.Any())
                {
                    context.HallPhoto.AddRange(new List<HallPhoto>()
                    {
                        new HallPhoto
                        {
                            HallId=1,
                            Url="./resources/classroom.jpg"
                        },
                        new HallPhoto
                        {
                            HallId=1,
                            Url="./resources/classroom1.jpg"
                        },
                        new HallPhoto
                        {
                            HallId=2,
                            Url="./resources/classroom1.jpg"
                        },
                        new HallPhoto
                        {
                            HallId=3,
                            Url="./resources/meetingRoom.webp"
                        },
                        new HallPhoto
                        {
                            HallId=4,
                            Url="./resources/Desk1.webp"
                        },
                        new HallPhoto
                        {
                            HallId=4,
                            Url="./resources/Desk2.webp"
                        },
                        new HallPhoto
                        {
                            HallId=5,
                            Url="./resources/classroom3.webp"
                        },
                        new HallPhoto
                        {
                            HallId=5,
                            Url="./resources/classroom2.webp"
                        },
                        new HallPhoto
                        {
                            HallId=6,
                            Url="./resources/classroom.webp"
                        },
                        new HallPhoto
                        {
                            HallId=7,
                            Url="./resources/meetingRoom.webp"
                        },
                        new HallPhoto
                        {
                            HallId=8,
                            Url="./resources/Desk3.webp"
                        },
                        new HallPhoto
                        {
                            HallId=8,
                            Url="./resources/Desk4.webp"
                        },
                    });
                    context.SaveChanges();
                }

                //Service
                if (!context.SpaceService.Any())
                {
                    context.SpaceService.AddRange(new List<SpaceService>()
                    {
                         new SpaceService
                         {
                             SpaceId=1,
                             Name="Mango Juice",
                             Description="Frish Juice",
                             Price=30,
                         },
                         new SpaceService
                         {
                             SpaceId=1,
                             Name="Oreo Juice",
                             Description="Not Frish Juice",
                             Price=50,
                         },
                         new SpaceService
                         {
                             SpaceId=2,
                             Name="Mango Juice",
                             Description="Frish Juice",
                             Price=30,
                         },
                         new SpaceService
                         {
                             SpaceId=1,
                             Name="Projector",
                             Description="Not Frish Projector",
                             Price=100,
                         }
                    });
                    context.SaveChanges();
                }
                //Service Photo
                if (!context.ServicePhoto.Any())
                {
                    context.ServicePhoto.AddRange(new List<ServicePhoto>()
                    {
                         new ServicePhoto
                         {
                             ServiceId=1,
                             Url="./resources/manogJuice.jpg"
                         },
                         new ServicePhoto
                         {
                             ServiceId=2,
                             Url="./resources/OreoJuice.jpg",
                         },
                         new ServicePhoto
                         {
                             ServiceId=3,
                             Url="./resources/manogJuice.jpg"
                         },
                         new ServicePhoto
                         {
                             ServiceId=4,
                             Url="./resources/Projector.jpg"
                         }
                    });
                    context.SaveChanges();
                }
            }
        }
    }
}