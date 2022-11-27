
using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

namespace LittleFoxLite
{
	public class InputManager : MonoBehaviour
	{
		[Header("Character Input Values")]
		public Vector2 move;
		public Vector2 look;
		public bool jump;
		public bool sprint;
		public bool Fire1;
		[Header("Movement Settings")]
		public bool analogMovement;

		[Header("Mouse Cursor Settings")]
		public bool cursorLocked = true;
		public bool cursorInputForLook = true;

#if ENABLE_INPUT_SYSTEM
        public void OnExitEvent()
        {
			InteractiveHandel.instance.ChangeToNormalState();
			if (InteractiveHandel.instance.isInDialogue)
			{
				InteractiveHandel.instance.OutDialoguePerform();
			}
        }
        public void OnEsc(InputValue value)
        {
			MasterMenuManager.instance.OnESCPressed();
        }
        public void OnEvent(InputValue value)
        {
            InteractiveHandel.instance.PerformInteractive();
        }
        public void OnMove(InputValue value)
		{
			MoveInput(value.Get<Vector2>());
		}

		public void OnLook(InputValue value)
		{
			if(cursorInputForLook)
			{
				LookInput(value.Get<Vector2>());
			}
		}

		public void OnJump(InputValue value)
		{
			JumpInput(value.isPressed);
			PlayerController.Instance.jumpAction?.Invoke();
		}

		public void OnSprint(InputValue value)
		{
			SprintInput(value.isPressed);
		}
		public void OnSwitchState(InputValue value)
		{
			if (value.isPressed)
				PlayerController.Instance.ChangeBetweenToState();
		}
		public void OnShoot (InputValue value)
		{
			ShootInput(value.isPressed);
		}
		public void OnReload(InputValue value)
		{
			if (value.isPressed)
				PlayerController.Instance.currentWeapon.OnReload();
		}

#endif


		public void MoveInput(Vector2 newMoveDirection)
		{
			move = newMoveDirection;
		} 

		public void LookInput(Vector2 newLookDirection)
		{
			look = newLookDirection;
		}

		public void JumpInput(bool newJumpState)
		{
			jump = newJumpState;
		}

		public void SprintInput(bool newSprintState)
		{
			sprint = newSprintState;
		}
		public void ShootInput(bool newShootState)
		{
			Fire1 = newShootState;
		}

		//private void OnApplicationFocus(bool hasFocus)
		//{
		//	SetCursorState(cursorLocked);
		//}

		//private void SetCursorState(bool newState)
		//{
		//	Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
		//}
	}
	
}