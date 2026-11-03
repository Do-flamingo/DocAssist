namespace DocAssist.Cli.Tests;

public class CalculCoutTests
{
    [Fact]
    public void Estimer_additionne_entree_et_sortie()
    {
        decimal cout = CalculCout.Estimer(1_000_000, 500_000, 0.40m, 1.60m);
        Assert.Equal(1.20m, cout);
    }

    [Fact]
    public void Estimer_sans_tokens_vaut_zero()
        => Assert.Equal(0m, CalculCout.Estimer(0, 0, 0.40m, 1.60m));
}