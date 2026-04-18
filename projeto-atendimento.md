# ZenitWpp

Plataforma de atendimento ao cliente via WhatsApp com automação por IA, gerenciamento de filas, suporte multimídia e análises em tempo real.

- Repositório: `git@github.com:brunoapolinariodev/ZenitWpp.git`
- Branch principal: `main` | Branch ativa: `develop`
- Estratégia de branches: **GitFlow**

---

## Funcionalidades

### 1. Interface de Usuário
- Botões interativos para opções frequentes
- Listas de seleção para múltiplas escolhas
- Menu principal com categorias de serviços

### 2. Automação e IA
- Respostas automáticas baseadas em palavras-chave
- Análise de sentimento para direcionar conversas
- Chatbot com aprendizado de máquina para melhorar respostas
- Sugestões de resposta para agentes humanos

### 3. Gerenciamento de Atendimento
- Fila de atendimento inteligente
- Transferência de conversas entre agentes
- Classificação automática de conversas
- Priorização de mensagens baseada em urgência/importância

### 4. Personalização
- Perfis de cliente com histórico de interações
- Mensagens personalizadas baseadas no perfil do cliente
- Fluxos de conversa customizáveis por segmento de cliente

### 5. Análise e Relatórios
- Dashboard em tempo real de métricas de atendimento
- Relatórios detalhados de desempenho e satisfação do cliente
- Análise de tendências de consultas e problemas comuns

### 6. Recursos Multimídia
- Suporte para envio e recebimento de imagens, áudios e vídeos
- Reconhecimento de texto em imagens (OCR)
- Transcrição automática de mensagens de voz

### 7. Agendamento e Lembretes
- Agendamento de compromissos via chat
- Sistema de lembretes automáticos

### 8. Segurança e Compliance
- Criptografia de ponta a ponta
- Conformidade com LGPD/GDPR
- Autenticação de dois fatores para agentes

### 9. Suporte Multilíngue
- Detecção automática de idioma
- Tradução em tempo real
- Suporte a múltiplos idiomas na interface do agente

### 10. Funcionalidades Avançadas
- Pesquisas de satisfação pós-atendimento
- Chatbot de voz para chamadas de áudio

### 11. Gestão de Campanhas
- Criação e envio de campanhas de marketing
- Segmentação de clientes para mensagens direcionadas
- Análise de engajamento de campanhas

---

## Stack Tecnológica

### WhatsApp
| Camada | Tecnologia |
|--------|-----------|
| Integração oficial | WhatsApp Business API (Meta Cloud) |
| Alternativa self-hosted | Evolution API |

### Backend — `apps/api`
| Camada | Tecnologia |
|--------|-----------|
| Linguagem | C# / .NET |
| Framework API | ASP.NET Core |
| Padrão arquitetural | Clean Architecture + DDD + CQRS |
| Mediator | MediatR |
| Validação | FluentValidation |
| Mapeamento | Mapster |
| ORM | Entity Framework Core |
| Banco relacional | PostgreSQL (Npgsql) |
| Cache / Filas | Redis (StackExchange.Redis) |
| Jobs agendados | Hangfire |
| WebSocket / Tempo real | SignalR (nativo ASP.NET Core) |
| Autenticação | JWT + 2FA (TOTP) |

### Frontend — `apps/web`
| Camada | Tecnologia |
|--------|-----------|
| Framework | Angular |

### IA e Processamento — `apps/ai-service`
| Função | Tecnologia |
|--------|-----------|
| NLP / Chatbot | Claude API (Anthropic) |
| Análise de sentimento | Hugging Face |
| OCR | Google Cloud Vision |
| Transcrição de áudio | OpenAI Whisper |
| Tradução | DeepL API |
| Framework | Python + FastAPI |

### Infraestrutura
| Camada | Tecnologia |
|--------|-----------|
| Containerização | Docker + Docker Compose |
| Storage (mídia) | AWS S3 / Cloudflare R2 |
| CI/CD | GitHub Actions |
| Monitoramento | Grafana + Prometheus |

---

## Arquitetura da API

### Padrão: Clean Architecture + DDD + CQRS + MediatR

