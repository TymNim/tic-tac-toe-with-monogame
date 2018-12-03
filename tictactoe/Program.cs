using System;

namespace tictactoe.Desktop
{ 
    /// <summary>
    ///
    /// Assignment 3.
    /// Game Dev - Dec, 2018 
    ///
    /// Tymofii Nimtes 
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
