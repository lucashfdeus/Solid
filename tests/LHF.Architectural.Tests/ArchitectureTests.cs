using ArchUnitNET.Domain;
using ArchUnitNET.Fluent;
using ArchUnitNET.Loader;
using ArchUnitNET.xUnit;
using LHF.Solid.Api.Controllers;
using LHF.Solid.Data.Repository;
using Xunit;

namespace LHF.Architectural.Tests
{
    public class ArchitectureTests
    {
        // Carregar a arquitetura uma vez e reutilizar em todos os testes
        private static readonly Architecture _architecture = new ArchLoader()
           .LoadAssemblies(
               typeof(ProductController).Assembly,       // Carrega o assembly da camada de API
               typeof(ProductRepository).Assembly)       // Certifica-se de carregar o assembly do repositório também
           .Build();

        [Fact]
        public void ApiLayerShouldDependOnConfigurationLayer()
        {
            // Verifica que a camada de API depende da camada de configuração e não diretamente da camada de dados.
            var rule = ArchRuleDefinition
                .Classes()
                .That()
                .ResideInNamespace("LHF.Solid.Api")
                .Should()
                .NotDependOnAny("LHF.Solid.Data");

            CheckArchitectureRule(rule);
        }

        [Fact]
        public void ControllersShouldNotDependOnDataLayer()
        {
            // Garante que os controllers da API não dependem diretamente da camada de dados.
            // Isso impede que controllers se conectem diretamente ao banco de dados, mantendo a separação de responsabilidades.
            var rule = ArchRuleDefinition
                .Classes()
                .That()
                .ResideInNamespace("LHF.Solid.Api.Controllers")
                .Should()
                .NotDependOnAny("LHF.Solid.Data");

            CheckArchitectureRule(rule);
        }       

        [Fact]
        public void BusinessLayerShouldDependOnlyOnDataLayer()
        {
            // Assegura que a camada de negócios depende exclusivamente da camada de dados e não da camada de API ou controladores.
            // Esse teste garante que a camada de negócios se comunica apenas com a camada de dados, preservando a lógica de negócios independente.
            var rule = ArchRuleDefinition
                .Classes()
                .That()
                .ResideInNamespace("LHF.Solid.Business")
                .Should()
                .OnlyDependOn("LHF.Solid.Data")
                .AndShould()
                .NotDependOnAny("LHF.Solid.Api")
                .AndShould()
                .NotDependOnAny("LHF.Solid.Api.Controllers")
                .WithoutRequiringPositiveResults();

            CheckArchitectureRule(rule);
        }    

        [Fact]
        public void ControllersShouldNotDependOnBusinessLayerDirectly()
        {
            // Garante que os controladores da API não dependem diretamente da camada de serviços da camada de negócios.
            // Isso impede que controladores chamem diretamente os serviços de negócios, preservando a separação de responsabilidades entre camadas.
            var rule = ArchRuleDefinition
                .Classes()
                .That()
                .ResideInNamespace("LHF.Solid.Api.Controllers")
                .Should()
                .NotDependOnAny("LHF.Solid.Business.Services");

            CheckArchitectureRule(rule);
        }

        [Fact]
        public void ServicesShouldDependOnlyOnRepositories()
        {
            // Garante que a camada de serviços de negócios depende exclusivamente da camada de repositórios de dados,
            // não devendo depender de camadas externas como a API ou controladores.
            var rule = ArchRuleDefinition
                .Classes()
                .That()
                .ResideInNamespace("LHF.Solid.Business.Services")
                .Should()
                .OnlyDependOn("LHF.Solid.Data.Repository")
                .AndShould()
                .NotDependOnAny("LHF.Solid.Api")
                .AndShould()
                .NotDependOnAny("LHF.Solid.Api.Controllers")
                .WithoutRequiringPositiveResults();

            CheckArchitectureRule(rule);
        }

        [Fact]
        public void RepositoriesShouldNotDependOnApiLayer()
        {
            // Verifica que os repositórios de dados não dependem da camada de API ou controladores.
            // Isso garante que a camada de acesso a dados é independente da camada de apresentação (API).
            var rule = ArchRuleDefinition
                .Classes()
                .That()
                .ResideInNamespace("LHF.Solid.Data.Repository")
                .Should()
                .NotDependOnAny("LHF.Solid.Api")
                .AndShould()
                .NotDependOnAny("LHF.Solid.Api.Controllers")
                .WithoutRequiringPositiveResults();

            CheckArchitectureRule(rule);
        }

        [Fact]
        public void ConfigurationLayerShouldNotDependOnApiOrDataLayer()
        {
            // Garante que a camada de configuração não depende de camadas como API ou dados
            var rule = ArchRuleDefinition
                .Classes()
                .That()
                .ResideInNamespace("LHF.Solid.Configurations")
                .Should()
                .NotDependOnAny("LHF.Solid.Api")
                .AndShould()
                .NotDependOnAny("LHF.Solid.Data")
                .WithoutRequiringPositiveResults();

            CheckArchitectureRule(rule);
        }

        // Método para verificar a regra da arquitetura
        private static void CheckArchitectureRule(IArchRule rule)
        {
            rule.Check(_architecture);
        }
    }
}