```
ZenitWpp.Api           → Controllers, Requests, Hubs (SignalR), Middlewares
ZenitWpp.Application   → Commands, Queries, Handlers, Validators, DTOs
ZenitWpp.Domain        → Entidades, Value Objects, Domain Events, Interfaces
ZenitWpp.Infrastructure→ Repositories, EF Core, Redis, Hangfire, Integrações
```

**Regra de dependência:**
```
Api → Infrastructure → Application → Domain
```

### Domínios
| Domínio | Responsabilidade |
|---------|-----------------|
| Conversations | Atendimentos, mensagens, fila |
| Contacts | Clientes, perfis, histórico |
| Agents | Agentes humanos, times |
| Automation | Fluxos, gatilhos, respostas automáticas |
| Campaigns | Campanhas, segmentação, disparos |
| Notifications | Lembretes, agendamentos |
| Reports | Dashboard, métricas |

### Pipeline de uma requisição
```
HTTP Request
    │
    ▼
Controller → _mediator.Send(Command)
    │
    ▼ Pipeline MediatR
┌─────────────────────┐
│  LoggingBehavior    │
├─────────────────────┤
│  ValidationBehavior │ ← FluentValidation
├─────────────────────┤
│  Handler            │ ← caso de uso
└─────────────────────┘
    │
    ▼
Domain + Repository → Banco de Dados
    │
    ▼
Domain Events → Notificações, IA, WhatsApp
```

### Pacotes NuGet por camada
| Projeto | Pacotes |
|---------|---------|
| Application | MediatR, FluentValidation, Mapster |
| Infrastructure | EF Core, Npgsql, StackExchange.Redis, Hangfire, JWT Bearer, Newtonsoft.Json |

---

## Estrutura de Pastas

```
ZenitWpp/
├── apps/
│   ├── api/
│   │   ├── ZenitWpp.sln
│   │   ├── src/
│   │   │   ├── ZenitWpp.Domain/
│   │   │   ├── ZenitWpp.Application/
│   │   │   ├── ZenitWpp.Infrastructure/
│   │   │   └── ZenitWpp.Api/
│   │   └── tests/
│   │       ├── ZenitWpp.Domain.Tests/
│   │       ├── ZenitWpp.Application.Tests/
│   │       └── ZenitWpp.Integration.Tests/
│   ├── web/          # Angular
│   └── ai-service/   # Python + FastAPI
├── docs/
├── docker-compose.yml
└── README.md
```

---

## Fluxo de Interação

```
Cliente inicia conversa
        │
        ▼
┌───────────────────────────┐
│   Menu principal exibido  │
│   com botões de categoria │
└───────────┬───────────────┘
            │
            ▼
┌───────────────────────────┐
│  Cliente seleciona        │
│  categoria (ex: Suporte   │
│  Técnico)                 │
└───────────┬───────────────┘
            │
            ▼
┌───────────────────────────┐
│  Chatbot apresenta opções │
│  comuns ou solicita       │
│  descrição do problema    │
└───────────┬───────────────┘
            │
     ┌──────┴──────┐
     │             │
     ▼             ▼
 Resolução    Transferência
 automática   para agente
     │             │
     │             ▼
     │    ┌────────────────────┐
     │    │ Agente recebe      │
     │    │ histórico +        │
     │    │ sugestões de       │
     │    │ resposta           │
     │    └────────┬───────────┘
     │             │
     └──────┬──────┘
            │
            ▼
┌───────────────────────────┐
│  Pesquisa de satisfação   │
│  enviada ao cliente       │
└───────────┬───────────────┘
            │
            ▼
┌───────────────────────────┐
│  Sistema gera relatório   │
│  de desempenho            │
└───────────────────────────┘
```

| # | Etapa | Responsável |
|---|-------|-------------|
| 1 | Cliente inicia conversa e recebe menu principal com botões | Sistema |
| 2 | Seleciona categoria (ex: "Suporte Técnico") de uma lista | Cliente |
| 3 | Chatbot oferece opções comuns via botões ou solicita descrição do problema | Chatbot |
| 4 | Com base na resposta, o bot resolve automaticamente ou transfere para um agente | IA |
| 5 | Agente recebe histórico e sugestões de resposta | Agente |
| 6 | Após resolução, cliente recebe pesquisa de satisfação | Sistema |
| 7 | Sistema gera relatório de desempenho do atendimento | Sistema |

