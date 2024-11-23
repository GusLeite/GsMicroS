using GsMS;
using Moq; // Adicione este using
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit; // Adicione este using
using Microsoft.AspNetCore.Mvc; // Adicione este using
using Microsoft.AspNetCore.Mvc.Abstractions; // Adicione este using
using FluentAssertions; // Adicione este using

namespace GsMSTests
{
    public class ConsumoControllerTests
    {
        private readonly Mock<MongoDBService> _mockMongoDBService;
        private readonly Mock<RedisCacheService> _mockRedisCacheService;
        private readonly ConsumoController _controller;

        public ConsumoControllerTests()
        {
            _mockMongoDBService = new Mock<MongoDBService>();
            _mockRedisCacheService = new Mock<RedisCacheService>();
            _controller = new ConsumoController(_mockMongoDBService.Object, _mockRedisCacheService.Object);
        }

        [Fact]
        public async Task Create_ShouldReturnBadRequest_WhenValorEmKwhIsZero()
        {
            var consumo = new Consumo { Id = 1, ValorEmKwh = 0, Nome = "Teste", local = "Local" };

            var result = await _controller.Create(consumo);

            result.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Get_ShouldReturnOk_WhenDataIsCached()
        {
            var cacheKey = "consumos";
            var cachedData = "[{\"Id\":1,\"ValorEmKwh\":100,\"Nome\":\"Teste\",\"local\":\"Local\"}]";

            _mockRedisCacheService.Setup(r => r.GetCacheAsync(cacheKey)).ReturnsAsync(cachedData);

            var result = await _controller.Get();

            result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task GetById_ShouldReturnNotFound_WhenConsumoDoesNotExist()
        {
            _mockMongoDBService.Setup(m => m.GetAsync()).ReturnsAsync(new List<Consumo>());

            var result = await _controller.GetById(1);

            result.Should().BeOfType<NotFoundResult>();
        }
    }

}