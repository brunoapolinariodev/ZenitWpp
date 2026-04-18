from fastapi import APIRouter, Depends
from app.models.sentiment import SentimentRequest, SentimentResponse
from app.services.claude_service import analyze_sentiment
from app.dependencies import verify_api_key

router = APIRouter(prefix="/sentiment", tags=["sentiment"])


@router.post("", response_model=SentimentResponse, dependencies=[Depends(verify_api_key)])
async def sentiment_analysis(request: SentimentRequest) -> SentimentResponse:
    return await analyze_sentiment(request.text, request.language)
