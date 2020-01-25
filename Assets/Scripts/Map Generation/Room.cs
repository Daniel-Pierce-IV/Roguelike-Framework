class Room
{
	public int x;
	public int y;
	public int x2;
	public int y2;
	public int width;
	public int height;

	// Width/height is total length, including walls
	// Actual walking area is (width - 2) * (height - 2)
	public Room(int x, int y, int width, int height)
	{
		this.x = x;
		this.y = y;
		this.width = width;
		this.height = height;
		this.x2 = x + width;
		this.y2 = y + height;
	}
}
