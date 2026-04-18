from pydantic import BaseModel
from typing import Literal


class Message(BaseModel):
    role: Literal["customer", "agent"]
    content: str


class SuggestionRequest(BaseModel):
    conversation_history: list[Message]
    contact_name: str = ""
    language: str = "pt"


class SuggestionResponse(BaseModel):
    suggestions: list[str]  # 3 sugestões de resposta ordenadas por relevância
