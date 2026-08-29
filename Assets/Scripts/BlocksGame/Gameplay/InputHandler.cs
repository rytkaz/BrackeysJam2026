using System;
using BlocksGame.UI;
using MessagePipe;
using R3;
using UnityEngine.InputSystem;

namespace BlocksGame.Gameplay
{
    public class InputHandler
    {
        private readonly GameplayController gameplayController;

        private IDisposable horizontalMoveRepeat;
        private bool isLeftPressed = false;
        private bool isRightPressed = false;
        
        public InputHandler(GameplayController controller)
        {
            gameplayController = controller;
            InputSystem.actions.FindAction(InputActions.Rotate).performed += OnRotate;
            InputSystem.actions.FindAction(InputActions.Pause).performed += OnPause;
            var moveLeftAction = InputSystem.actions.FindAction(InputActions.MoveLeft);
            moveLeftAction.started += StartMoveLeft;
            moveLeftAction.canceled += StopMoveLeft;
            var moveRightAction = InputSystem.actions.FindAction(InputActions.MoveRight);
            moveRightAction.started += StartMoveRight;
            moveRightAction.canceled += StopMoveRight;
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
            GlobalMessagePipe.GetPublisher<MToggleGameUI>().Publish(new MToggleGameUI() { ScreenType = GameUIScreenType.Pause });
        }

        private void OnRotate(InputAction.CallbackContext context)
        {
            gameplayController.ActivePiece.Rotate();
        }
        
        private void StartMoveLeft(InputAction.CallbackContext context)
        {
            isLeftPressed = true;
            StartHorizontalMove(-1);
        }

        private void StopMoveLeft(InputAction.CallbackContext context)
        {
            isLeftPressed = false;
            StopHorizontalMove();
            if (isRightPressed)
            {
                StartHorizontalMove(1);
            }
        }

        private void StartMoveRight(InputAction.CallbackContext context)
        {
            isRightPressed = true;
            StartHorizontalMove(1);
        }

        private void StopMoveRight(InputAction.CallbackContext context)
        {
            isRightPressed = false;
            StopHorizontalMove();
            if (isLeftPressed)
            {
                StartHorizontalMove(-1);
            }
        }

        private void StartHorizontalMove(int direction)
        {
            horizontalMoveRepeat?.Dispose();
            gameplayController.ActivePiece.MoveHorizontal(direction);
            horizontalMoveRepeat = Observable.Interval(TimeSpan.FromSeconds(0.1f)).Subscribe(_ =>
            {
                if (gameplayController.ActivePiece != null && gameplayController.TickGameplay)
                {
                    gameplayController.ActivePiece.MoveHorizontal(direction);
                }
            });
        }

        private void StopHorizontalMove()
        {
            horizontalMoveRepeat?.Dispose();
        }

        public void Cleanup()
        {
            horizontalMoveRepeat?.Dispose();
            InputSystem.actions.FindAction(InputActions.Rotate).performed -= OnRotate;
            InputSystem.actions.FindAction(InputActions.Pause).performed -= OnPause;
            var moveLeftAction = InputSystem.actions.FindAction(InputActions.MoveLeft);
            moveLeftAction.started -= StartMoveLeft;
            moveLeftAction.canceled -= StopMoveLeft;
            var moveRightAction = InputSystem.actions.FindAction(InputActions.MoveRight);
            moveRightAction.started -= StartMoveLeft;
            moveRightAction.canceled -= StopMoveLeft;
            var dropAction = InputSystem.actions.FindAction(InputActions.Drop);
            dropAction.started -= OnDropStart;
            dropAction.canceled -= OnDropCancel;
        }
    }
}
        