---

## Sprints

> Legenda: `[ ]` pendente · `[x]` concluído · `[~]` em andamento

---

### Sprint 0 — Fundação do Projeto
> Objetivo: estrutura base pronta, repositório configurado, solução compilando.

| Status | Task | Camada | Branch |
|--------|------|--------|--------|
| `[x]` | Criar repositório no GitHub | — | `main` |
| `[x]` | Configurar GitFlow (main + develop) | — | `main` |
| `[x]` | Criar monorepo com pastas `apps/`, `docs/` | — | `develop` |
| `[x]` | Criar `.gitignore` (.NET + Angular + Python) | — | `develop` |
| `[x]` | Inicializar solution ASP.NET Core | `api` | `develop` |
| `[x]` | Criar projetos Domain, Application, Infrastructure, Api | `api` | `develop` |
| `[x]` | Configurar referências entre projetos (Clean Architecture) | `api` | `develop` |
| `[x]` | Instalar pacotes NuGet (MediatR, FluentValidation, EF Core, Redis, Hangfire, JWT) | `api` | `develop` |
| `[x]` | Criar projetos de teste (Domain.Tests, Application.Tests, Integration.Tests) | `tests` | `develop` |

---

### Sprint 1 — Domain Layer ✓
> Objetivo: modelar o coração do sistema — entidades, value objects e eventos de domínio.
> Branch: `feature/domain-layer` | Commit: `feat(domain): implement domain layer`

#### Base
| Status | Task | Detalhe |
|--------|------|---------|
| `[x]` | Criar `BaseEntity` | Id (Guid), CreatedAt, UpdatedAt |
| `[x]` | Criar `AggregateRoot` | herda BaseEntity, gerencia Domain Events |
| `[x]` | Criar `IDomainEvent` + `BaseDomainEvent` | interface base + implementação abstrata |
| `[x]` | Criar `ValueObject` | base para objetos de valor (PhoneNumber, Email) |

#### Domínio: Conversations
| Status | Task | Detalhe |
|--------|------|---------|
| `[x]` | Criar entidade `Conversation` (Aggregate Root) | Id, Status, Channel, AssignedAgentId, ContactId |
| `[x]` | Criar entidade `Message` | Id, Content, Type, SentAt, Direction |
| `[x]` | Criar enum `ConversationStatus` | Open, InProgress, Waiting, Closed |
| `[x]` | Criar enum `MessageType` | Text, Image, Audio, Video, Document |
| `[x]` | Criar enum `MessageDirection` | Inbound, Outbound |
| `[x]` | Criar Domain Event `ConversationStartedEvent` | — |
| `[x]` | Criar Domain Event `MessageSentEvent` | — |
| `[x]` | Criar Domain Event `ConversationTransferredEvent` | — |
| `[x]` | Criar Domain Event `ConversationClosedEvent` | — |
| `[x]` | Criar interface `IConversationRepository` | — |

#### Domínio: Contacts
| Status | Task | Detalhe |
|--------|------|---------|
| `[x]` | Criar entidade `Contact` (Aggregate Root) | Id, Name, PhoneNumber, Email, Segment, Language |
| `[x]` | Criar Value Object `PhoneNumber` | validação e normalização de formato |
| `[x]` | Criar Value Object `Email` | validação e normalização (lowercase) |
| `[x]` | Criar Domain Event `ContactCreatedEvent` | — |
| `[x]` | Criar interface `IContactRepository` | GetByPhone incluso |

#### Domínio: Agents
| Status | Task | Detalhe |
|--------|------|---------|
| `[x]` | Criar entidade `Agent` (Aggregate Root) | Id, Name, Email, PasswordHash, Role, IsOnline, 2FA |
| `[x]` | Criar enum `AgentRole` | Operator, Supervisor, Admin |
| `[x]` | Criar Domain Event `AgentStatusChangedEvent` | — |
| `[x]` | Criar interface `IAgentRepository` | GetByEmail incluso |

