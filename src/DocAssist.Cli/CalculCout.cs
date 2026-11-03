public static class CalculCout
{
    public static decimal Estimer(long tokensEntree, long tokensSortie,
                                  decimal prixEntreeParMillion, decimal prixSortieParMillion)
        => tokensEntree * prixEntreeParMillion / 1_000_000m
         + tokensSortie * prixSortieParMillion / 1_000_000m;
}