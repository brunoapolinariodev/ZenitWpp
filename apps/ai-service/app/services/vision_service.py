from google.cloud import vision


async def extract_text_from_image(image_bytes: bytes) -> str:
    client = vision.ImageAnnotatorClient()
    image = vision.Image(content=image_bytes)
    response = client.text_detection(image=image)

    if response.error.message:
        raise RuntimeError(f"Google Vision error: {response.error.message}")

    annotations = response.text_annotations
    return annotations[0].description.strip() if annotations else ""
