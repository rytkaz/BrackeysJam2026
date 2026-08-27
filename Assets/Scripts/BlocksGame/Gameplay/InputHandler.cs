using UnityEngine;
using UnityEngine.InputSystem;

namespace BlocksGame.Gameplay
{
    public class InputHandler
    {
        private readonly GameplayController gameplayController;

        public InputHandler(GameplayController controller)
        {
            gameplayController = controller;
            InputSystem.actions.FindAction(InputActions.Rotate).performed += context => gameplayController.ActivePiece.Rotate();
            InputSystem.actions.FindAction(InputActions.MoveLeft).performed += context => gameplayController.ActivePiece.MoveHorizontal(-1);
            InputSystem.actions.FindAction(InputActions.MoveRight).performed += context => gameplayController.ActivePiece.MoveHorizontal(1);
            InputSystem.actions.FindAction(InputActions.Pause).performed += context =>
            {
                //TODO: Implement pause
            };
            var dropAction = InputSystem.actions.FindAction(InputActions.Drop);
            dropAction.started += context => gameplayController.DropHeld = true;
            dropAction.canceled += context => gameplayController.DropHeld = false;
        }
    }
}
        

