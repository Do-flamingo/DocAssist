import json
import os
from pathlib import Path

from azure.identity import AzureCliCredential, get_bearer_token_provider
from openai import OpenAI

DOSSIER = Path(__file__).parent

PROMPT_SYSTEME = """Tu es DocAssist, un assistant expert de la documentation officielle de Kubernetes.
Réponds uniquement en JSON, avec exactement ces clés :
- "hors_sujet" : true si la question ne concerne pas Kubernetes, sinon false
- "reponse" : ta réponse en français, 3 phrases maximum
N'invente jamais : si tu n'es pas sûr, dis-le dans la réponse."""


def creer_client() -> OpenAI:
    ressource = os.environ["AZURE_RESOURCE_NAME"]
    fournisseur_jeton = get_bearer_token_provider(
        AzureCliCredential(), "https://ai.azure.com/.default"
    )
    return OpenAI(
        base_url=f"https://{ressource}.openai.azure.com/openai/v1/",
        api_key=fournisseur_jeton,
    )


def interroger(client: OpenAI, deploiement: str, question: str) -> dict:
    reponse = client.chat.completions.create(
        model=deploiement,
        messages=[
            {"role": "system", "content": PROMPT_SYSTEME},
            {"role": "user", "content": question},
        ],
        response_format={"type": "json_object"},
        max_completion_tokens=2000,
    )

    contenu = json.loads(reponse.choices[0].message.content)
    contenu["tokens"] = {
        "envoyes": reponse.usage.prompt_tokens,
        "recus": reponse.usage.completion_tokens,
    }
    return contenu


if __name__ == "__main__":
    deploiement = os.environ["AZURE_OPENAI_GPT_NAME"]
    client = creer_client()

    questions = json.loads((DOSSIER / "questions.json").read_text(encoding="utf-8"))

    resultats = []
    for cas in questions:
        obtenu = interroger(client, deploiement, cas["question"])
        correct = obtenu["hors_sujet"] == cas["hors_sujet_attendu"]
        resultats.append({**cas, "obtenu": obtenu, "correct": correct})
        print(f"{'OK' if correct else 'KO'}  {cas['question']}")

    nb_corrects = sum(r["correct"] for r in resultats)
    print(f"\nScore : {nb_corrects}/{len(resultats)}")

    (DOSSIER / "resultats.json").write_text(
        json.dumps(resultats, ensure_ascii=False, indent=2), encoding="utf-8"
    )