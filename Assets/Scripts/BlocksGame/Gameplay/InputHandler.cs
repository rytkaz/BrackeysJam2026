using UnityEngine;
using UnityEngine.InputSystem;
using R3;

namespace BlocksGame.Gameplay
{
    public class InputHandler
    {
        private readonly GameplayController gameplayController;

        public InputHandler(GameplayController controller)
        {
            gameplayController = controller;
            InputSystem.actions.FindAction(InputActions.Rotate).performed += OnRotate;
            InputSystem.actions.FindAction(InputActions.MoveLeft).performed += OnMoveLeft;
            InputSystem.actions.FindAction(InputActions.MoveRight).performed += OnMoveRight;
            InputSystem.actions.FindAction(InputActions.Pause).performed += OnPause;
            var dropAction = InputSystem.actions.FindAction(InputActions.Drop);
            dropAction.started += OnDropStart;
            dropAction.canceled += OnDropCancel;
        }

        private void OnDropCancel(InputAction.CallbackContext context)
        {
            gameplayController.DropHeld = false;
        }

        private void OnDropStart(InputAction.CallbackContext context)
        {
            gameplayController.DropHeld = true;
        }

        private void OnPause(InputAction.CallbackContext context)
        {
            //TODO: Implement pause
        }

        private void OnRotate(InputAction.CallbackContext context)
        {
            gameplayController.ActivePiece.Rotate();
        }

        private void OnMoveLeft(InputAction.CallbackContext context)
        {
            gameplayController.ActivePiece.MoveHorizontal(-1);
        }

        private void OnMoveRight(InputAction.CallbackContext context)
        {
            gameplayController.ActivePiece.MoveHorizontal(1);
        }

        public void Cleanup()
        {
            InputSystem.actions.FindAction(InputActions.Rotate).performed -= OnRotate;
            InputSystem.actions.FindAction(InputActions.MoveLeft).performed -= OnMoveLeft;
            InputSystem.actions.FindAction(InputActions.MoveRight).performed -= OnMoveRight;
            InputSystem.actions.FindAction(InputActions.Pause).performed -= OnPause;
            var dropAction = InputSystem.actions.FindAction(InputActions.Drop);
            dropAction.started -= OnDropStart;
            dropAction.canceled -= OnDropCancel;
        }
    }
}
        

