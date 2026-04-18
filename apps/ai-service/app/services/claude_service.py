import json
from openai import AsyncOpenAI
from app.config import settings
from app.models.sentiment import SentimentResponse
from app.models.suggestion import SuggestionRequest, SuggestionResponse
from app.models.classification import ClassificationResponse

client = AsyncOpenAI(
    api_key=settings.openrouter_api_key,
    base_url=settings.openrouter_base_url,
)


async def analyze_sentiment(text: str, language: str) -> SentimentResponse:
    prompt = f"""Analise o sentimento do seguinte texto de atendimento ao cliente.
Idioma: {language}
Texto: "{text}"

Responda SOMENTE com JSON válido no formato:
{{
  "label": "positive" | "neutral" | "negative",
  "score": <float entre 0.0 e 1.0 representando a intensidade>,
  "summary": "<frase curta explicando o sentimento>"
}}"""

    response = await client.chat.completions.create(
        model=settings.openrouter_model,
        max_tokens=256,
        messages=[{"role": "user", "content": prompt}],
    )

    data = json.loads(response.choices[0].message.content)
    return SentimentResponse(**data)


async def suggest_responses(request: SuggestionRequest) -> SuggestionResponse:
    history = "\n".join(
        f"{'Cliente' if m.role == 'customer' else 'Agente'}: {m.content}"
        for m in request.conversation_history
    )

    prompt = f"""Você é um assistente de atendimento ao cliente. Com base no histórico de conversa abaixo,
gere 3 sugestões de resposta para o agente humano. As respostas devem ser profissionais,
empáticas e objetivas. Idioma: {request.language}.
{f'Nome do cliente: {request.contact_name}.' if request.contact_name else ''}

Histórico:
{history}

Responda SOMENTE com JSON válido no formato:
{{
  "suggestions": ["<resposta 1>", "<resposta 2>", "<resposta 3>"]
}}"""

    response = await client.chat.completions.create(
        model=settings.openrouter_model,
        max_tokens=512,
        messages=[{"role": "user", "content": prompt}],
    )

    data = json.loads(response.choices[0].message.content)
    return SuggestionResponse(**data)


async def classify_conversation(text: str, language: str) -> ClassificationResponse:
    prompt = f"""Classifique a mensagem de atendimento ao cliente abaixo.
Idioma: {language}
Mensagem: "{text}"

Categorias possíveis: Suporte Técnico, Financeiro, Comercial, Reclamação, Elogio, Dúvida Geral, Cancelamento, Outros.
Urgência: low (informativo), medium (precisa de atenção), high (urgente), critical (emergência).

Responda SOMENTE com JSON válido no formato:
{{
  "category": "<categoria>",
  "urgency": "low" | "medium" | "high" | "critical",
  "confidence": <float entre 0.0 e 1.0>,
  "tags": ["<tag1>", "<tag2>"]
}}"""

    response = await client.chat.completions.create(
        model=settings.openrouter_model,
        max_tokens=256,
        messages=[{"role": "user", "content": prompt}],
    )

    data = json.loads(response.choices[0].message.content)
    return ClassificationResponse(**data)
