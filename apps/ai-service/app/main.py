from fastapi import FastAPI
from fastapi.middleware.cors import CORSMiddleware
from app.routers import sentiment, suggestion, classification, transcription, ocr

app = FastAPI(
    title="ZenitWpp AI Service",
    description="AI-powered features: sentiment analysis, response suggestions, classification, transcription, and OCR",
    version="1.0.0",
)

app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

app.include_router(sentiment.router, prefix="/ai")
app.include_router(suggestion.router, prefix="/ai")
app.include_router(classification.router, prefix="/ai")
app.include_router(transcription.router, prefix="/ai")
app.include_router(ocr.router, prefix="/ai")


@app.get("/health")
async def health() -> dict:
    return {"status": "ok"}
