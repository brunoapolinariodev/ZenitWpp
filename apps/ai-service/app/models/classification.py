from pydantic import BaseModel
from typing import Literal


class ClassificationRequest(BaseModel):
    text: str
    language: str = "pt"


class ClassificationResponse(BaseModel):
    category: str   # ex: "Suporte Técnico", "Financeiro", "Comercial"
    urgency: Literal["low", "medium", "high", "critical"]
    confidence: float  # 0.0 a 1.0
    tags: list[str]
