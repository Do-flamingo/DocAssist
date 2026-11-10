def cosinus(a: np.ndarray, b: np.ndarray) -> float:
    return float(np.dot(a, b) / (np.linalg.norm(a) * np.linalg.norm(b)))


if __name__ == "__main__":
    # ... phrases ...
    francaises = [p.texte for p in phrases if p.langue == "fr"]
    print(f"{len(francaises)} phrases en français")

    print(Counter(p.langue for p in phrases))

    a = np.array([1.0, 0.0])
    for b in ([1.0, 0.0], [1.0, 1.0], [0.0, 1.0], [-1.0, 0.0]):
        print(f"cos((1,0), {tuple(b)}) = {cosinus(a, np.array(b)):.2f}")

    try:
        cosinus(np.array([1.0, 0.0]), np.array([1.0, 0.0, 0.0]))
    except ValueError as erreur:
        print(f"Erreur attendue : {erreur}")