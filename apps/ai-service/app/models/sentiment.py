from pydantic import BaseModel
from typing import Literal


class SentimentRequest(BaseModel):
    text: str
    language: str = "pt"


class SentimentResponse(BaseModel):
    label: Literal["positive", "neutral", "negative"]
    score: float  # 0.0 a 1.0
    summary: str
