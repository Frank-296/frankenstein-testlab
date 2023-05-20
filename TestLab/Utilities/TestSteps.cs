#nullable disable
namespace TestLab.Utilities;

public class TestSteps
{
	private static Int32 step = 0;
	public static Int32 Step { get => step; set => step = value; }

    public static void SetStepNumber() => Step++;
}