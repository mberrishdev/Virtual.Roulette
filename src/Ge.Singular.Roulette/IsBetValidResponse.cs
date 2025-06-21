namespace ge.singular.roulette;

public class IsBetValidResponse
{
	private bool isValid = false;

	private long betAmount = 0L;

	public void setIsValid(bool isValid)
	{
		this.isValid = isValid;
	}

	public void setBetAmount(long amount)
	{
		betAmount = amount;
	}

	public bool getIsValid()
	{
		return isValid;
	}

	public long getBetAmount()
	{
		return betAmount;
	}
}
