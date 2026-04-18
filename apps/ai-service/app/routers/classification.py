from fastapi import APIRouter, Depends
from app.models.classification import ClassificationRequest, ClassificationResponse
from app.services.claude_service import classify_conversation
from app.dependencies import verify_api_key

router = APIRouter(prefix="/classification", tags=["classification"])


@router.post("", response_model=ClassificationResponse, dependencies=[Depends(verify_api_key)])
async def conversation_classification(request: ClassificationRequest) -> ClassificationResponse:
    return await classify_conversation(request.text, request.language)
