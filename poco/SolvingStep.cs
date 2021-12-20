public record struct SolvingStep
{
	public byte[,] Board { get; set; }
	public byte From { get; set; }
	public byte To { get; set; }
}