#### Domínio: Automation
| Status | Task | Detalhe |
|--------|------|---------|
| `[x]` | Criar entidade `Flow` (Aggregate Root) | Id, Name, TriggerType, TriggerValue, IsActive |
| `[x]` | Criar entidade `FlowStep` | Id, Type, Content, Order, NextStepId |
| `[x]` | Criar enum `TriggerType` | Keyword, FirstMessage, OutsideHours |
| `[x]` | Criar enum `StepType` | SendMessage, ShowMenu, TransferToAgent, End |
| `[x]` | Criar interface `IFlowRepository` | FindByTrigger incluso |

#### Domínio: Campaigns
| Status | Task | Detalhe |
|--------|------|---------|
| `[x]` | Criar entidade `Campaign` (Aggregate Root) | Id, Name, Message, Status, Segment, ScheduledAt |
| `[x]` | Criar entidade `CampaignRecipient` | ContactId, Status, SentAt, ReadAt |
| `[x]` | Criar enum `CampaignStatus` | Draft, Scheduled, Running, Completed, Cancelled |
| `[x]` | Criar enum `RecipientStatus` | Pending, Sent, Failed, Read |
| `[x]` | Criar Domain Event `CampaignSentEvent` | — |
| `[x]` | Criar interface `ICampaignRepository` | — |

#### Domínio: Notifications
| Status | Task | Detalhe |
|--------|------|---------|
| `[x]` | Criar entidade `Reminder` | Id, ContactId, ConversationId, Message, ScheduledAt, Sent |
| `[x]` | Criar interface `IReminderRepository` | ListPending incluso |

---

### Sprint 2 — Infrastructure Layer ✓
> Objetivo: implementar persistência, cache e integrações externas.
> Branch: `feature/infrastructure-layer` | Commit: `feat(infrastructure): implement infrastructure layer`

#### Persistência
| Status | Task | Detalhe |
|--------|------|---------|
| `[x]` | Criar `AppDbContext` (EF Core) | registrar todas as entidades via `ApplyConfigurationsFromAssembly` |
| `[x]` | Criar configuração EF de `Conversation` | mapeamento de colunas, relacionamento com Messages |
| `[x]` | Criar configuração EF de `Message` | — |
| `[x]` | Criar configuração EF de `Contact` | OwnsOne PhoneNumber e Email (Value Objects) |
| `[x]` | Criar configuração EF de `Agent` | índice único no Email |
| `[x]` | Criar configuração EF de `Flow` + `FlowStep` | — |
| `[x]` | Criar configuração EF de `Campaign` + `CampaignRecipient` | — |
| `[x]` | Criar configuração EF de `Reminder` | — |
| `[x]` | Implementar `ConversationRepository` | com Include de Messages, paginação |
| `[x]` | Implementar `ContactRepository` | GetByPhone via Value Object |
| `[x]` | Implementar `AgentRepository` | GetByEmail normalizado |
| `[x]` | Implementar `FlowRepository` | FindByTrigger incluso |
| `[x]` | Implementar `CampaignRepository` | com Include de Recipients |
| `[x]` | Implementar `ReminderRepository` | ListPending filtra por data e Sent=false |
| `[ ]` | Criar primeira migration | aguardando ambiente com PostgreSQL |

#### Cache e Filas
| Status | Task | Detalhe |
|--------|------|---------|
| `[x]` | Configurar Redis (`IConnectionMultiplexer`) | registrado como Singleton |
| `[x]` | Criar `ICacheService` + `RedisCacheService` | Get, Set (TTL 30min padrão), Remove |
| `[x]` | Configurar Hangfire com PostgreSQL | via `Hangfire.PostgreSql` |

#### Auth
| Status | Task | Detalhe |
|--------|------|---------|
| `[x]` | Implementar `IJwtService` + `JwtService` | claims: agentId, email, role, jti |
| `[x]` | Implementar refresh token | gerado via `RandomNumberGenerator` |
| `[x]` | Implementar `ITotpService` + `TotpService` | HMAC-SHA1, janela de ±1 step, QR code URI |

#### Integrações
| Status | Task | Detalhe |
|--------|------|---------|
| `[x]` | Criar `IWhatsAppService` + `EvolutionApiService` | sendText, sendMedia, sendButtons |
| `[x]` | Criar `IAIService` + `ClaudeAIService` | sentimento, sugestão de resposta, classificação |
| `[x]` | Criar `IStorageService` + `NullStorageService` | placeholder — implementar S3/R2 na Sprint 7 |
| `[x]` | Criar `ITranslationService` + `NullTranslationService` | placeholder — implementar DeepL na Sprint 7 |
| `[x]` | Criar `DependencyInjection.cs` | extension method que registra toda a camada |

