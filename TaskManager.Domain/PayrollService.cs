
namespace TaskManager.Domain;

public class PayrollResult
{
    public decimal GrossSalary { get; set; }
    public decimal NetSalary { get; set; }
    public decimal TaxPaid { get; set; } 
}

public class PayrollService
{
    // No c# decimal é obrigatório para dinheiro.
    // O 'm' no final dos números indica que são literais decimais
    private const decimal TAX_RATE = 0.11m; // 11%

    public PayrollResult CalculateNetSalary(decimal grossSalary)
    {
        // Regra de Negócio: se o salário for menor n paga taxa
        decimal tax = 0;
        if (grossSalary > 800)
        {
            tax = grossSalary * TAX_RATE;
        }

        return new PayrollResult{
            GrossSalary = grossSalary,
            TaxPaid = tax,
            NetSalary = grossSalary - tax
        };
    }
}