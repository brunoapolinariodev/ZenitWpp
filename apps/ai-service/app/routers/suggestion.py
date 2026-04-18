from fastapi import APIRouter, Depends
from app.models.suggestion import SuggestionRequest, SuggestionResponse
from app.services.claude_service import suggest_responses
from app.dependencies import verify_api_key

router = APIRouter(prefix="/suggestion", tags=["suggestion"])


@router.post("", response_model=SuggestionResponse, dependencies=[Depends(verify_api_key)])
async def response_suggestion(request: SuggestionRequest) -> SuggestionResponse:
    return await suggest_responses(request)
