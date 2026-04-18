<div align="center">

# ZenitWpp

<a href="https://freeimage.host/i/Bg5OwyG">
  <img src="https://iili.io/Bg5OwyG.md.png" alt="ZenitWpp Logo" width="160">
</a>

### Plataforma escalável para orquestração de fluxos e atendimento via WhatsApp com agentes.

*Solução Full Stack para gerenciamento de filas, integração de LLMs e processamento de dados em tempo real.*

---
</div>

## Stack

| Camada | Tecnologia |
|---|---|
| Backend API | ASP.NET Core 10 — Clean Architecture + DDD + CQRS + MediatR |
| Frontend | Angular 19 — Tailwind CSS v3, SignalR, ngx-echarts |
| AI Service | Python 3.12 + FastAPI — OpenRouter, Whisper, Google Vision |
| Banco de dados | PostgreSQL 16 |
| Cache / Filas | Redis 7 |
| Storage de mídia | Local (dev) / AWS S3 / Cloudflare R2 (prod) |
| Containerização | Docker + Docker Compose |

---

## Estrutura do projeto

```
ZenitWpp/
├── apps/
│   ├── api/              # ASP.NET Core — Clean Architecture
│   │   └── src/
│   │       ├── ZenitWpp.Domain/
│   │       ├── ZenitWpp.Application/
│   │       ├── ZenitWpp.Infrastructure/
│   │       └── ZenitWpp.Api/
│   ├── web/              # Angular 19
│   └── ai-service/       # Python + FastAPI
├── credentials/          # Credenciais GCP (não versionado)
├── docker-compose.yml
└── README.md
```

---

## Pré-requisitos

- [Docker](https://www.docker.com/) e Docker Compose
- [.NET 10 SDK](https://dotnet.microsoft.com/) (para rodar a API localmente)
- [Node.js 22+](https://nodejs.org/) (para rodar o frontend localmente)
- [Python 3.12+](https://www.python.org/) (para rodar o ai-service localmente)

---

## Setup rápido com Docker

**1. Clone o repositório**

```bash
git clone git@github.com:brunoapolinariodev/ZenitWpp.git
cd ZenitWpp
```

**2. Configure as variáveis de ambiente**

Crie um arquivo `.env` na raiz do projeto:

```env
# AI Service
OPENROUTER_API_KEY=your-openrouter-api-key
OPENROUTER_MODEL=anthropic/claude-sonnet-4-5
OPENAI_API_KEY=your-openai-api-key
AI_SERVICE_API_KEY=changeme

# Storage (opcional — padrão: local)
STORAGE_PROVIDER=local
# Para S3/R2:
# STORAGE_PROVIDER=s3
# S3_BUCKET=zenitwpp
# S3_SERVICE_URL=https://<account-id>.r2.cloudflarestorage.com
# S3_ACCESS_KEY=...
# S3_SECRET_KEY=...
# S3_PUBLIC_URL=https://cdn.seudominio.com
```

**3. (Opcional) Credenciais Google Cloud Vision**

Coloque o arquivo JSON da service account em `credentials/gcp-vision.json`.

**4. Suba os serviços**

```bash
docker-compose up --build
```

| Serviço | URL |
|---|---|
| Frontend | http://localhost:4200 |
| API | http://localhost:5000 |
| AI Service | http://localhost:8000 |
| Swagger | http://localhost:5000/swagger |
| PostgreSQL | localhost:5432 |
| Redis | localhost:6379 |

**Credenciais do admin criado automaticamente:**

```
Email: admin@zenitwpp.com
Senha: Admin@123
```

---

## Setup local (sem Docker)

### API (.NET)

```bash
cd apps/api

# Suba o banco e o Redis via Docker
docker-compose up postgres redis -d

# Rode a API
dotnet run --project src/ZenitWpp.Api
```

### Frontend (Angular)

```bash
cd apps/web
npm install
npm start
```

Acesse em http://localhost:4200.

### AI Service (Python)

```bash
cd apps/ai-service

# Crie o ambiente virtual
python -m venv .venv
.venv\Scripts\activate   # Windows
# source .venv/bin/activate  # Linux/macOS

pip install -r requirements.txt

# Configure as variáveis
cp .env.example .env
# edite o .env com suas chaves

uvicorn app.main:app --reload --port 8000
```

---

## Endpoints principais

### API — `http://localhost:5000`

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/api/auth/login` | Login com JWT + 2FA opcional |
| `GET` | `/api/conversations` | Listar conversas (paginado) |
| `POST` | `/api/conversations/{id}/messages` | Enviar mensagem de texto |
| `POST` | `/api/conversations/{id}/messages/media` | Upload de mídia (multipart) |
| `PATCH` | `/api/conversations/{id}/transfer` | Transferir para agente |
| `PATCH` | `/api/conversations/{id}/close` | Fechar conversa |
| `GET` | `/api/reports/dashboard` | Métricas em tempo real |
| `PATCH` | `/api/agents/{id}/status` | Alternar online/offline |
| `GET` | `/health` | Health check |

### AI Service — `http://localhost:8000`

Todos os endpoints requerem o header `X-API-Key`.

| Método | Rota | Descrição |
|---|---|---|
| `POST` | `/ai/sentiment` | Análise de sentimento |
| `POST` | `/ai/suggestion` | Sugestões de resposta para o agente |
| `POST` | `/ai/classification` | Classificação da conversa |
| `POST` | `/ai/transcription` | Transcrição de áudio (Whisper) |
| `POST` | `/ai/ocr` | Extração de texto de imagem |
| `GET` | `/health` | Health check |

---

## Storage de mídia

Por padrão o storage é local — os arquivos ficam em `apps/api/wwwroot/uploads/` e são servidos em `http://localhost:5000/uploads/...`.

Para usar S3 ou Cloudflare R2 em produção, altere `STORAGE_PROVIDER=s3` e configure as variáveis `S3_*` no `.env`.

---

## WebSocket (SignalR)

O hub de chat está disponível em `ws://localhost:5000/hubs/chat`.

```js
// Entrar em uma conversa
connection.invoke("JoinConversation", conversationId)

// Notificar digitação
connection.invoke("NotifyTyping", conversationId)
```

---

## Variáveis de ambiente — referência completa

| Variável | Padrão | Descrição |
|---|---|---|
| `OPENROUTER_API_KEY` | — | Chave da API OpenRouter |
| `OPENROUTER_MODEL` | `anthropic/claude-sonnet-4-5` | Modelo LLM usado |
| `OPENAI_API_KEY` | — | Chave OpenAI (Whisper) |
| `AI_SERVICE_API_KEY` | `changeme` | Chave interna entre API e AI Service |
| `STORAGE_PROVIDER` | `local` | `local` ou `s3` |
| `S3_BUCKET` | — | Nome do bucket |
| `S3_SERVICE_URL` | — | Endpoint customizado (R2/MinIO) |
| `S3_ACCESS_KEY` | — | Access key S3 |
| `S3_SECRET_KEY` | — | Secret key S3 |
| `S3_PUBLIC_URL` | — | URL pública do CDN |
