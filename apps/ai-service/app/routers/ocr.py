from fastapi import APIRouter, Depends, UploadFile, File
from pydantic import BaseModel
from app.services.vision_service import extract_text_from_image
from app.dependencies import verify_api_key

router = APIRouter(prefix="/ocr", tags=["ocr"])


class OcrResponse(BaseModel):
    text: str


@router.post("", response_model=OcrResponse, dependencies=[Depends(verify_api_key)])
async def ocr(file: UploadFile = File(...)) -> OcrResponse:
    image_bytes = await file.read()
    text = await extract_text_from_image(image_bytes)
    return OcrResponse(text=text)
