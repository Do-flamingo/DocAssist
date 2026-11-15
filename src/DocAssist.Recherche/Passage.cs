using Microsoft.Extensions.VectorData;

public class Passage
{
    [VectorStoreKey]
    public int Cle { get; set; }

    [VectorStoreData]
    public string Source { get; set; } = "";

    [VectorStoreData]
    public string Titre { get; set; } = "";

    [VectorStoreData]
    public string Texte { get; set; } = "";

    [VectorStoreVector(dimensions: 1536, DistanceFunction = DistanceFunction.CosineSimilarity)]
    public ReadOnlyMemory<float> Vecteur { get; set; }
}