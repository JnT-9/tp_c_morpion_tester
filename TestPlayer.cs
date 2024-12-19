using TicTacToe;

namespace tp_c_morpion_tester;

public class TestPlayer
{
    [Fact]
    public void Test1()
    {
        IPlayer player = new HumanPlayer();
    }
}
