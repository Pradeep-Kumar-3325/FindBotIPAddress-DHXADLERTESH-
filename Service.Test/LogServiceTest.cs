using BOTIP.Services.Concerte;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;

namespace Service.Test
{
    public class LogFileServiceTest
    {
        private readonly Mock<ILogger<LogFileService>> logger;
        
        private readonly Mock<IConfiguration> config;
        LogFileService service = null;

        //Initialization
        public LogFileServiceTest()
        {
            logger = new Mock<ILogger<LogFileService>>();
            config = new Mock<IConfiguration>();
            service = new LogFileService(logger.Object, config.Object);
        }

        [Fact]
        public async Task Process_SucessFully_Return_suspiciousIps_When_File_Exist()
        {
            //Arrange
            var correctpath= "C:\\Users\\prade\\source\\repos\\FindBotIPAddress(DHXADLERTESH)\\FindBotIPAddress(DHXADLERTESH)\\bin\\restolabs_php7.0.access.log";

            //Act
            var response = this.service.Process(correctpath);


            //Assert
            Assert.NotNull(response);
            Assert.NotNull(response.Count > 0);

        }

        [Fact]
        public async Task Process_Throw_Exception_When_File_NotExist()
        {
            //Arrange
            var path = "Filepath";
            //Act and Assert
            Assert.Throws<FileNotFoundException>(() => this.service.Process(path));

        }
    }
}