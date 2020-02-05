using DesafioStone.Domain.Entity;
using System;
using Xunit;

namespace DesafioStone.Domain.test
{
    public class TesteEntity
    {
        [Fact]
        public void TestInstanciaEdificio()
        {
            Edificio edificio = new Edificio("Edifio teste", "Rua teste do testei", "tes", "tesil");

            Assert.Equal(edificio.Nome, "Edifio teste");
            Assert.Equal(edificio.Endereco, "Rua teste do testei");
            Assert.Equal(edificio.UF, "tes");
            Assert.Equal(edificio.Pais, "tesil");
        }

        [Fact]
        public void TestInstanciaEdificioIniciaAtivo()
        {
            Edificio edificio = new Edificio("Edifio teste", "Rua teste do testei", "tes", "tesil");

            Assert.Equal(edificio.Ativo, true);
            Assert.Equal(edificio.Ativo, true);
            Assert.Equal(edificio.Ativo, true); Assert.Equal(edificio.Ativo, true); Assert.Equal(edificio.Ativo, true);
        }

        [Fact]
        public void TestInstanciaEdificioCorretamente()
        {
            Edificio edificio = new Edificio("Edifio teste", "Rua teste do testei", "tes", "tesil");

            Assert.NotNull(edificio);
        }

        [Fact]
        public void TestInstanciaNivel()
        {
            Nivel nivel = new Nivel("Nivel", "Nivel de teste");

            Assert.Equal(nivel.Nome, "Nivel");
            Assert.Equal(nivel.Descricao, "Nivel de teste");
        }

        [Fact]
        public void TestSetEdificiIdInNivel()
        {
            Nivel nivel = new Nivel("Nivel", "Nivel de teste");

            Assert.Null(nivel.EdificioId);
        }

        [Fact]
        public void TestInstanciaNivelCorretamente()
        {
            Nivel nivel = new Nivel("Nivel", "Nivel de teste");

            Assert.NotNull(nivel);
        }

        [Fact]
        public void TestInstanciaItemCorretamente()
        {
            Item item = new Item("item teste", "iten sendo testando ao máximo AAAAAAA");

            Assert.NotNull(item);
        }

        [Fact]
        public void TestInstanciaItem()
        {
            Item item = new Item("item teste", "iten sendo testando ao máximo AAAAAAA");

            Assert.Equal(item.Nome, "item teste");
            Assert.Equal(item.Descricao, "iten sendo testando ao máximo AAAAAAA");
        }
    }
}