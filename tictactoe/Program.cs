using System;

namespace tictactoe.Desktop
{ 
    /// <summary>
    ///
    /// Game Dev - Dec, 2018.
    ///
    /// Tymofii Nimtes. 
    ///     @ Conestoga College.
    ///
    /// </summary>
    public static class Program {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main( ) {
        
            using ( var game = new Game1( ) )
                game.Run( );
             
        }
        
    }
}
