public class StringApproximation
{
	public StringApproximation() { }

	// Calculate Levenshtein distance between two strings
	public int LevenshteinDistance(string source, string target)
	{
		if (string.IsNullOrEmpty(source))
			return target?.Length ?? 0;
		if (string.IsNullOrEmpty(target))
			return source.Length;

		int[,] distance = new int[source.Length + 1, target.Length + 1];

		for (int i = 0; i <= source.Length; i++)
			distance[i, 0] = i;
		for (int j = 0; j <= target.Length; j++)
			distance[0, j] = j;

		for (int i = 1; i <= source.Length; i++)
		{
			for (int j = 1; j <= target.Length; j++)
			{
				int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;
				distance[i, j] = Math.Min(
					Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1),
					distance[i - 1, j - 1] + cost);
			}
		}

		return distance[source.Length, target.Length];
	}

	// Calculate similarity percentage
	public double CalculateSimilarity(string source, string target)
	{
		int maxLength = Math.Max(source.Length, target.Length);
		if (maxLength == 0) return 100.0;

		int distance = LevenshteinDistance(source.ToLower(), target.ToLower());
		return (1.0 - (double)distance / maxLength) * 100.0;
	}

	// Find best match from a list of strings
	public (string Match, double Similarity) FindBestMatch(string input, List<string> candidates)
	{
		string bestMatch = null;
		double bestSimilarity = 0;

		foreach (var candidate in candidates)
		{
			double similarity = CalculateSimilarity(input, candidate);

			if (similarity > bestSimilarity)
			{
				bestSimilarity = similarity;
				bestMatch = candidate;
			}
		}

		return (bestMatch, bestSimilarity);
	}

	// Find all matches above a threshold
	public List<(string Country, double Similarity)> FindMatches(
		string input, List<string> candidates, double threshold = 70.0)
	{
		return [.. candidates
			.Select(c => (Country: c, Similarity: CalculateSimilarity(input, c)))
			.Where(x => x.Similarity >= threshold)
			.OrderByDescending(x => x.Similarity)];
	}
}