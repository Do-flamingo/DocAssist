
public class LectureQuestionTests
{
    [Fact]
    public void Depuis_sans_argument_renvoie_null()
        => Assert.Null(LectureQuestion.Depuis([]));

    [Fact]
    public void Depuis_argument_vide_renvoie_null()
        => Assert.Null(LectureQuestion.Depuis([""]));

        // À toi : plusieurs arguments réunis par des espaces
    [Fact]
    public void Depuis_argument_reunis_par_des_espaces()
        => Assert.Equal("test espace debut fin", LectureQuestion.Depuis(["test", "espece", "debut", "fin"]));

    [Fact]
    public void Depuis_espace_au_debut_et_a_la_fin_retires()
        => Assert.Equal("test espace debut fin", LectureQuestion.Depuis([" ", " test", "espece", "debut", "fin ", " "]));
}