---

### Sprint 3 — Application Layer ✓
> Objetivo: implementar casos de uso via CQRS com MediatR.
> Branch: `feature/application-layer` | Commit: `feat(application): implement application layer with CQRS`

#### Behaviors (Pipeline MediatR)
| Status | Task | Detalhe |
|--------|------|---------|
| `[x]` | Criar `LoggingBehavior` | loga request, response e tempo de execução (ms) |
| `[x]` | Criar `ValidationBehavior` | executa FluentValidation antes de qualquer Handler |

#### Interfaces Comuns (Application)
| Status | Task | Detalhe |
|--------|------|---------|
| `[x]` | `ICurrentUser` | contrato para usuário autenticado na requisição |
| `[x]` | `IPasswordHasher` | contrato + `Argon2PasswordHasher` (Argon2id, 64MB RAM, 4 iterações) |
| `[x]` | `IWhatsAppService` | movida para Application — Clean Architecture compliance |

#### Conversations
| Status | Task | Detalhe |
|--------|------|---------|
| `[x]` | Command: `CreateConversationCommand` + Handler + Validator | — |
| `[x]` | Command: `SendMessageCommand` + Handler + Validator | — |
| `[x]` | Command: `TransferConversationCommand` + Handler | — |
| `[x]` | Command: `CloseConversationCommand` + Handler | — |
| `[x]` | Query: `GetConversationQuery` + Handler | — |
| `[x]` | Query: `ListConversationsQuery` + Handler | paginação |
| `[x]` | DTOs: `ConversationResponse`, `MessageResponse` | — |

#### Contacts
| Status | Task | Detalhe |
|--------|------|---------|
| `[x]` | Command: `CreateContactCommand` + Handler + Validator | — |
| `[x]` | Command: `UpdateContactCommand` + Handler | — |
| `[x]` | Query: `GetContactQuery` + Handler | — |
| `[x]` | DTOs: `ContactResponse` | — |

#### Agents
| Status | Task | Detalhe |
|--------|------|---------|
| `[x]` | Command: `CreateAgentCommand` + Handler + Validator | hash via Argon2id |
| `[x]` | Query: `GetAgentQuery` + Handler | — |
| `[x]` | DTOs: `AgentResponse` | — |
| `[x]` | Command: `LoginCommand` + Handler | JWT + 2FA |
| `[ ]` | Command: `UpdateAgentStatusCommand` + Handler | online/offline — Sprint 5 |

#### Campaigns
| Status | Task | Detalhe |
|--------|------|---------|
| `[x]` | Command: `CreateCampaignCommand` + Handler + Validator | segmentação de contatos |
| `[x]` | Command: `SendCampaignCommand` + Handler | disparo via WhatsApp, marca Sent/Failed |
| `[x]` | Query: `GetCampaignQuery` + Handler | — |
| `[x]` | DTOs: `CampaignResponse` | totais de enviados/falhas |

#### Reports
| Status | Task | Detalhe |
|--------|------|---------|
| `[x]` | Query: `GetDashboardQuery` + Handler | contagens por status de conversa |
| `[x]` | DTOs: `DashboardResponse` | — |

#### DI
| Status | Task | Detalhe |
|--------|------|---------|
| `[x]` | `DependencyInjection.cs` | registra MediatR, validators, LoggingBehavior e ValidationBehavior |

---

### Sprint 4 — API Layer ✅
> Objetivo: expor os casos de uso via HTTP e WebSocket.
> Branch: `feature/api-layer` | Commit: a definir

