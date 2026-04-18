from openai import AsyncOpenAI
from app.config import settings

client = AsyncOpenAI(api_key=settings.openai_api_key)


async def transcribe_audio(audio_bytes: bytes, filename: str, language: str = "pt") -> str:
    transcription = await client.audio.transcriptions.create(
        model=settings.whisper_model,
        file=(filename, audio_bytes),
        language=language,
    )
    return transcription.text
