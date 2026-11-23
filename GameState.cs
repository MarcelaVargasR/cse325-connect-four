
namespace ConnectFour;

public class GameState
{
    public enum WinState
    {
        NoWinner,
        Player1_Wins,
        Player2_Wins,
        Tie
    }

    // Tablero de 6 filas y 7 columnas
    private readonly int[,] board = new int[6, 7];

    // Jugador actual
    public int PlayerTurn { get; private set; } = 1;

    // Número de fichas colocadas
    public int CurrentTurn { get; private set; } = 0;

    // Reiniciar el tablero
    public void ResetBoard()
    {
        Array.Clear(board);

        PlayerTurn = 1;
        CurrentTurn = 0;
    }

    // Colocar una ficha en una columna
    public int PlayPiece(byte col)
    {
        if (col >= 7)
        {
            throw new ArgumentException(
                "Invalid column."
            );
        }

        if (CheckForWin() != WinState.NoWinner)
        {
            throw new ArgumentException(
                "The game is over."
            );
        }

        // Buscar la posición libre más baja
        for (int row = 5; row >= 0; row--)
        {
            if (board[row, col] == 0)
            {
                board[row, col] = PlayerTurn;

                CurrentTurn++;

                // Cambiar de jugador
                PlayerTurn = PlayerTurn == 1 ? 2 : 1;

                // Devolver la fila para la animación
                return row + 1;
            }
        }

        throw new ArgumentException(
            "This column is full."
        );
    }

    // Comprobar si existe un ganador
    public WinState CheckForWin()
    {
        for (int row = 0; row < 6; row++)
        {
            for (int col = 0; col < 7; col++)
            {
                int player = board[row, col];

                if (player == 0)
                    continue;

                // Horizontal, vertical y diagonales
                if (
                    HasFour(row, col, 0, 1, player) ||
                    HasFour(row, col, 1, 0, player) ||
                    HasFour(row, col, 1, 1, player) ||
                    HasFour(row, col, 1, -1, player)
                )
                {
                    return player == 1
                        ? WinState.Player1_Wins
                        : WinState.Player2_Wins;
                }
            }
        }

        // Comprobar empate
        if (CurrentTurn == 42)
        {
            return WinState.Tie;
        }

        return WinState.NoWinner;
    }

    // Verificar cuatro fichas consecutivas
    private bool HasFour(
        int row,
        int col,
        int rowStep,
        int colStep,
        int player)
    {
        for (int i = 0; i < 4; i++)
        {
            int r = row + i * rowStep;
            int c = col + i * colStep;

            if (
                r < 0 || r >= 6 ||
                c < 0 || c >= 7 ||
                board[r, c] != player
            )
            {
                return false;
            }
        }

        return true;
    }
}