| Status | Task | Detalhe |
|--------|------|---------|
| `[x]` | Configurar `Program.cs` | DI, middlewares, Swagger (JWT Bearer), SignalR, CORS |
| `[x]` | Criar `GlobalExceptionMiddleware` | ValidationException→400, NotFound→404, Conflict→409, Unauthorized→401 |
| `[x]` | Criar `ConversationController` | list, get, create, send message, transfer, close |
| `[x]` | Criar `ContactController` | list, get, create, update |
| `[x]` | Criar `AgentController` | list, get, create |
| `[x]` | Criar `AuthController` | login (JWT + TOTP 2FA), setup 2FA |
| `[x]` | Criar `AutomationController` | list flows, create flow |
| `[x]` | Criar `CampaignController` | get, create, send |
| `[x]` | Criar `ReportController` | dashboard |
| `[x]` | Criar `WhatsAppWebhookController` | receber eventos da Evolution API |
| `[x]` | Criar `ChatHub` (SignalR) | JoinConversation, LeaveConversation, NotifyTyping |
| `[x]` | Configurar Swagger com autenticação JWT | Bearer token no header |
| `[x]` | Criar `docker-compose.yml` | API + PostgreSQL + Redis com health checks |
| `[x]` | Criar `Dockerfile` | multi-stage build sdk:10.0 → aspnet:10.0 |
| `[x]` | Mover `IJwtService`/`ITotpService` para Application | Clean Architecture — Application não depende de Infrastructure |
| `[x]` | Fixar compatibilidade `Microsoft.OpenApi` 2.x | `OpenApiSecuritySchemeReference`, `List<string>` no requirement |

---

### Sprint 5 — Frontend (Angular)
> Objetivo: interface do agente funcional com tempo real.
> Branch: `feature/frontend-angular` | Em andamento

| Status | Task | Detalhe |
|--------|------|---------|
| `[x]` | Inicializar projeto Angular em `apps/web` | Angular 19, Tailwind CSS v3, paleta Indigo + Slate |
| `[x]` | Configurar roteamento e autenticação (guards) | authGuard, publicGuard, authInterceptor JWT, lazy loading |
| `[x]` | Tela de login com 2FA | formulário reativo, campo TOTP condicional |
| `[x]` | Layout principal com sidebar de conversas | ShellComponent + SidenavComponent com toggle de tema |
| `[x]` | Componente de lista de conversas com fila | busca, filtros por status, avatar, badges, tempo real via SignalR |
| `[x]` | Componente de chat (envio/recebimento de mensagens) | bubbles inbound/outbound, separadores de data, indicador de digitação |
| `[ ]` | Suporte a mídia no chat (imagem, áudio, vídeo) | depende do storage — Sprint 7 |
| `[x]` | Painel de informações do contato | metadata da conversa, ações rápidas |
| `[x]` | Tela de dashboard com gráficos (métricas) | cards + donut por status + barras por hora (ngx-echarts) |
| `[x]` | Tela de gerenciamento de agentes | tabela com role badge, status online/offline, 2FA, form de criação |
| `[x]` | Tela de automação (flows) | lista de flows com trigger type, form com campo condicional |
| `[x]` | Tela de campanhas | criação, segmentação, disparo inline |
| `[x]` | Tela de relatórios | KPIs + line chart 7 dias + bar chart por agente |

---

### Sprint 6 — AI Service (Python)
> Objetivo: microserviço de IA consumido pela API.

| Status | Task | Detalhe |
|--------|------|---------|
| `[ ]` | Inicializar projeto FastAPI em `apps/ai-service` | — |
| `[ ]` | Endpoint: análise de sentimento | retorna score + label |
| `[ ]` | Endpoint: sugestão de resposta para agente | baseado no histórico |
| `[ ]` | Endpoint: classificação automática de conversa | categoria + urgência |
| `[ ]` | Integrar Claude API (Anthropic) | — |
| `[ ]` | Integrar Whisper (transcrição de áudio) | — |
| `[ ]` | Integrar Google Vision (OCR) | — |

---

### Sprint 7 — Qualidade e DevOps
> Objetivo: testes, CI/CD e preparação para produção.

| Status | Task | Detalhe |
|--------|------|---------|
| `[ ]` | Escrever testes unitários do Domain | entidades e value objects |
| `[ ]` | Escrever testes unitários da Application | handlers com mocks |
| `[ ]` | Escrever testes de integração | endpoints HTTP + banco real |
| `[ ]` | Configurar GitHub Actions (build + test) | roda em todo PR para develop |
| `[ ]` | Configurar GitHub Actions (deploy) | merge na main faz deploy |
| `[ ]` | Configurar variáveis de ambiente (.env.example) | — |
| `[ ]` | Documentar README com setup local | — |
| `[ ]` | Configurar Grafana + Prometheus | monitoramento em produção |
