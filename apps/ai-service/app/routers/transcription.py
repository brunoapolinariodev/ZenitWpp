from fastapi import APIRouter, Depends, UploadFile, File, Form
from pydantic import BaseModel
from app.services.whisper_service import transcribe_audio
from app.dependencies import verify_api_key

router = APIRouter(prefix="/transcription", tags=["transcription"])


class TranscriptionResponse(BaseModel):
    text: str


@router.post("", response_model=TranscriptionResponse, dependencies=[Depends(verify_api_key)])
async def transcribe(
    file: UploadFile = File(...),
    language: str = Form(default="pt"),
) -> TranscriptionResponse:
    audio_bytes = await file.read()
    text = await transcribe_audio(audio_bytes, file.filename or "audio.ogg", language)
    return TranscriptionResponse(text=text)
