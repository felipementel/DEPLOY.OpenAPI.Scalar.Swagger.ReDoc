# Testes de Unidade - DEPLOY.OpenAPI.Scalar.Swagger.ReDoc

Este projeto contém testes de unidade abrangentes criados com XUnit para a aplicação DEPLOY.OpenAPI.Scalar.Swagger.ReDoc.

## Estrutura dos Testes

### 📁 Models/
- **AuthorTests.cs** - 8 testes para o modelo Author
  - Inicialização de propriedades
  - Definição de propriedades  
  - Gerenciamento da coleção de livros
  - Validação de tipos de dados

- **BookTests.cs** - 27 testes para o modelo Book
  - Inicialização de propriedades
  - Validação de título, preço, data de publicação
  - Testes para todos os gêneros de livros
  - Testes com dados completos

- **BookGenreTests.cs** - 14 testes para o enum BookGenre
  - Verificação de todos os valores do enum
  - Contagem de gêneros
  - Valor padrão
  - Conversão ToString()

### 📁 Database/
- **DEPLOYContextTests.cs** - 9 testes para o contexto do Entity Framework
  - Operações CRUD para Author
  - Operações CRUD para Book
  - Consultas de dados
  - Uso de banco em memória para testes

## Resumo dos Testes

- **Total de Testes**: 58
- **Testes Aprovados**: 58 ✅
- **Testes Falharam**: 0
- **Cobertura**: Modelos de domínio e operações de banco de dados

## Tecnologias Utilizadas

- **XUnit** - Framework de testes
- **Entity Framework Core InMemory** - Banco de dados em memória para testes
- **.NET 8.0** - Plataforma de desenvolvimento

## Como Executar os Testes

```bash
# Executar todos os testes
dotnet test

# Executar com detalhes
dotnet test --logger "console;verbosity=detailed"

# Executar somente os testes de modelo
dotnet test --filter "FullyQualifiedName~Models"

# Executar somente os testes de banco de dados
dotnet test --filter "FullyQualifiedName~Database"
```

## Funcionalidades Testadas

✅ Modelo Author com coleção de livros  
✅ Modelo Book com todas as propriedades  
✅ Enum BookGenre com 11 gêneros  
✅ Contexto de banco de dados DEPLOYContext  
✅ Operações CRUD para Author e Book  
✅ Validação de tipos de dados  
✅ Inicialização de propriedades  
✅ Relacionamentos entre entidades  

Os testes garantem que a lógica de negócio central da aplicação funciona corretamente.