"""
Micro-service IA — détection d'anomalies et prédiction de consommation.
100% gratuit / open-source (FastAPI + NumPy), sans dépendance à une API payante.
Peut être étendu avec ML.NET, scikit-learn ou un modèle Ollama local.
"""
from fastapi import FastAPI
from pydantic import BaseModel
from typing import List
import numpy as np

app = FastAPI(title="Smart Metering AI Service")


class AnomalyRequest(BaseModel):
    history: List[float]
    current: float


class AnomalyResponse(BaseModel):
    isAnomaly: bool
    score: float


class PredictRequest(BaseModel):
    history: List[float]


class PredictResponse(BaseModel):
    predictedNextValue: float


@app.get("/health")
def health():
    return {"status": "ok"}


@app.post("/anomaly", response_model=AnomalyResponse)
def detect_anomaly(req: AnomalyRequest):
    """Détection simple par score-Z : signale une valeur au-delà de 3 écarts-types."""
    if len(req.history) < 5:
        return AnomalyResponse(isAnomaly=False, score=0.0)

    values = np.array(req.history)
    mean = float(np.mean(values))
    std = float(np.std(values)) or 1e-6
    z_score = abs(req.current - mean) / std

    return AnomalyResponse(isAnomaly=bool(z_score > 3), score=round(z_score, 3))


@app.post("/predict", response_model=PredictResponse)
def predict_next(req: PredictRequest):
    """Prédiction naïve par moyenne mobile pondérée (à remplacer par un modèle ML.NET entraîné)."""
    if not req.history:
        return PredictResponse(predictedNextValue=0.0)

    values = np.array(req.history[-10:])
    weights = np.arange(1, len(values) + 1)
    predicted = float(np.average(values, weights=weights))

    return PredictResponse(predictedNextValue=round(predicted, 3))
