# 🌍 TravelBuddy

## 📌 Sobre o Projeto

O **TravelBuddy** é uma solução em .NET para gerenciamento de reservas de viagens e consulta de informações de destinos, incluindo previsão do tempo por cidade. A arquitetura é organizada em múltiplos projetos e camadas bem definidas:

### APIs

* **TravelBuddy.Destinations.Api**: expõe endpoints REST para consulta de destinos e clima.
* **TravelBuddy.Reservations.Api**: expõe endpoints REST para cadastro e consulta de reservas.

### Camada de Aplicação / Domínio

* **TravelBuddy.Application** — contratos e orquestração de casos de uso (serviços de destinos e reservas).

### Persistência

* **TravelBuddy.Infrastructure.Persistence** — mapeamentos, DbContext e repositórios (EF Core + Oracle).

### Integrações externas

* **TravelBuddy.Infrastructure.Integration** — cliente HTTP para integração com provedores de clima (OpenMeteo).

### Contratos (DTOs)

* **TravelBuddy.Shared.Contracts** — requests/responses compartilhados entre camadas.

### Interface Web (MVC)

* **TravelBuddy.Web.Mvc** — interface web interna para consultar destinos, criar e listar reservas.

---

## 🏢 Aplicação Interna

A aplicação foi pensada para uso interno:

* Usuários podem consultar o clima de destinos antes de criar reservas.
* Permite criar, listar, editar e excluir reservas, vinculadas a destinos cadastrados.
* Todas as operações passam pelas APIs dedicadas e seguem separação clara de responsabilidades.

---

## 🔗 Rotas da API

### 👥 Reservas (API Reservations)

| Método | Rota                   | Descrição                 | Status HTTP Esperado          |
| ------ | ---------------------- | ------------------------- | ----------------------------- |
| GET    | /api/reservations      | Lista todas as reservas   | 200 OK                        |
| GET    | /api/reservations/{id} | Obtém uma reserva pelo ID | 200 OK / 404 Not Found        |
| POST   | /api/reservations      | Cria uma nova reserva     | 201 Created / 400 Bad Request |

### 📫 Destinos (API Destinations)

| Método | Rota                     | Descrição                                    | Status HTTP Esperado   |
| ------ | ------------------------ | -------------------------------------------- | ---------------------- |
| GET    | /api/destinations/{city} | Retorna dados do destino e previsão do tempo | 200 OK / 404 Not Found |

Formato de resposta esperado:

```json
{
  "id": "GUID-do-destino",
  "name": "São Paulo",
  "country": "Brasil",
  "description": "Capital paulista",
  "averagePrice": 1500.00
}
```

Caso o destino/cidade não exista, a API retorna **404 Not Found**.

---

## 📋 Pré‑requisitos

* .NET SDK 8.0+ instalado
* Oracle acessível (instância/serviço) para persistência
* Ferramenta de banco (opcional): SQL Developer, DBeaver etc.

---

## ⚙️ Como Instalar e Rodar

1. **Clonar o repositório**

```bash
git clone https://github.com/seu-usuario/travelbuddy.git
cd travelbuddy
```

2. **Configurar a conexão com Oracle**
   Edite o `appsettings.json` das APIs para definir a string de conexão:

```json
"ConnectionStrings": {
  "OracleConnection": "User Id=<usuario>;Password=<senha>;Data Source=//<host>:<porta>/<ServiceName>;"
}
```

> Você pode usar Secret Manager ou variáveis de ambiente para não versionar credenciais.

3. **Aplicar migrations e subir as APIs**

```bash
# Cria/atualiza o schema no Oracle
dotnet ef database update -p TravelBuddy.Infrastructure.Persistence -s TravelBuddy.Reservations.Api

# Sobe a API de Reservas
dotnet run --project TravelBuddy.Reservations.Api

# Em outro terminal, sobe a API de Destinos
dotnet run --project TravelBuddy.Destinations.Api
```

4. **Acessar Swagger**

* API de Reservas: `http://localhost:{porta}/swagger`
* API de Destinos: `http://localhost:{porta}/swagger`

As portas dependem do `launchSettings.json` ou configuração local.

---

## ✅ Exemplo de Fluxo

1. **Consultar destino e clima**

```
GET /api/destinations/Sao%20Paulo
```

Resposta: JSON com informações do destino ou 404.

2. **Criar reserva**

```
POST /api/reservations
Content-Type: application/json

{
  "destinationId": "GUID-do-destino",
  "customerName": "João Silva",
  "travelDate": "2025-09-01",
  "numberOfPeople": 2,
  "totalPrice": 3000.00
}
```

Resposta: **201 Created** com o ID da reserva gerado.

3. **Consultar reserva por ID**

```
GET /api/reservations/{id}
```

---

## 📐 Princípios SOLID Aplicados

* **SRP — Single Responsibility Principle**

  * Controllers tratam apenas HTTP.
  * Serviços de aplicação concentram regras de negócio.
  * Repositórios cuidam exclusivamente da persistência.
  * Clientes de integração encapsulam chamadas externas.

* **OCP — Open/Closed Principle**

  * Serviços dependem de interfaces (`IReservationRepository`, `IOpenMeteoClient`).
  * Extensões podem ser feitas sem alterar o código existente.

* **DIP — Dependency Inversion Principle**

  * Camadas de alto nível dependem de abstrações, não de detalhes concretos.
  * Implementações concretas (EF Core, HttpClient) são injetadas via DI.

---

## 🧭 Estrutura (visão geral)

```
travelbuddy/
├── TravelBuddy.Application
├── TravelBuddy.Domain
├── TravelBuddy.Infrastructure.Persistence
├── TravelBuddy.Infrastructure.Integration
├── TravelBuddy.Shared.Contracts
├── TravelBuddy.Destinations.Api
├── TravelBuddy.Reservations.Api
└── TravelBuddy.Web.Mvc
```

---

## 🧪 Boas práticas

* Swagger habilitado em ambas as APIs (`/swagger`).
* Validações e mensagens de erro claras (400/404/201 etc.).
* DTOs dedicados em `Shared.Contracts` para desacoplar camadas.
* DI configurada para serviços, repositórios e integrações.
* Configurações e portas ajustáveis via `launchSettings.json` e `appsettings*.json`.

---
