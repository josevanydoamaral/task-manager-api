using Xunit;
using TaskManager.Domain;

namespace TaskManager.Tests;

public class PayrollServiceTests
{
    [Fact]
    public void CalculateNetSalary_SalarioAcimaDe800_DeveAplicarTaxa()
    {
        //--- Arrange ---
        var service = new PayrollService();

        //--- Act ---
        var result = service.CalculateNetSalary(1000m);


        //--- Assert ---
        Assert.Equal(890m, result.NetSalary);
    }

    [Fact]
    public void CalculateNetSalary_SalarioAbaixoDe800_NaoDeveAplicarTaxa()
    {
        // Arrange
        var payrollService = new PayrollService();

        // Act
        var netSalary = payrollService.CalculateNetSalary(700m);


        // Assert
        Assert.Equal(700m, netSalary.NetSalary);
    }
}