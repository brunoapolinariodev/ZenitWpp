from pydantic_settings import BaseSettings, SettingsConfigDict


class Settings(BaseSettings):
    model_config = SettingsConfigDict(env_file=".env", env_file_encoding="utf-8", extra="ignore")

    openrouter_api_key: str = ""
    openrouter_base_url: str = "https://openrouter.ai/api/v1"
    openrouter_model: str = "anthropic/claude-sonnet-4-5"

    openai_api_key: str = ""
    whisper_model: str = "whisper-1"

    google_application_credentials: str = ""

    api_key: str = "changeme"  # chave para proteger o serviço internamente


settings = Settings()
