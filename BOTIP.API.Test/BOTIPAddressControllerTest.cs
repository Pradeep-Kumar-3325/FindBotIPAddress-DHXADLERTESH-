using BOTIP.Services.Interface;
using FindBotIPAddress_DHXADLERTESH_.Controllers;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Principal;

namespace BOTIP.API.Test
{
    public class BotIPaddressControllerTest
    {
        private readonly Mock<ILogger<BotIPaddressController>> mocklogger;
        private readonly Mock<IWebHostEnvironment> mockenv;
        private readonly Mock<IBOTService> mockbotService;
        BotIPaddressController controller=null;

        //Initialization
        public BotIPaddressControllerTest()
        {
            mocklogger = new Mock<ILogger<BotIPaddressController>>();
            mockbotService = new Mock<IBOTService>();
            mockenv = new Mock<IWebHostEnvironment>();
            controller = new BotIPaddressController(mocklogger.Object, mockenv.Object, mockbotService.Object);
        }

        [Fact]
        public async Task Get_suspiciousIps_When_Return_Data_From_LogFileService()
        {
            //Arrange
            var suspiciousIps = new Dictionary<string, int>();
            suspiciousIps.Add("186.10.170.159", 100);
            mockenv.Setup(x => x.ContentRootPath)
               .Returns("Path");
            mockbotService.Setup(x => x.Process(It.IsAny<string>()))
               .Returns(suspiciousIps);

            //Act
            var response = this.controller.Get("filepath");
            

            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.suspiciousIps);
            Assert.True(response.suspiciousIps.ContainsKey("186.10.170.159"));
            
        }

        [Fact]
        public async Task Get_Throw_Exception_When_LogFileService_Throw_Exception()
        {
            //Arrange
            mockenv.Setup(x => x.ContentRootPath)
               .Returns("Path");
            mockbotService.Setup(x => x.Process(It.IsAny<string>()))
               .Throws(new Exception());

            //Act
            var response = this.controller.Get("filepath");

            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Error);
        }

    }
}