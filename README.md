# Desafio Técnico - Luxclusif

Repositório referente a desafio da FCamara Europa e Luxclusif, e cujo objetivo era avaliar minhas habilidades técnicas como pessoa candidata à vaga de Desenvolvedor Backend e capacidade de resolver problemas de forma eficiente e clara.

## Para começar
O primeiro passo é criar uma clone deste repositório. Siga os passos abaixo para fazer a clone:


Abra seu client do git e siga os comandos:
```sh
git clone --bare https://github.com/bonifiq/prova-backend.git
```

## Conhecendo o projeto
> Recomendo que você utilize o Visual Studio 2026 (pode ser a versão community). Você também precisa do .NET 10 instalado, ok? Ah, não esquece de instalar o pacote de desenvolvimento para o ASP NET durante a instalação do Visual Studio.

Ao abrir o projeto no Visual Studio, você pode notar que se trata de um projeto Web API do ASP NET. Você pode se orientar pela pasta `Controllers`. 

Antes de rodar o projeto, você precisa rodar as migrations. Para isso, primeiro instale o [EF Tools](https://learn.microsoft.com/en-us/ef/core/get-started/overview/install#get-the-entity-framework-core-tools):

```csharp
dotnet tool install --global dotnet-ef
```

Agora, pode rodar as migrations de fato:

```csharp
dotnet ef database update --project src/ProvaPub.Infrastructure/ProvaPub.Infrastructure.csproj --startup-project src/ProvaPub.Api/ProvaPub.Api.csproj
```

Pronto, o projeto já criou as tabelas e alguns registros no seu localDB.

Rode o projeto e, se tudo deu certo, você deverá ver uma página de documentação usado Scalar com as APIs que foram propostas para este teste.