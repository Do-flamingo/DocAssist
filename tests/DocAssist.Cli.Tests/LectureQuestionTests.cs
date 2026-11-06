namespace DocAssist.Cli.Tests;

public class LectureQuestionTests
{
    [Fact]
    public void Depuis_sans_argument_renvoie_null()
        => Assert.Null(LectureQuestion.Depuis([]));

    [Fact]
    public void Depuis_argument_vide_renvoie_null()
        => Assert.Null(LectureQuestion.Depuis([" "]));

    [Fact]
    public void Depuis_argument_reunis_par_des_espaces()
        => Assert.Equal("test espace debut fin", LectureQuestion.Depuis(["test", "espace", "debut", "fin"]));

    [Fact]
    public void Depuis_espace_au_debut_et_a_la_fin_retires()
        => Assert.Equal("Qu'est-ce qu'un Pod ?", LectureQuestion.Depuis(["  Qu'est-ce qu'un Pod ?  "]));

    [Fact]
    public void Depuis_cas_nominal()
        => Assert.Equal("Pod", LectureQuestion.Depuis(["Pod"]));

    [Fact]
    public void Depuis_ignore_les_arguments_vide_au_milieu()
        => Assert.Equal("test fin", LectureQuestion.Depuis(["test", " ", "fin"]));
}