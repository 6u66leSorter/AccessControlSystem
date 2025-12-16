using AccessControlSystem.Entities;
using NUnit.Framework;
using Moq;
using Microsoft.Extensions.Logging;
using AccessControlSystem.Services.Implementations;
using AccessControlSystem.Repositories.Interfaces;

namespace AccessControlSystem.Tests.Services;

[TestFixture]
public class AccessControlServiceTests
{
    private Mock<IEmployeeRepository> _employeeRepository;
    private Mock<IVehicleRepository> _vehicleRepository;
    private Mock<IVisitorRepository> _visitorRepository;
    private Mock<IAccessLogRepository> _accessLogRepository;
    private Mock<ILogger<AccessControlService>> _logger;

    private AccessControlService _service;

    [SetUp]
    public void Setup()
    {
        _employeeRepository = new Mock<IEmployeeRepository>();
        _vehicleRepository = new Mock<IVehicleRepository>();
        _visitorRepository = new Mock<IVisitorRepository>();
        _accessLogRepository = new Mock<IAccessLogRepository>();
        _logger = new Mock<ILogger<AccessControlService>>();

        _service = new AccessControlService(
            _employeeRepository.Object,
            _vehicleRepository.Object,
            _visitorRepository.Object,
            _accessLogRepository.Object,
            _logger.Object);
    }

    [Test]
    public async Task ValidateEmployeeAccessAsync_EmployeeNotFound_Denied()
    {
        _employeeRepository
            .Setup(r => r.GetByCardNumberAsync("123"))
            .ReturnsAsync((Employee)null);

        var result = await _service.ValidateEmployeeAccessAsync("123");

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Reason, Is.EqualTo("Сотрудник не найден"));

        _accessLogRepository.Verify(
            r => r.AddAsync(It.IsAny<AccessLog>()),
            Times.Once);
    }
    
    [Test]
    public async Task ValidateEmployeeAccessAsync_InactiveEmployee_Denied()
    {
        var employee = new Employee
        {
            Id = 1,
            FullName = "Иван Иванов",
            IsActive = false
        };

        _employeeRepository
            .Setup(r => r.GetByCardNumberAsync("123"))
            .ReturnsAsync(employee);

        var result = await _service.ValidateEmployeeAccessAsync("123");

        Assert.That(result.IsValid, Is.False);
        Assert.That(result.Reason, Is.EqualTo("Сотрудник неактивен"));
    }

    [Test]
    public async Task ValidateEmployeeAccessAsync_ActiveEmployee_Granted()
    {
        var employee = new Employee
        {
            Id = 1,
            FullName = "Иван Иванов",
            IsActive = true
        };

        _employeeRepository
            .Setup(r => r.GetByCardNumberAsync("123"))
            .ReturnsAsync(employee);

        var result = await _service.ValidateEmployeeAccessAsync("123");

        Assert.That(result.IsValid, Is.True);
        Assert.That(result.EmployeeName, Is.EqualTo("Иван Иванов"));
    }
    
    [Test]
    public void CreateEmployeeAsync_EmployeeAlreadyExists_Throws()
    {
        var employee = new Employee { CardNumber = "123" };

        _employeeRepository
            .Setup(r => r.GetByCardNumberAsync("123"))
            .ReturnsAsync(new Employee());

        Assert.ThrowsAsync<InvalidOperationException>(
            () => _service.CreateEmployeeAsync(employee));
    }

    [Test]
    public async Task CreateEmployeeAsync_NewEmployee_Created()
    {
        var employee = new Employee
        {
            CardNumber = "123",
            FullName = "Иван Иванов"
        };

        _employeeRepository
            .Setup(r => r.GetByCardNumberAsync("123"))
            .ReturnsAsync((Employee)null);

        _employeeRepository
            .Setup(r => r.AddAsync(It.IsAny<Employee>()))
            .ReturnsAsync(employee);

        var result = await _service.CreateEmployeeAsync(employee);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.CardNumber, Is.EqualTo("123"));
    }

    [Test]
    public void RegisterVehicleEntryAsync_ActiveVehicleExists_Throws()
    {
        _vehicleRepository
            .Setup(r => r.GetByVehicleNumberAsync("A123BC"))
            .ReturnsAsync(new VehiclePass { IsInternal = false });

        Assert.ThrowsAsync<InvalidOperationException>(() =>
            _service.RegisterVehicleEntryAsync(
                "A123BC", "Driver", "Org", false));
    }

    [Test]
    public async Task RegisterVehicleEntryAsync_NewVehicle_Created()
    {
        _vehicleRepository
            .Setup(r => r.GetByVehicleNumberAsync("A123BC"))
            .ReturnsAsync((VehiclePass)null);

        _vehicleRepository
            .Setup(r => r.AddAsync(It.IsAny<VehiclePass>()))
            .ReturnsAsync(new VehiclePass { Id = 1 });

        var result = await _service.RegisterVehicleEntryAsync(
            "A123BC", "Driver", "Org", false);

        Assert.That(result.Id, Is.EqualTo(1));
    }

    [Test]
    public async Task MarkVehicleExitAsync_VehicleExists_ReturnsTrue()
    {
        var vehicle = new VehiclePass { Id = 1, IsInternal = false };

        _vehicleRepository
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(vehicle);

        var result = await _service.MarkVehicleExitAsync(1);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task RegisterVisitorAsync_CreatesVisitor()
    {
        _visitorRepository
            .Setup(r => r.AddAsync(It.IsAny<Visitor>()))
            .ReturnsAsync(new Visitor { Id = 1 });

        var result = await _service.RegisterVisitorAsync(
            "Гость", "Компания", "Визит", "Контакт");

        Assert.That(result.Id, Is.EqualTo(1));
    }

    [Test]
    public async Task MarkVisitorExitAsync_VisitorExists_ReturnsTrue()
    {
        var visitor = new Visitor { Id = 1 };

        _visitorRepository
            .Setup(r => r.GetByIdAsync(1))
            .ReturnsAsync(visitor);

        var result = await _service.MarkVisitorExitAsync(1);

        Assert.That(result, Is.True);
    }

    [Test]
    public async Task GetActiveVehiclesAsync_ReturnsList()
    {
        _vehicleRepository
            .Setup(r => r.GetActiveVehiclesAsync())
            .ReturnsAsync(new List<VehiclePass>());

        var result = await _service.GetActiveVehiclesAsync();

        Assert.That(result, Is.Not.Null);
    }

}